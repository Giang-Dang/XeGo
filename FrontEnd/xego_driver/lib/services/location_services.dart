import 'dart:convert';
import 'dart:developer';
import 'dart:ui';

import 'package:flutter_polyline_points/flutter_polyline_points.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:http/http.dart' as http;
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'dart:math' as math;

import 'package:xego_driver/settings/kSecrets.dart';

class LocationServices {
  final _apiServices = ApiServices();
  static LatLng? currentLocation;

  Future<bool> updateLocationToDb(
      String userId, bool isDriver, LatLng latLng, String createdBy) async {
    try {
      final subApiUrl =
          isDriver ? "api/locations/drivers" : "api/locations/riders";
      final url = Uri.http(KSecret.kApiIp, subApiUrl);

      final response = await _apiServices.post(url.toString(), data: {
        "userId": userId,
        "latitude": latLng.latitude,
        "longitude": latLng.longitude,
        "createdBy": createdBy,
        "modifiedBy": createdBy,
      });

      return response.data['isSuccess'];
    } catch (e) {
      log(e.toString());
      return false;
    }
  }

  Future<void> updateCurrentLocation() async {
    determinePosition().then((value) {
      log('updateCurrentLocation completed');
      currentLocation = LatLng(value.latitude, value.longitude);
    }).onError((error, stackTrace) {
      log(error.toString());
    });
  }

  Future<bool> deleteLocationInDb(String userId, bool isDriver) async {
    try {
      final subApiUrl = isDriver
          ? "api/locations/drivers/$userId"
          : "api/locations/riders/$userId";
      final url = Uri.http(KSecret.kApiIp, subApiUrl);

      final response = await _apiServices.delete(url.toString());
      return response.data['isSuccess'];
    } catch (e) {
      log(e.toString());
      return false;
    }
  }

  Future<Position> determinePosition() async {
    bool serviceEnabled;
    LocationPermission permission;

    serviceEnabled = await Geolocator.isLocationServiceEnabled();
    if (!serviceEnabled) {
      return Future.error('Location services are disabled.');
    }

    permission = await Geolocator.checkPermission();
    if (permission == LocationPermission.denied) {
      permission = await Geolocator.requestPermission();
      if (permission == LocationPermission.denied) {
        return Future.error('Location permissions are denied');
      }
    }

    if (permission == LocationPermission.deniedForever) {
      return Future.error(
          'Location permissions are permanently denied, we cannot request permissions.');
    }
    return await Geolocator.getCurrentPosition(
        desiredAccuracy: LocationAccuracy.best);
  }

  String getlocationImageUrl(double lat, double lng, int zoom) {
    return 'https://maps.googleapis.com/maps/api/staticmap?center=$lat,$lng&zoom=$zoom&size=600x300&maptype=roadmap&markers=color:red%7Clabel:S%7C$lat,$lng&key=${KSecret.kMapsAPIKey}';
  }

  Future<String> getAddress(double lat, double lng) async {
    final url = Uri.parse(
        'https://maps.googleapis.com/maps/api/geocode/json?latlng=$lat,$lng&key=${KSecret.kMapsAPIKey}');

    final response = await http.get(url);
    final resData = json.decode(response.body);
    return resData['results'][0]['formatted_address'];
  }

  Future<LatLng?> getCoordinates(String address) async {
    final uri = Uri.https(
      'maps.googleapis.com',
      '/maps/api/geocode/json',
      {
        'address': address,
        'key': KSecret.kMapsAPIKey,
      },
    );
    final response = await http.get(uri);
    final data = jsonDecode(response.body);
    if (data['results'].length == 0) {
      return null;
    }
    final lat = data['results'][0]['geometry']['location']['lat'];
    final lng = data['results'][0]['geometry']['location']['lng'];
    return LatLng(lat, lng);
  }

  double calculateDistance(double lat1, double lng1, double lat2, double lng2) {
    const earthRadius = 6371; // km
    final dLat = _toRadians(lat2 - lat1);
    final dLon = _toRadians(lng2 - lng1);
    final lat1Rad = _toRadians(lat1);
    final lat2Rad = _toRadians(lat2);

    final a = math.sin(dLat / 2) * math.sin(dLat / 2) +
        math.sin(dLon / 2) *
            math.sin(dLon / 2) *
            math.cos(lat1Rad) *
            math.cos(lat2Rad);
    final c = 2 * math.atan2(math.sqrt(a), math.sqrt(1 - a));

    return earthRadius * c;
  }

  double _toRadians(double degree) {
    return degree * (math.pi / 180);
  }

  Future<DirectionGoogleApiResponseDto?> getPlaceDirectionDetails(
      LatLng startPosition, LatLng destinationPosition) async {
    String directionUrl =
        'https://maps.googleapis.com/maps/api/directions/json?destination=${destinationPosition.latitude},${destinationPosition.longitude}&origin=${startPosition.latitude},${startPosition.longitude}&key=${KSecret.kMapsAPIKey}';

    var res = await _apiServices.get(directionUrl);

    log(res.toString());

    if (res.data['status'].toString().toUpperCase() != 'OK') {
      return null;
    }

    DirectionGoogleApiResponseDto directionResponse =
        DirectionGoogleApiResponseDto();

    directionResponse.encodedPoints =
        res.data['routes'][0]['overview_polyline']['points'];
    directionResponse.distanceText =
        res.data['routes'][0]['legs'][0]['distance']['text'];
    directionResponse.distanceValue =
        res.data['routes'][0]['legs'][0]['distance']['value'];
    directionResponse.durationText =
        res.data['routes'][0]['legs'][0]['duration']['text'];
    directionResponse.durationValue =
        res.data['routes'][0]['legs'][0]['duration']['value'];

    return directionResponse;
  }

  Future<Set<Polyline>> getDirectionPolylines(
      DirectionGoogleApiResponseDto directionResponse,
      {Color directionColor = KColors.kBlue}) async {
    Set<Polyline> polylines = {};

    PolylinePoints polylinePoints = PolylinePoints();
    List<PointLatLng> decodedPolylinePoints =
        polylinePoints.decodePolyline(directionResponse.encodedPoints!);
    polylines.add(Polyline(
      polylineId: PolylineId('direction'),
      points: decodedPolylinePoints
          .map((point) => LatLng(point.latitude, point.longitude))
          .toList(),
      color: directionColor,
      width: 3,
    ));

    return polylines;
  }
}
