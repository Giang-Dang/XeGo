import 'dart:developer';

import 'package:xego_rider/models/Entities/price.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class PriceServices {
  final _apiServices = ApiServices();

  Future<Price?> getPriceByRideId(int rideId) async {
    try {
      const subApiUrl = 'api/price';
      final url =
          Uri.http(KSecret.kApiIp, subApiUrl, {'rideId': rideId.toString()});

      final response = await _apiServices.get(url.toString());
      if (!response.data['isSuccess']) {
        log('getPriceByRideId: failed!');
        return null;
      }

      final prices = (response.data['data'] as List)
          .map((item) => Price.fromJson(item))
          .toList();

      if (prices.isEmpty) {
        log('getPriceByRideId: not found!');
        return null;
      }

      return prices[0];
    } catch (e) {
      log('getPriceByRideId Error:');
      log(e.toString());
      return null;
    }
  }
}
