import 'dart:developer';

import 'package:xego_rider/models/Dto/vehicle_response_dto.dart';
import 'package:xego_rider/models/Dto/vehicle_type_dto.dart';
import 'package:xego_rider/models/Dto/vehicle_type_calculated_price_info_dto.dart';
import 'package:xego_rider/models/Entities/vehicle.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/services/driver_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class VehicleServices {
  final apiServices = ApiServices();

  Future<List<Vehicle>> getAllVehicles({
    int? id,
    String? plateNumber,
    String? type,
    bool? isActive,
    bool? isAssigned,
    String? createdBy,
    DateTime? createdStartDate,
    DateTime? createdEndDate,
    String? lastModifiedBy,
    DateTime? lastModifiedStartDate,
    DateTime? lastModifiedEndDate,
    String? searchString,
    int pageNumber = 0,
    int pageSize = 0,
  }) async {
    try {
      const subApiUrl = 'api/vehicles';
      final url = Uri.http(KSecret.kApiIp, subApiUrl, {
        'id': id,
        'plateNumber': plateNumber,
        'type': type,
        'isActive': isActive,
        'isAssigned': isAssigned,
        'createdBy': createdBy,
        'createdStartDate': createdStartDate?.toIso8601String(),
        'createdEndDate': createdEndDate?.toIso8601String(),
        'lastModifiedBy': lastModifiedBy,
        'lastModifiedStartDate': lastModifiedStartDate?.toIso8601String(),
        'lastModifiedEndDate': lastModifiedEndDate?.toIso8601String(),
        'searchString': searchString,
        'pageNumber': pageNumber,
        'pageSize': pageSize,
      });

      final response = await apiServices.get(url.toString());

      if (response.statusCode != 200) {
        return [];
      }

      VehicleResponseDto vehicleResponseDto =
          VehicleResponseDto.fromJson(response.data);

      if (!vehicleResponseDto.isSuccess) {
        return [];
      }
      log("getAllVehicles done!");
      return vehicleResponseDto.vehicles;
    } catch (e) {
      return [];
    }
  }

  Future<List<VehicleTypeDto>> getAllActiveVehicleTypes() async {
    try {
      const subApiUrl = 'api/vehicles/types';
      final url = Uri.http(KSecret.kApiIp, subApiUrl);

      final response = await apiServices.get(url.toString());

      if (response.statusCode != 200) {
        return [];
      }

      final vehicleTypeList = (response.data['data'] as List)
          .map((json) => VehicleTypeDto.fromJson(json))
          .toList();

      return vehicleTypeList;
    } catch (e) {
      log(e.toString());
      return [];
    }
  }

  Future<double> getVehicleTypeCalculatedPriceInfo(
      int vehicleTypeId, double distanceInMeters, int? discountId) async {
    try {
      const subApiUrl = 'api/rides/estimated-price';
      final url = Uri.http(KSecret.kApiIp, subApiUrl, {
        "vehicleTypeId": vehicleTypeId.toString(),
        "distanceInMeters": distanceInMeters.toString(),
        if (discountId != null) "discountId": discountId.toString()
      });

      final response = await apiServices.get(url.toString());

      if (response.statusCode != 200) {
        return 0;
      }

      return (response.data['data'] ?? 0.0) as double;
    } catch (e) {
      log(e.toString());
      return 0;
    }
  }

  Future<List<VehicleTypeCalculatedPriceInfoDto>>
      getAllActiveVehicleTypeCalculatedPriceInfo(
          double distanceInMeters, int? discountId) async {
    try {
      final vehicleTypeList = await getAllActiveVehicleTypes();
      List<VehicleTypeCalculatedPriceInfoDto>
          vehicleTypeCalculatedPriceInfoList = [];

      for (var vt in vehicleTypeList) {
        double price = await getVehicleTypeCalculatedPriceInfo(
            vt.id, distanceInMeters, discountId);
        vehicleTypeCalculatedPriceInfoList.add(
          VehicleTypeCalculatedPriceInfoDto(
            vehicleTypeId: vt.id,
            vehicleTypeName: vt.name,
            calculatedPrice: price,
          ),
        );
      }

      return vehicleTypeCalculatedPriceInfoList;
    } catch (e) {
      log(e.toString());
      return [];
    }
  }

  Future<bool> isDriverAssigned(String driverId) async {
    try {
      final driverServices = DriverServices();
      final driver = await driverServices.getDriver(driverId);
      if (driver == null) {
        return false;
      }
      return driver.isAssigned;
    } catch (e) {
      log("isDriverAssigned Error:");
      log(e.toString());
      return false;
    }
  }
}
