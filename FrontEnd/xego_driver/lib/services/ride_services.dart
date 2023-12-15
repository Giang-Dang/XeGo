import 'dart:developer';

import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/settings/ride_status_constants.dart';

class RideServices {
  final _apiServices = ApiServices();

  Future<List<Ride>> getAllRides({
    String? driverId,
    bool? isScheduleRide,
    String? status,
    bool? rideFinished,
  }) async {
    try {
      final Map<String, dynamic> queryParams = {
        if (driverId != null) 'driverId': driverId,
        if (isScheduleRide != null) 'isScheduleRide': isScheduleRide.toString(),
        if (status != null) 'status': status,
        if (rideFinished != null) 'rideFinished': rideFinished.toString(),
      };

      final Uri uri = Uri.http(KSecret.kApiIp, 'api/rides', queryParams);

      final response = await _apiServices.get(uri.toString());

      if (!response.data['isSuccess']) {
        log('getAllRides: failed!');
        return [];
      }

      return (response.data['data'] as List)
          .map((item) => Ride.fromJson(item))
          .toList();
    } catch (e) {
      log('getAllRides error: $e');
      return [];
    }
  }

  String getShowingRideStatus(String rideStatus) {
    switch (rideStatus) {
      case RideStatusConstants.scheduled:
        return "Scheduled. Driver has been assigned.";
      case RideStatusConstants.findingDriver:
        return "Finding Driver...";
      case RideStatusConstants.awaitingPickup:
        return "Driver is waiting for you...";
      case RideStatusConstants.driverAccepted:
        return "Driver is heading to the pickup location..";
      case RideStatusConstants.inProgress:
        return "In Progress";
      case RideStatusConstants.cancelled:
        return "Cancelled";
      case RideStatusConstants.completed:
        return "Completed";
      default:
        return rideStatus;
    }
  }
}
