import 'package:flutter/material.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Dto/user_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/services/ride_services.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/ride_status_constants.dart';

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

  Ride? _ride;
  int _currentStep = 0;
  UserDto? _rider;

  _onFloatingActionButtonPressed() {}

  _updateNextStatus(Ride ride) async {}

  _updatePreviousStatus(Ride ride) async {}

  _getRideCurrentStep(String rideStatus, bool isScheduledRide) {
    if (isScheduledRide) {
      return scheduledRideStatusList.indexOf(rideStatus);
    }

    return normalRideStatusList.indexOf(rideStatus);
  }

  _initialize() async {
    final rider = await _userServices.getUserById(widget.ride.riderId);

    setState(() {
      _rider = rider;
      _currentStep =
          _getRideCurrentStep(widget.ride.status, widget.ride.isScheduleRide);
    });
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
          child: Column(
        children: [
          Expanded(
            child: Stepper(
              currentStep: _currentStep,
              onStepContinue:
                  _currentStep < RideStatusConstants.maxStepCount - 1
                      ? () async {
                          final isSuccess = await _updateNextStatus(ride);
                          if (isSuccess == null) {
                            return;
                          }
                        }
                      : null,
              onStepCancel: _currentStep > 0
                  ? () async {
                      final isSuccess = await _updatePreviousStatus(ride);
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
                              _rideServices.getShowingRideStatus(status),
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
                            content: Text(
                                _rideServices.getShowingRideStatus(status))),
                      )
                      .toList()
                  : normalRideStatusList
                      .where((status) =>
                          status != RideStatusConstants.findingDriver)
                      .map(
                        (status) => Step(
                            title: Text(
                              _rideServices.getShowingRideStatus(status),
                              style: TextStyle(
                                color: _currentStep ==
                                        normalRideStatusList.indexOf(status) - 1
                                    ? KColors.kPrimaryColor
                                    : null,
                                fontWeight: FontWeight.bold,
                              ),
                            ),
                            content: Text(
                                _rideServices.getShowingRideStatus(status))),
                      )
                      .toList(),
            ),
          )
        ],
      )),
    );
  }
}
