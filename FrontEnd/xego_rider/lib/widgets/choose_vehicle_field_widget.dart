import 'dart:async';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Dto/vehicle_type_calculated_price_info_dto.dart';
import 'package:xego_rider/screens/choose_vehicle_type_screen.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/services/vehicle_services.dart';
import 'package:xego_rider/settings/kColors.dart';

class ChooseVehicleFieldWidget extends StatefulWidget {
  const ChooseVehicleFieldWidget({
    Key? key,
    required this.directionResponse,
    required this.setVehicleTypeId,
    this.discountId,
    this.pickupLatLng,
    this.destinationLatLng,
    this.vehicleTypeName,
    this.calculatedPrice,
  }) : super(key: key);

  final LatLng? pickupLatLng;
  final LatLng? destinationLatLng;
  final DirectionGoogleApiResponseDto directionResponse;
  final Function(int, String, double) setVehicleTypeId;
  final int? discountId;
  final String? vehicleTypeName;
  final double? calculatedPrice;

  @override
  State<ChooseVehicleFieldWidget> createState() =>
      _ChooseVehicleFieldWidgetState();
}

class _ChooseVehicleFieldWidgetState extends State<ChooseVehicleFieldWidget> {
  late List<VehicleTypeCalculatedPriceInfoDto>?
      _vehicleTypeCalculatedPriceInfoDto;
  Timer? _initialTimer;
  final _vehicleServices = VehicleServices();

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

  _initialize() async {}

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
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
      child: GestureDetector(
        onTap: () async {
          if (widget.pickupLatLng == null || widget.destinationLatLng == null) {
            _showAlertDialog('Location Selection Required',
                'Before selecting a vehicle, please ensure that both your pick-up and drop-off locations have been chosen.',
                () {
              if (context.mounted) {
                Navigator.of(context).pop();
              }
            });
            return;
          }

          final vehicleTypeCalculatedPriceList =
              await _vehicleServices.getAllActiveVehicleTypeCalculatedPriceInfo(
                  widget.directionResponse.distanceValue?.toDouble() ?? 0,
                  widget.discountId);

          _vehicleTypeCalculatedPriceInfoDto = vehicleTypeCalculatedPriceList;

          if (context.mounted) {
            Navigator.of(context).push(MaterialPageRoute(
              builder: (context) => ChooseVehicleTypeScreen(
                pickupLatLng: widget.pickupLatLng!,
                destinationLatLng: widget.destinationLatLng!,
                currentRiderLatLng:
                    LocationServices.currentLocation ?? widget.pickupLatLng!,
                directionResponse: widget.directionResponse,
                vehicleTypePriceInfoList:
                    _vehicleTypeCalculatedPriceInfoDto ?? [],
                setVehicleTypeId: widget.setVehicleTypeId,
              ),
            ));
          }
        },
        child: Container(
          padding: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 15),
          decoration: BoxDecoration(
            border: Border.all(
              color: KColors.kTertiaryColor,
            ),
            borderRadius: BorderRadius.circular(20.0),
          ),
          child: Row(
            children: [
              Expanded(
                flex: widget.calculatedPrice == null ? 1 : 3,
                child: Text(
                  widget.vehicleTypeName ?? 'Press here to select vehicle',
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontSize: 14,
                  ),
                ),
              ),
              if (widget.calculatedPrice != null)
                Expanded(
                  flex: 1,
                  child: Text(
                    '\$${widget.calculatedPrice}',
                    style: const TextStyle(
                      color: KColors.kColor6,
                      fontSize: 14,
                    ),
                  ),
                ),
            ],
          ),
        ),
      ),
    );
  }
}
