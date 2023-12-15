import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Dto/user_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/screens/main_tabs_screen.dart';
import 'package:xego_driver/screens/rating_user_screen.dart';
import 'package:xego_driver/services/ride_services.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/image_size_constants.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/settings/ride_status_constants.dart';
import 'package:xego_driver/widgets/info_section_container.dart';
import 'package:xego_driver/widgets/map_widget.dart';

class RideScreen extends StatefulWidget {
  const RideScreen(
      {super.key,
      required this.ride,
      required this.directionResponse,
      required this.totalPrice});

  final Ride ride;
  final DirectionGoogleApiResponseDto directionResponse;
  final double totalPrice;

  @override
  State<RideScreen> createState() => _RideScreenState();
}

class _RideScreenState extends State<RideScreen> {
  final _rideServices = RideServices();
  final _userServices = UserServices();

  final normalRideStatusList = const [
    RideStatusConstants.findingDriver,
    RideStatusConstants.driverAccepted,
    RideStatusConstants.awaitingPickup,
    RideStatusConstants.inProgress,
    RideStatusConstants.completed
  ];

  final scheduledRideStatusList = const [
    RideStatusConstants.findingDriver,
    RideStatusConstants.scheduled,
    RideStatusConstants.awaitingPickup,
    RideStatusConstants.inProgress,
    RideStatusConstants.completed
  ];

  HubConnection? _rideHubConnection;

  String _showingImageUrl = 'assets/images/person_male.png';
  String? _riderAvatarUrl;
  Ride? _ride;
  int _currentStep = 0;
  UserDto? _rider;

  _onFloatingActionButtonPressed() {
    final pickupLatLng =
        LatLng(widget.ride.startLatitude, widget.ride.startLongitude);
    final dropoffLatLng = LatLng(
        widget.ride.destinationLatitude, widget.ride.destinationLongitude);

    if (mounted) {
      Navigator.of(context).push(MaterialPageRoute(
          builder: (context) => MapWidget(
                pickUpLocation: pickupLatLng,
                destinationLocation: dropoffLatLng,
                directionGoogleApiDto: widget.directionResponse,
              )));
    }
  }

  _updateNextStatus(Ride ride) async {
    log('_updateNextStatus:');
    try {
      final updatingStep = _currentStep + 1;
      final driverId = UserServices.userDto!.userId;
      final updatingStatus = ride.isScheduleRide
          ? scheduledRideStatusList[updatingStep + 1]
          : normalRideStatusList[updatingStep + 1];

      if (updatingStatus == RideStatusConstants.completed) {
        _showConfirmCreationDialog(
          title: "Ride completed",
          message: "Are you certain you want to mark this ride as completed?",
          onOkPressed: () async {
            final updateRideStatusResponse = await _rideHubConnection!.invoke(
                "UpdateRideStatus",
                args: [driverId, ride.riderId, ride.id, updatingStatus]);
            final driverDto = await _userServices.getUserById(driverId);
            final riderDto = await _userServices.getUserById(ride.riderId);

            log("updatingStatus: $updatingStatus");
            if (mounted) {
              Navigator.of(context).pop();
              if (driverDto == null || riderDto == null) {
                log('driverDto == null || riderDto == null');
                Navigator.of(context).pushReplacement(MaterialPageRoute(
                    builder: (context) => const MainTabsScreen()));
                return;
              }
              Navigator.of(context).pushReplacement(MaterialPageRoute(
                  builder: (context) => RatingUserScreen(
                        rideId: ride.id,
                        fromUserDto: driverDto,
                        toUserDto: riderDto,
                      )));
              return;
            }
          },
          onCancelPressed: () {
            if (mounted) {
              Navigator.of(context).pop();
            }
          },
        );
      }

      final updateRideStatusResponse = await _rideHubConnection!.invoke(
          "UpdateRideStatus",
          args: [driverId, ride.riderId, ride.id, updatingStatus]);
      log("updatingStatus: $updatingStatus");
      log("updateRideStatusResponse: $updateRideStatusResponse");

      if (mounted) {
        setState(() {
          _currentStep = updatingStep;
          log("updatingStep = $_currentStep");
        });
      }
    } catch (e) {
      log("updateRideStatusResponse error: ${e.toString()}");
    }
  }

  _updatePreviousStatus(Ride ride) async {
    try {
      final updatingStep = _currentStep - 1;
      final driverId = UserServices.userDto!.userId;
      final updatingStatus = ride.isScheduleRide
          ? scheduledRideStatusList[updatingStep + 1]
          : normalRideStatusList[updatingStep + 1];

      final updateRideStatusResponse = await _rideHubConnection!.invoke(
          "UpdateRideStatus",
          args: [driverId, ride.riderId, ride.id, updatingStatus]);
      log("updatingStatus: $updatingStatus");
      log("updateRideStatusResponse: $updateRideStatusResponse");

      if (mounted) {
        setState(() {
          _currentStep = updatingStep;
          log("updatingStep = $_currentStep");
        });
      }
    } catch (e) {
      log("updateRideStatusResponse error: ${e.toString()}");
    }
  }

  _cancelRide() async {
    try {
      final driverId = UserServices.userDto!.userId;
      final ride = widget.ride;
      final updateRideStatusResponse = await _rideHubConnection!
          .invoke("UpdateRideStatus", args: [
        driverId,
        ride.riderId,
        ride.id,
        RideStatusConstants.cancelled
      ]);
      log("_onCancelRide:");
      log("updateRideStatusResponse: $updateRideStatusResponse");
    } catch (e) {
      log("_onCancelRide error: ${e.toString()}");
    }
  }

  _showConfirmCreationDialog({
    required String title,
    required String message,
    required void Function() onOkPressed,
    required void Function() onCancelPressed,
  }) async {
    await showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: Text(message),
          actions: [
            TextButton(
              child: const Text('Create'),
              onPressed: () {
                onOkPressed();
              },
            ),
            TextButton(
              child: const Text('I wanna edit something.'),
              onPressed: () {
                onCancelPressed();
              },
            ),
          ],
        );
      },
    );
  }

  _getRideCurrentStep(String rideStatus, bool isScheduledRide) {
    if (isScheduledRide) {
      return scheduledRideStatusList.indexOf(rideStatus);
    }

    return normalRideStatusList.indexOf(rideStatus);
  }

  _initialize() async {
    final rider = await _userServices.getUserById(widget.ride.riderId);

    if (rider == null) {
      log("rider is null");
      return;
    }

    final riderAvatarUrl = await _userServices.getAvatarUrl(
        rider.userId, ImageSizeConstants.origin);

    setState(() {
      _rider = rider;
      _currentStep =
          _getRideCurrentStep(widget.ride.status, widget.ride.isScheduleRide);
      _riderAvatarUrl = riderAvatarUrl;
    });

    final driverId = UserServices.userDto!.userId;

    await _connectRideHub();
    await _listenLocationChanges(driverId, rider.userId);
  }

  _connectRideHub() async {
    const subHubUrl = 'hubs/ride-hub';
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);
    _rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    _rideHubConnection!.onclose((error) {
      log(error.toString());
      log("Ride Hub Connection Closed");
    });

    try {
      await _rideHubConnection!.start();
      log('Ride Hub Connection started');

      var registerConnectionId = await _rideHubConnection!.invoke(
        'RegisterConnectionId',
        args: [UserServices.userDto!.userId],
      );
      log(registerConnectionId.toString());
    } catch (e) {
      log('Ride Hub Connection failed: $e');
    }
  }

  _listenLocationChanges(String fromUserId, String toUserId) {
    var locationSettings = const LocationSettings(
        accuracy: LocationAccuracy.high, distanceFilter: 10);

    Geolocator.getPositionStream(locationSettings: locationSettings)
        .listen((Position position) {
      log("New position: " +
          position.latitude.toString() +
          ', ' +
          position.longitude.toString());
      final latLng = LatLng(position.latitude, position.longitude);
      _updateLocationToServer(fromUserId, toUserId, latLng);
    });
  }

  _updateLocationToServer(
      String fromUserId, String toUserId, LatLng latLng) async {
    try {
      final updateLocationResponse =
          await _rideHubConnection!.invoke("UpdateLocation", args: [
        fromUserId,
        toUserId,
        latLng.latitude,
        latLng.longitude,
      ]);
      log("updateLocationResponse: $updateLocationResponse");
    } catch (e) {
      log("updateLocationResponse error: ${e.toString()}");
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initialize();
  }

  @override
  Widget build(BuildContext context) {
    final ride = _ride ?? widget.ride;
    ImageProvider showingImageProvider =
        const AssetImage('assets/images/person_male.png');
    final screenHeight = MediaQuery.of(context).size.height;

    if (_showingImageUrl.startsWith('http')) {
      showingImageProvider = NetworkImage(_showingImageUrl);
    } else {
      showingImageProvider = AssetImage(_showingImageUrl);
    }
    return Scaffold(
      floatingActionButton: SizedBox(
        width: 50,
        height: 50,
        child: FloatingActionButton(
          onPressed: _onFloatingActionButtonPressed,
          elevation: 10.0,
          shape: const CircleBorder(),
          backgroundColor: KColors.kPrimaryColor,
          foregroundColor: KColors.kWhite,
          child: const Icon(Icons.map_outlined),
        ),
      ),
      body: Container(
        padding: EdgeInsets.fromLTRB(10, 80, 10, 0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            if (_rider != null)
              Expanded(
                child: Container(
                  padding: const EdgeInsets.fromLTRB(0, 0, 0, 0),
                  child: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      Text(
                        "Rider Informations",
                        textAlign: TextAlign.start,
                        style: TextStyle(
                          fontSize:
                              Theme.of(context).textTheme.titleLarge!.fontSize,
                          color: KColors.kPrimaryColor,
                        ),
                      ),
                      const SizedBox(
                        height: 10,
                      ),
                      Container(
                          width: double.infinity,
                          decoration: BoxDecoration(
                            color: KColors.kOnBackgroundColor,
                            borderRadius: BorderRadius.circular(10.0),
                            border: Border.all(
                              color: KColors.kPrimaryColor.withOpacity(0.3),
                              width: 1.0,
                            ),
                          ),
                          padding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                          child: Row(
                            children: [
                              Container(
                                height: 100,
                                width: 100,
                                decoration: BoxDecoration(
                                    shape: BoxShape.circle,
                                    color: KColors.kWhite,
                                    border: Border.all(
                                        color: KColors.kColor4, width: 2.0)),
                                child: ClipOval(
                                  child: Image(
                                    image: showingImageProvider,
                                    fit: _showingImageUrl.startsWith('http')
                                        ? BoxFit.cover
                                        : null, // or BoxFit.contain
                                  ),
                                ),
                              ),
                              const Gap(20),
                              Expanded(
                                child: Column(
                                  crossAxisAlignment: CrossAxisAlignment.start,
                                  children: [
                                    RichText(
                                      text: TextSpan(
                                        children: [
                                          TextSpan(
                                            text: 'Name: ',
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kColor6,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                          TextSpan(
                                            text:
                                                "${_rider!.firstName}, ${_rider!.lastName}",
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kTertiaryColor,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                        ],
                                      ),
                                    ),
                                    RichText(
                                      text: TextSpan(
                                        children: [
                                          TextSpan(
                                            text: 'Tel: ',
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kColor6,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                          TextSpan(
                                            text: _rider!.phoneNumber,
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kTertiaryColor,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                        ],
                                      ),
                                    ),
                                    RichText(
                                      softWrap: true,
                                      text: TextSpan(
                                        children: [
                                          TextSpan(
                                            text: 'Pickup: ',
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kColor6,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                          TextSpan(
                                            text: ride.startAddress,
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kTertiaryColor,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                        ],
                                      ),
                                    ),
                                    RichText(
                                      text: TextSpan(
                                        children: [
                                          TextSpan(
                                            text: 'Dropoff: ',
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kColor6,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                          TextSpan(
                                            text: ride.destinationAddress,
                                            style: Theme.of(context)
                                                .textTheme
                                                .bodySmall!
                                                .copyWith(
                                                  color: KColors.kTertiaryColor,
                                                  fontWeight: FontWeight.bold,
                                                ),
                                          ),
                                        ],
                                      ),
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ))
                    ],
                  ),
                ),
              ),
            Expanded(
              flex: 3,
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    "Ride Status",
                    textAlign: TextAlign.start,
                    style: TextStyle(
                      fontSize:
                          Theme.of(context).textTheme.titleLarge!.fontSize,
                      color: KColors.kPrimaryColor,
                    ),
                  ),
                  SizedBox(
                    height: screenHeight / 2,
                    child: Stepper(
                      currentStep: _currentStep,
                      onStepContinue: _currentStep <
                              RideStatusConstants.maxStepCount - 2
                          ? () async {
                              final isSuccess = await _updateNextStatus(ride);
                              if (isSuccess == null) {
                                return;
                              }
                            }
                          : null,
                      onStepCancel: _currentStep > 0
                          ? () async {
                              final isSuccess =
                                  await _updatePreviousStatus(ride);
                              if (isSuccess == null) {
                                return;
                              }
                            }
                          : null,
                      steps: ride.isScheduleRide
                          ? scheduledRideStatusList
                              .where((status) =>
                                  status != RideStatusConstants.findingDriver)
                              .map(
                                (status) => Step(
                                    title: Text(
                                      _rideServices
                                          .getShowingRideStatus(status),
                                      style: TextStyle(
                                        color: _currentStep ==
                                                scheduledRideStatusList
                                                        .indexOf(status) -
                                                    1
                                            ? KColors.kPrimaryColor
                                            : null,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                    content: Text(_rideServices
                                        .getShowingRideStatus(status))),
                              )
                              .toList()
                          : normalRideStatusList
                              .where((status) =>
                                  status != RideStatusConstants.findingDriver)
                              .map(
                                (status) => Step(
                                    title: Text(
                                      _rideServices
                                          .getShowingRideStatus(status),
                                      style: TextStyle(
                                        color: _currentStep ==
                                                normalRideStatusList
                                                        .indexOf(status) -
                                                    1
                                            ? KColors.kPrimaryColor
                                            : null,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                    content: Text(_rideServices
                                        .getShowingRideStatus(status))),
                              )
                              .toList(),
                    ),
                  ),
                  const Gap(20),
                  Align(
                      child: ElevatedButton(
                          onPressed: () {
                            _cancelRide();
                          },
                          child: Text('Cancel Ride'))),
                ],
              ),
            )
          ],
        ),
      ),
    );
  }
}
