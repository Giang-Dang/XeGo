import 'dart:developer';

import 'package:xego_rider/models/Dto/create_ride_request_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class RideServices {
  final apiServices = ApiServices();

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
}
