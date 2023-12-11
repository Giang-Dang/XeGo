import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/settings/kSecrets.dart';
import 'package:xego_rider/widgets/map_widget.dart';

class RideScreen extends StatefulWidget {
  const RideScreen({
    super.key,
    required this.rideInfo,
    required this.directionResponse,
    required this.vehicleTypeName,
    required this.totalPrice,
  });

  final Ride rideInfo;
  final DirectionGoogleApiResponseDto directionResponse;
  final String vehicleTypeName;
  final double totalPrice;

  @override
  State<RideScreen> createState() => _RideScreenState();
}

class _RideScreenState extends State<RideScreen> {
  Timer? _initialTimer;
  HubConnection? _rideHubConnection;
  final _locationServices = LocationServices();
  List<LatLng> _driverLocationList = [];

//   // Assuming you have a HubConnection instance `hubConnection`
// hubConnection.on("ReceiveLocation", _handleLocationUpdate);

  _initialize() async {
    await _locationServices.updateCurrentLocation();
    await _locationServices.pushRiderLocation(
        LocationServices.currentLocation!, UserServices.userDto!.userId);

    _startHub();
  }

  _startHub() async {
    const subHubUrl = 'hubs/ride-hub';
    // final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);

    _rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    _rideHubConnection!.onclose((error) => log("Ride Hub Connection Closed"));
    _rideHubConnection!.on("ReceiveLocation", _handleDriverLocationUpdate);

    try {
      await _rideHubConnection!.start();
      log('Ride Hub Connection started');

      var registerConnectionId = await _rideHubConnection!.invoke(
        'RegisterConnectionId',
        args: [UserServices.userDto!.userId],
      );

      final directionResponseJson =
          jsonEncode(widget.directionResponse.toJson()).toString();

      log(registerConnectionId.toString());
      log(jsonEncode(widget.directionResponse.toJson()).toString());

      log("UserServices.userDto!.userId: ${UserServices.userDto!.userId}");
      log("widget.rideInfo: ${jsonEncode(widget.rideInfo.toJson())}");
      log("widget.totalPrice: ${widget.totalPrice}");
      log("directionResponseJson: $directionResponseJson");

      final driverId = await _rideHubConnection!.invoke(
        'FindDriver',
        args: [
          UserServices.userDto!.userId,
          widget.rideInfo.toJson(),
          widget.totalPrice,
          directionResponseJson,
        ],
      );
      log("driverId: $driverId");
    } catch (e) {
      log('Ride Hub Connection failed: $e');
    }
  }

  void _handleDriverLocationUpdate(List<dynamic>? parameters) {
    if (parameters == null) {
      log("_handleDriverLocationUpdate: parameters are null!");
      return;
    }

    LatLng newDriverLocation =
        LatLng(parameters[0] as double, parameters[1] as double);
    setState(() {
      _driverLocationList = [newDriverLocation];
    });
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initialTimer = Timer.periodic(
      const Duration(milliseconds: 100),
      (timer) {
        _initialize();
        _initialTimer?.cancel();
      },
    );
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _initialTimer?.cancel();
    _rideHubConnection?.stop();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    double screenWidth = MediaQuery.of(context).size.width;
    double screenHeight = MediaQuery.of(context).size.height;
    const bottomContainerHeight = 310.0;
    const circularContainerHeight = 120.0;
    const circularContainerWidth = 120.0;
    const roundedBorderContainerHeight = 80.0;
    const roundedBorderContainerWidth = 250.0;

    var pickUpLocation = LatLng(
      widget.rideInfo.startLatitude,
      widget.rideInfo.startLongitude,
    );
    var destinationLocation = LatLng(
      widget.rideInfo.destinationLatitude,
      widget.rideInfo.destinationLongitude,
    );

    return Scaffold(
      body: Stack(
        children: [
          SizedBox(
            height: screenHeight,
            width: screenWidth,
          ),
          SizedBox(
            height: screenHeight - bottomContainerHeight,
            child: MapWidget(
              pickUpLocation: pickUpLocation,
              destinationLocation: destinationLocation,
              driverLocationsList: _driverLocationList,
              mapMyLocationEnabled: false,
              mapZoomControllerEnabled: false,
              markerOutterPadding: 100,
            ),
          ),
          Positioned(
            bottom: 0,
            left: 0,
            right: 0,
            child: Container(
              height: bottomContainerHeight,
              width: double.infinity,
              decoration: BoxDecoration(
                color: KColors.kSecondaryColor,
                borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(8.0),
                    topRight: Radius.circular(8.0)),
                border: Border.all(color: KColors.kColor4, width: 1.0),
              ),
              padding: const EdgeInsets.fromLTRB(20, 70, 20, 30),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Expanded(
                        flex: 2,
                        child: Text(
                          'Pick Up Location: ',
                          style: TextStyle(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                            fontSize: 14,
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 3,
                        child: Text(
                          widget.rideInfo.startAddress,
                          style: const TextStyle(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.normal,
                            fontSize: 14,
                          ),
                        ),
                      ),
                    ],
                  ),
                  const Gap(5),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Expanded(
                        flex: 2,
                        child: Text(
                          'Drop Off Location: ',
                          style: TextStyle(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                            fontSize: 14,
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 3,
                        child: Text(
                          widget.rideInfo.destinationAddress,
                          style: const TextStyle(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.normal,
                            fontSize: 14,
                          ),
                        ),
                      ),
                    ],
                  ),
                  Row(
                    children: [
                      const Expanded(flex: 8, child: Text('')),
                      Expanded(
                        flex: 7,
                        child: RichText(
                          text: TextSpan(
                            style: const TextStyle(
                              color: KColors.kTextColor,
                              fontSize: 12,
                            ),
                            children: <TextSpan>[
                              const TextSpan(
                                  text: 'Distance: ',
                                  style: TextStyle(
                                      fontWeight: FontWeight.bold,
                                      color: KColors.kColor6)),
                              TextSpan(
                                text: widget.directionResponse.distanceText,
                                style: const TextStyle(
                                  color: KColors.kTertiaryColor,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 5,
                        child: RichText(
                          text: TextSpan(
                            style: const TextStyle(
                              color: KColors.kTextColor,
                              fontSize: 12,
                            ),
                            children: <TextSpan>[
                              const TextSpan(
                                  text: 'ETA: ',
                                  style: TextStyle(
                                      fontWeight: FontWeight.bold,
                                      color: KColors.kColor6)),
                              TextSpan(
                                text: widget.directionResponse.durationText,
                                style: const TextStyle(
                                  color: KColors.kTertiaryColor,
                                ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
                  ),
                  const Gap(7),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Expanded(
                        flex: 2,
                        child: Text(
                          'Vehicle Type: ',
                          style: TextStyle(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                            fontSize: 14,
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 3,
                        child: Text(
                          widget.vehicleTypeName,
                          style: const TextStyle(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.normal,
                            fontSize: 14,
                          ),
                        ),
                      ),
                    ],
                  ),
                  const Divider(color: KColors.kTertiaryColor),
                  Row(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Expanded(
                        flex: 2,
                        child: Text(
                          'Total Price: ',
                          style: TextStyle(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                            fontSize: 14,
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 3,
                        child: Text(
                          '\$${widget.totalPrice}',
                          style: const TextStyle(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.bold,
                            fontSize: 14,
                          ),
                          textAlign: TextAlign.end,
                        ),
                      ),
                    ],
                  ),
                ],
              ),
            ),
          ),
          Positioned(
            bottom: bottomContainerHeight - 40,
            right: 20 + circularContainerWidth / 2,
            child: Container(
              height: roundedBorderContainerHeight,
              width: roundedBorderContainerWidth,
              decoration: BoxDecoration(
                borderRadius: BorderRadius.circular(15),
                color: KColors.kWhite,
              ),
            ),
          ),
          Positioned(
            bottom: bottomContainerHeight - circularContainerHeight / 2,
            right: 20,
            child: Container(
              height: circularContainerHeight,
              width: circularContainerWidth,
              decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: KColors.kWhite,
                  border: Border.all(color: KColors.kColor4, width: 2.0)),
            ),
          ),
        ],
      ),
    );
  }
}
