import 'dart:developer';

import 'package:xego_rider/models/Entities/driver.dart';
import 'package:xego_rider/models/Entities/vehicle.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class DriverServices {
  final _apiServices = ApiServices();

  Future<Driver?> getDriver(String userId) async {
    try {
      const subApiUrl = 'api/drivers';
      final url = "${Uri.http(KSecret.kApiIp, subApiUrl)}/$userId";

      final response = await _apiServices.get(url);

      if (response.data['isSuccess']) {
        final driver = Driver.fromJson(response.data['data']);
        log("getDriver done!");
        return driver;
      }
    } catch (e) {
      log(e.toString());
    }
    return null;
  }

  Future<Vehicle?> getAssignedVehicle(String driverId) async {
    try {
      const subApiUrl = 'api/drivers';
      final url =
          "${Uri.http(KSecret.kApiIp, subApiUrl)}/$driverId/assigned-vehicle";

      final response = await _apiServices.get(url);

      if (response.data['isSuccess']) {
        final vehicle = Vehicle.fromJson(response.data['data']);
        log("getAssignedVehicle done!");
        return vehicle;
      }
    } catch (e) {
      log("getAssignedVehicle Error:");
      log(e.toString());
    }
    return null;
  }
}
