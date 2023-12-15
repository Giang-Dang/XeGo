import 'dart:developer';

import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class UserRatingServices {
  final _apiServices = ApiServices();

  Future<bool> createRating(
    int rideId,
    String fromUserId,
    String fromUserRole,
    String toUserId,
    String toUserRole,
    double rating,
    String createdBy,
  ) async {
    try {
      const subApiUrl = 'api/rating';
      final url = Uri.http(KSecret.kApiIp, subApiUrl);

      final response = await _apiServices.post(url.toString(), data: {
        "RideId": rideId,
        "FromUserId": fromUserId,
        "FromUserRole": fromUserRole,
        "ToUserId": toUserId,
        "ToUserRole": toUserRole,
        "Rating": rating,
      });
      log("UserRatingServices > createRating: Done");
      return response.data['isSuccess'];
    } catch (e) {
      log("UserRatingServices > createRating error:");
      log(e.toString());
      return false;
    }
  }
}
