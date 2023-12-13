import 'dart:convert';
import 'dart:developer';

import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class RideHubService {
  static HubConnection? rideHubConnection;

  RideHubService(Function(List<dynamic>?) handleDriverLocationUpdate) {
    startHub(handleDriverLocationUpdate);
  }

  startHub(Function(List<dynamic>?) handleDriverLocationUpdate) async {
    const subHubUrl = 'hubs/ride-hub';
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);

    rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    rideHubConnection!.onclose(_handleClose);
    rideHubConnection!.on("ReceiveLocation", handleDriverLocationUpdate);

    try {
      await startConnection();
    } catch (e) {
      log('Ride Hub Connection failed: $e');
    }
  }

  _handleClose(error) {
    log("Ride Hub Connection Closed");
  }

  startConnection() async {
    await rideHubConnection!.start();
    log('Ride Hub Connection started');
  }

  registerAndFindDriver(DirectionGoogleApiResponseDto directionResponse,
      Ride ride, double totalPrice) async {
    var registerConnectionId = await rideHubConnection!.invoke(
      'RegisterConnectionId',
      args: [UserServices.userDto!.userId],
    );

    final directionResponseJson =
        jsonEncode(directionResponse.toJson()).toString();

    final driverId = await rideHubConnection!.invoke(
      'FindDriver',
      args: [
        UserServices.userDto!.userId,
        ride.toJson(),
        totalPrice,
        directionResponseJson,
      ],
    ) as String;

    log("driverId: $driverId");
  }
}
