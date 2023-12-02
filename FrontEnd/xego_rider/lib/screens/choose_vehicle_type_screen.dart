import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Dto/vehicle_type_calculated_price_info_dto.dart';
import 'package:xego_rider/widgets/choose_vehicle_type_sheet_widget.dart';
import 'package:xego_rider/widgets/map_widget.dart';

class ChooseVehicleTypeScreen extends StatefulWidget {
  const ChooseVehicleTypeScreen(
      {super.key,
      required this.pickupLatLng,
      required this.destinationLatLng,
      required this.currentRiderLatLng,
      this.directionResponse,
      required this.vehicleTypePriceInfoList,
      required this.setVehicleTypeId});

  final LatLng pickupLatLng;
  final LatLng destinationLatLng;
  final LatLng currentRiderLatLng;
  final List<VehicleTypeCalculatedPriceInfoDto> vehicleTypePriceInfoList;
  final DirectionGoogleApiResponseDto? directionResponse;
  final Function(int, String, double) setVehicleTypeId;

  @override
  State<ChooseVehicleTypeScreen> createState() =>
      _ChooseVehicleTypeScreenState();
}

class _ChooseVehicleTypeScreenState extends State<ChooseVehicleTypeScreen> {
  @override
  void initState() {
    // TODO: implement initState
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    log(widget.pickupLatLng.toString());
    log(widget.destinationLatLng.toString());
    log(widget.currentRiderLatLng.toString());
    log(widget.directionResponse.toString());
    log(widget.vehicleTypePriceInfoList.toString());

    return Scaffold(
        body: Stack(children: [
      MapWidget(
        pickUpLocation: widget.pickupLatLng,
        destinationLocation: widget.destinationLatLng,
        riderLocation: widget.currentRiderLatLng,
        directionGoogleApiDto: widget.directionResponse,
      ),
      ChooseVehicleTypeSheetWidget(
        vehicleTypePriceList: widget.vehicleTypePriceInfoList,
        setVehicleTypeId: widget.setVehicleTypeId,
      ),
    ]));
  }
}
