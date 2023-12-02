import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/models/Dto/create_ride_request_dto.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/screens/ride_screen.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/services/ride_services.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/settings/app_constants.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/settings/ride_status_constants.dart';
import 'package:xego_rider/widgets/choose_vehicle_field_widget.dart';
import 'package:xego_rider/widgets/date_time_picker_widget.dart';
import 'package:xego_rider/widgets/enter_pickup_location_widget.dart';
import 'package:xego_rider/widgets/enter_where_to_location_widget.dart';
import 'package:xego_rider/widgets/first_name_input_field.dart';
import 'package:xego_rider/widgets/info_section_container.dart';

class GetARideScreen extends StatefulWidget {
  const GetARideScreen({super.key});

  @override
  State<GetARideScreen> createState() => _GetARideScreenState();
}

class _GetARideScreenState extends State<GetARideScreen> {
  String? _startAddress;
  String? _destinationAddress;
  LatLng? _startLatLng;
  LatLng? _destinationLatLng;
  DirectionGoogleApiResponseDto? _directionResponse;
  DateTime? _pickupDateTime;
  int? _vehicleTypeId;
  String? _vehicleTypeName;
  double? _calculatedPrice;
  final bool _isVipRider =
      UserServices.riderType == AppConstants.kRiderType_Vip;

  final _rideServices = RideServices();

  final _locationServices = LocationServices();

  _showAlertDialog(String title, String message, void Function() onOkPressed) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: Text(message),
          actions: [
            TextButton(
              child: const Text('OK'),
              onPressed: () {
                onOkPressed();
              },
            ),
          ],
        );
      },
    );
  }

  _onOrderRideTap() async {
    final requestDto = CreateRideRequestDto(
      riderId: UserServices.userDto!.userId,
      vehicleTypeId: _vehicleTypeId!,
      status: RideStatusConstants.FindingDriver,
      startLatitude: _startLatLng!.latitude,
      startLongitude: _startLatLng!.longitude,
      startAddress: _startAddress!,
      destinationLatitude: _destinationLatLng!.latitude,
      destinationLongitude: _destinationLatLng!.longitude,
      destinationAddress: _destinationAddress!,
      distanceInMeters: (_directionResponse!.distanceValue ?? 0.0).toDouble(),
      pickupTime: _pickupDateTime ?? DateTime.now(),
      isScheduleRide: _pickupDateTime != null,
      modifiedBy: UserServices.userDto!.userId,
    );

    final response = await _rideServices.createRide(requestDto);

    log(response?.toJson().toString() ?? "createRide: null");

    if (response == null) {
      _showAlertDialog(
        'We\'re sorry!',
        'Something has gone wrong! Please try again later!',
        () {
          Navigator.of(context).pop();
        },
      );
    }

    if (context.mounted) {
      Navigator.of(context).push(
        MaterialPageRoute(
          builder: (BuildContext context) => RideScreen(
            rideInfo: response!,
            directionResponse: _directionResponse!,
            vehicleTypeName: _vehicleTypeName!,
            totalPrice: _calculatedPrice!,
          ),
        ),
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: const Text('Get A Ride'),
        ),
        body: SingleChildScrollView(
            padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
            child: Column(
              children: [
                InfoSectionContainer(
                  title: 'Where To?',
                  titleFontSize: 18,
                  titleColor: KColors.kPrimaryColor,
                  haveBoxBorder: true,
                  innerPadding: const EdgeInsets.symmetric(vertical: 10),
                  children: [
                    EnterPickUpLocationWidget(
                      initialLatLng: _startLatLng,
                      setPickUpLocation: (startLatLng, startAddress) async {
                        DirectionGoogleApiResponseDto? directionResponse;
                        if (_destinationLatLng != null) {
                          directionResponse =
                              await _locationServices.getPlaceDirectionDetails(
                                  startLatLng, _destinationLatLng!);
                        }
                        if (context.mounted) {
                          setState(() {
                            _startLatLng = startLatLng;
                            _startAddress = startAddress;
                            _directionResponse = null;
                            _vehicleTypeId = null;
                            _vehicleTypeName = null;
                            _calculatedPrice = null;
                            if (directionResponse != null) {
                              _directionResponse = directionResponse;
                              log(directionResponse.toString());
                            }
                          });
                          log(_startAddress ?? 'null');
                          log(_startLatLng.toString());
                        }
                      },
                    ),
                    const Gap(5),
                    EnterWhereToLocationWidget(
                      initialLatLng: _destinationLatLng,
                      setPickUpLocation:
                          (destinationLatLng, destinationAddress) async {
                        DirectionGoogleApiResponseDto? directionResponse;
                        if (_startLatLng != null) {
                          directionResponse =
                              await _locationServices.getPlaceDirectionDetails(
                                  _startLatLng!, destinationLatLng);
                        }
                        if (context.mounted) {
                          setState(() {
                            _destinationLatLng = destinationLatLng;
                            _destinationAddress = destinationAddress;
                            _directionResponse = null;
                            _vehicleTypeId = null;
                            _vehicleTypeName = null;
                            _calculatedPrice = null;
                            if (directionResponse != null) {
                              _directionResponse = directionResponse;
                              log(directionResponse.toString());
                            }
                          });
                          log(_destinationAddress ?? 'null');
                          log(_destinationLatLng.toString());
                        }
                      },
                    ),
                    const Gap(5),
                    if (_directionResponse != null)
                      Container(
                        padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
                        child: Container(
                          padding: const EdgeInsets.symmetric(
                              vertical: 5.0, horizontal: 15),
                          decoration: BoxDecoration(
                            borderRadius: BorderRadius.circular(20.0),
                          ),
                          child: Row(
                            children: [
                              Expanded(
                                flex: 1,
                                child: Text(
                                  'Distance: ${_directionResponse!.distanceText}',
                                  style: TextStyle(fontSize: 12),
                                ),
                              ),
                              Expanded(
                                  flex: 1,
                                  child: Text(
                                    'ETA: ${_directionResponse!.durationText}',
                                    style: TextStyle(fontSize: 12),
                                  )),
                            ],
                          ),
                        ),
                      ),
                  ],
                ),
                InfoSectionContainer(
                  title: 'Choose Vehicle',
                  titleColor: KColors.kPrimaryColor,
                  titleFontSize: 18,
                  innerPadding: EdgeInsets.fromLTRB(0, 15, 0, 15),
                  haveBoxBorder: true,
                  children: [
                    ChooseVehicleFieldWidget(
                        pickupLatLng: _startLatLng,
                        destinationLatLng: _destinationLatLng,
                        directionResponse: _directionResponse ??
                            DirectionGoogleApiResponseDto(),
                        vehicleTypeName: _vehicleTypeName,
                        calculatedPrice: _calculatedPrice,
                        setVehicleTypeId: (
                          vehicleTypeId,
                          vehicleTypeName,
                          calculatedPrice,
                        ) {
                          if (context.mounted) {
                            setState(() {
                              _vehicleTypeId = vehicleTypeId;
                              _vehicleTypeName = vehicleTypeName;
                              _calculatedPrice = calculatedPrice;
                            });
                          }
                        })
                  ],
                ),
                InfoSectionContainer(
                  title: 'Scheduled Ride (👑 VIP only)',
                  titleColor: _isVipRider ? KColors.kPrimaryColor : Colors.grey,
                  titleFontSize: 18,
                  haveBoxBorder: true,
                  innerPadding: const EdgeInsets.symmetric(vertical: 15),
                  isDisable: !_isVipRider,
                  children: [
                    DateTimePickerWidget(
                      isDisabled: !_isVipRider,
                      setPickUpTime: (pickupDateTime) {
                        if (context.mounted) {
                          setState(() {
                            _pickupDateTime = pickupDateTime;
                          });
                        }
                      },
                    )
                  ],
                ),
                ElevatedButton(
                    onPressed: _onOrderRideTap,
                    child: const Text('Order Ride')),
              ],
            )));
  }
}
