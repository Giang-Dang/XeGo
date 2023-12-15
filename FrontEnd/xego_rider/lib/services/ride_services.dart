import 'dart:developer';

import 'package:xego_rider/models/Dto/create_ride_request_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';
import 'package:xego_rider/settings/ride_status_constants.dart';

class RideServices {
  final apiServices = ApiServices();

  Future<List<Ride>> getAllRides({
    String? riderId,
    bool? isScheduleRide,
    String? status,
    bool? rideFinished,
  }) async {
    try {
      final Map<String, dynamic> queryParams = {
        if (riderId != null) 'riderId': riderId,
        if (isScheduleRide != null) 'isScheduleRide': isScheduleRide.toString(),
        if (status != null) 'status': status,
        if (rideFinished != null) 'rideFinished': rideFinished.toString(),
      };

      final Uri uri = Uri.http(KSecret.kApiIp, 'api/rides', queryParams);

      final response = await apiServices.get(uri.toString());

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

  Future<Ride?> createRide(CreateRideRequestDto requestDto) async {
    const subApiUrl = 'api/rides';
    final url = Uri.http(KSecret.kApiIp, subApiUrl);

    final response =
        await apiServices.post(url.toString(), data: requestDto.toJson());

    if (response.statusCode != 200) {
      log(response.data['message'].toString());
      return null;
    }

    final res = Ride.fromJson(response.data['data']);
    return res;
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
