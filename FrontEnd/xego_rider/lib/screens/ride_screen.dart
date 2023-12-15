import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Entities/driver.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/models/Entities/vehicle.dart';
import 'package:xego_rider/services/driver_services.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/services/vehicle_services.dart';
import 'package:xego_rider/settings/image_size_constants.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/settings/kSecrets.dart';
import 'package:xego_rider/settings/ride_status_constants.dart';
import 'package:xego_rider/widgets/driver_info_and_ride_status.dart';
import 'package:xego_rider/widgets/map_widget.dart';
import 'package:xego_rider/widgets/ride_info.dart';

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
  final _userServices = UserServices();
  final _driverServices = DriverServices();
  final _vehicleServices = VehicleServices();

  final _notFoundImageUrl = 'assets/images/not_found.png';
  final _findingImageUrl = 'assets/images/searching_icon.gif';
  final _defaultAvatarUrl = 'assets/images/person_male.png';

  Ride? _ride;
  List<LatLng> _driverLocationList = [];
  Driver? _acceptedDriver;
  Vehicle? _acceptedVehicle;
  String? _driverAvatarUrl;
  bool _isFindingDriver = true;
  String _showingImageUrl = 'assets/images/searching_icon.gif';
  bool _isDriverNotFound = false;

  // bool _isDriverNotFound = false;

//   // Assuming you have a HubConnection instance `hubConnection`
// hubConnection.on("ReceiveLocation", _handleLocationUpdate);

  _initialize() async {
    _showingImageUrl = _findingImageUrl;

    await _locationServices.updateCurrentLocation();
    await _locationServices.pushRiderLocation(
        LocationServices.currentLocation!, UserServices.userDto!.userId);

    _startHub();
  }

  _setIsFindingDriverState(bool state) {
    if (mounted) {
      setState(() {
        _showingImageUrl =
            state ? _findingImageUrl : (_driverAvatarUrl ?? _defaultAvatarUrl);
        _isFindingDriver = state;
      });
    }
  }

  _setDriverNotFoundState(bool state) {
    if (state) {
      if (mounted) {
        setState(() {
          _showingImageUrl = _notFoundImageUrl;
          _isDriverNotFound = true;
        });
      }
      return;
    } else {
      if (mounted) {
        setState(() {
          _isDriverNotFound = false;
        });
      }
    }
  }

  _handleUpdateRideStatus(List<dynamic>? parameters) {
    log("_handleUpdateRideStatus begin");
    if (parameters == null) {
      log("_handleUpdateRideStatus Error: parameters is null");
      return;
    }

    String newStatus = parameters[0];
    if (mounted) {
      setState(() {
        _ride = Ride(
          id: _ride?.id ?? widget.rideInfo.id,
          riderId: _ride?.riderId ?? widget.rideInfo.riderId,
          driverId: _ride?.driverId ?? widget.rideInfo.driverId,
          vehicleTypeId: _ride?.vehicleTypeId ?? widget.rideInfo.vehicleTypeId,
          status: newStatus,
          startLatitude: _ride?.startLatitude ?? widget.rideInfo.startLatitude,
          startLongitude:
              _ride?.startLongitude ?? widget.rideInfo.startLongitude,
          startAddress: _ride?.startAddress ?? widget.rideInfo.startAddress,
          destinationLatitude:
              _ride?.destinationLatitude ?? widget.rideInfo.destinationLatitude,
          destinationLongitude: _ride?.destinationLongitude ??
              widget.rideInfo.destinationLongitude,
          destinationAddress:
              _ride?.destinationAddress ?? widget.rideInfo.destinationAddress,
          pickupTime: _ride?.pickupTime ?? widget.rideInfo.pickupTime,
          isScheduleRide:
              _ride?.isScheduleRide ?? widget.rideInfo.isScheduleRide,
        );
      });
    }
  }

  _onTryAgainTap() {
    _startHub();

    _setDriverNotFoundState(false);
  }

  _startHub() async {
    const subHubUrl = 'hubs/ride-hub';
    // final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);

    _rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    _rideHubConnection!.onclose((error) {
      log(error.toString());
      log("Ride Hub Connection Closed");
    });
    _rideHubConnection!.on("receiveLocation", _handleReceiveLocation);
    _rideHubConnection!.on("updateRideStatus", _handleUpdateRideStatus);

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

      _setIsFindingDriverState(true);

      final driverId = await _rideHubConnection!.invoke(
        'FindDriver',
        args: [
          UserServices.userDto!.userId,
          widget.rideInfo.toJson(),
          widget.totalPrice,
          directionResponseJson,
        ],
      ) as String;
      log("driverId: $driverId");

      if (driverId != "") {
        final driverAvatarUrl = await _userServices.getAvatarUrl(
            driverId, ImageSizeConstants.origin);
        final acceptedDriver = await _driverServices.getDriver(driverId);
        final vehicle = await _driverServices.getAssignedVehicle(driverId);
        if (mounted) {
          setState(() {
            _driverAvatarUrl = driverAvatarUrl;
            _acceptedDriver = acceptedDriver;
            _acceptedVehicle = vehicle;
            _ride = Ride(
                id: widget.rideInfo.id,
                riderId: widget.rideInfo.riderId,
                driverId: driverId,
                vehicleTypeId: widget.rideInfo.vehicleTypeId,
                status: RideStatusConstants.driverAccepted,
                startLatitude: widget.rideInfo.startLatitude,
                startLongitude: widget.rideInfo.startLongitude,
                startAddress: widget.rideInfo.startAddress,
                destinationLatitude: widget.rideInfo.destinationLatitude,
                destinationLongitude: widget.rideInfo.destinationLongitude,
                destinationAddress: widget.rideInfo.destinationAddress,
                pickupTime: widget.rideInfo.pickupTime,
                isScheduleRide: widget.rideInfo.isScheduleRide);
          });
        }

        _setIsFindingDriverState(false);
        return;
      }
      _setIsFindingDriverState(false);
      _setDriverNotFoundState(true);
    } catch (e) {
      log('Ride Hub Connection failed: $e');
    }
  }

  void _handleReceiveLocation(List<dynamic>? parameters) {
    if (parameters == null) {
      log("_handleDriverLocationUpdate: parameters are null!");
      return;
    }

    LatLng newDriverLocation =
        LatLng(parameters[1] as double, parameters[2] as double);
    if (mounted) {
      setState(() {
        _driverLocationList = [newDriverLocation];
      });
    }
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
    const roundedBorderContainerHeight = 100.0;
    const roundedBorderContainerWidth = 250.0;
    ImageProvider showingImageProvider =
        const AssetImage('assets/images/person_male.png');

    if (_showingImageUrl.startsWith('http')) {
      showingImageProvider = NetworkImage(_showingImageUrl);
    } else {
      showingImageProvider = AssetImage(_showingImageUrl);
    }

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
              child: RideInfo(
                height: bottomContainerHeight,
                width: double.infinity,
                ride: _ride ?? widget.rideInfo,
                totalPrice: widget.totalPrice,
                directionResponse: widget.directionResponse,
                vehicleTypeName: widget.vehicleTypeName,
              )),
          Positioned(
              bottom: bottomContainerHeight - roundedBorderContainerHeight / 2,
              right: 20 + circularContainerWidth / 2,
              child: DriverInfoAndRideStatus(
                height: roundedBorderContainerHeight,
                width: roundedBorderContainerWidth,
                driver: _acceptedDriver,
                ride: _ride ?? widget.rideInfo,
                vehicle: _acceptedVehicle,
                isDriverNotFound: _isDriverNotFound,
                onTryAgainTap: _onTryAgainTap,
              )),
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
              child: ClipOval(
                child: Image(
                  image: showingImageProvider,
                  fit: _showingImageUrl.startsWith('http')
                      ? BoxFit.cover
                      : null, // or BoxFit.contain
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }
}
