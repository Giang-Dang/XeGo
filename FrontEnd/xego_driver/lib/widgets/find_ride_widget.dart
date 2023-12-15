import 'dart:async';
import 'dart:developer';

import 'package:circular_countdown_timer/circular_countdown_timer.dart';
import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:intl/intl.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/screens/ride_screen.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/services/price_services.dart';
import 'package:xego_driver/services/ride_services.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/services/vehicle_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/widgets/image_input.dart';
import 'package:xego_driver/widgets/info_section_container.dart';
import 'package:xego_driver/widgets/info_section_single_child_scroll.dart';
import 'package:xego_driver/widgets/received_rides.dart';

class FindRideWidget extends StatefulWidget {
  const FindRideWidget({
    super.key,
    required this.isFindingRide,
    required this.receivedRidesList,
    required this.totalPriceList,
    required this.directionResponseList,
    required this.onRemoveItemInList,
    required this.onAcceptRide,
  });

  final bool isFindingRide;
  final List<Ride> receivedRidesList;
  final List<double> totalPriceList;
  final List<DirectionGoogleApiResponseDto> directionResponseList;
  final void Function(int) onRemoveItemInList;
  final void Function(int) onAcceptRide;

  @override
  State<FindRideWidget> createState() => _FindRideWidgetState();
}

class _FindRideWidgetState extends State<FindRideWidget> {
  @override
  Widget build(BuildContext context) {
    return Container(
        padding: const EdgeInsets.fromLTRB(0, 0, 0, 0),
        child: widget.isFindingRide
            ? widget.receivedRidesList == []
                ? const Center(
                    child: Text("Finding Ride..."),
                  )
                : ReceivedRidesWidget(
                    receivedRidesList: widget.receivedRidesList,
                    totalPriceList: widget.totalPriceList,
                    directionResponseList: widget.directionResponseList,
                    onRemoveItemInList: widget.onRemoveItemInList,
                    onAcceptRide: widget.onAcceptRide,
                  )
            : const Center(
                child:
                    Text("Press \"Power\" button to start searching for ride."),
              ));
  }
}
