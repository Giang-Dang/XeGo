import 'dart:developer';

import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class NotificationServices {
  static String? fcmToken;
  final apiServices = ApiServices();

  Future<bool> saveFcmTokenToDb(String userId, String fcmToken) async {
    try {
      const subApiUrl = 'api/notifications/update-fcm-token';
      final url = Uri.http(KSecret.kApiIp, subApiUrl);

      final response = await apiServices.post(url.toString(), data: {
        "userId": userId,
        "fcmToken": fcmToken,
      });

      return response.statusCode as int == 200;
    } catch (e) {
      log(e.toString());
      return false;
    }
  }
}
