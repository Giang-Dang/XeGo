import 'dart:developer';

import 'package:xego_driver/models/Dto/vehicle_response_dto.dart';
import 'package:xego_driver/models/Entities/vehicle.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class VehicleServices {
  final apiServices = ApiServices();

  Future<List<Vehicle>> getAllVehicles({
    int? id,
    String? plateNumber,
    String? type,
    String? driverId,
    bool? isActive,
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
        'driverId': driverId,
        'isActive': isActive,
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
      return vehicleResponseDto.vehicles;
    } catch (e) {
      log(e.toString());
      return [];
    }
  }

  Future<bool> isDriverAssigned(String driverId) async {
    try {
      const subApiUrl = 'api/drivers';
      final url = Uri.http(KSecret.kApiIp, subApiUrl, {
        "userId": driverId,
      });
      final response = await apiServices.get(url.toString());

      if (response.data['isSuccess'] as bool) {
        return false;
      }

      return response.data['data']['isAssigned'] as bool;
    } catch (e) {
      log(e.toString());
      return false;
    }
  }
}
