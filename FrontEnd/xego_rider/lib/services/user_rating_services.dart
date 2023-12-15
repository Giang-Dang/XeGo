import 'dart:developer';

import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

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
        "rideId": rideId,
        "fromUserId": fromUserId,
        "fromUserRole": fromUserRole,
        "toUserId": toUserId,
        "toUserRole": toUserRole,
        "rating": rating,
        "createdBy": createdBy,
      });
      log("UserRatingServices > createRating: Done");
      log(response.data['isSuccess'].toString());
      return response.data['isSuccess'];
    } catch (e) {
      log("UserRatingServices > createRating error:");
      log(e.toString());
      return false;
    }
  }
}
