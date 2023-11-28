import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/enter_pickup_location_widget.dart';
import 'package:xego_rider/widgets/enter_where_to_location_widget.dart';
import 'package:xego_rider/widgets/first_name_input_field.dart';
import 'package:xego_rider/widgets/info_section_container.dart';

class ChooseEndPointLocationsScreen extends StatefulWidget {
  const ChooseEndPointLocationsScreen({super.key});

  @override
  State<ChooseEndPointLocationsScreen> createState() =>
      _ChooseEndPointLocationsScreenState();
}

class _ChooseEndPointLocationsScreenState
    extends State<ChooseEndPointLocationsScreen> {
  String? _startAddress;
  String? _destinationAddress;
  LatLng? _startLatLng;
  LatLng? _destinationLatLng;

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
                  title: 'Pickup Location',
                  titleColor: KColors.kPrimaryColor,
                  haveBoxBorder: false,
                  innerPadding: const EdgeInsets.symmetric(vertical: 10),
                  children: [
                    EnterPickUpLocationWidget(
                      initialLatLng: _startLatLng,
                      setPickUpLocation: (startLatLng, startAddress) {
                        if (context.mounted) {
                          setState(() {
                            _startLatLng = startLatLng;
                            _startAddress = startAddress;
                          });
                          log(_startAddress ?? 'null');
                          log(_startLatLng.toString());
                        }
                      },
                    ),
                  ],
                ),
                InfoSectionContainer(
                  title: 'Drop Off Location',
                  titleColor: KColors.kPrimaryColor,
                  haveBoxBorder: false,
                  innerPadding: const EdgeInsets.symmetric(vertical: 10),
                  children: [
                    EnterWhereToLocationWidget(
                      initialLatLng: _destinationLatLng,
                      setPickUpLocation:
                          (destinationLatLng, destinationAddress) {
                        if (context.mounted) {
                          setState(() {
                            _destinationLatLng = destinationLatLng;
                            _destinationAddress = destinationAddress;
                          });
                          log(_destinationAddress ?? 'null');
                          log(_destinationLatLng.toString());
                        }
                      },
                    ),
                  ],
                ),
                InfoSectionContainer(
                  title: 'Schedule Ride (👑 VIP only)',
                  titleColor: KColors.kColor4,
                  haveBoxBorder: false,
                  innerPadding: const EdgeInsets.symmetric(vertical: 10),
                  children: [
                    EnterWhereToLocationWidget(
                      initialLatLng: _destinationLatLng,
                      setPickUpLocation:
                          (destinationLatLng, destinationAddress) {
                        if (context.mounted) {
                          setState(() {
                            _destinationLatLng = destinationLatLng;
                            _destinationAddress = destinationAddress;
                          });
                          log(_destinationAddress ?? 'null');
                          log(_destinationLatLng.toString());
                        }
                      },
                    ),
                  ],
                )
              ],
            )));
  }
}
