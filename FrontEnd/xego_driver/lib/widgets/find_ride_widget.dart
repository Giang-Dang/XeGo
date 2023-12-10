import 'dart:async';
import 'dart:developer';

import 'package:circular_countdown_timer/circular_countdown_timer.dart';
import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/widgets/image_input.dart';
import 'package:xego_driver/widgets/info_section_container.dart';

class FindRideWidget extends StatefulWidget {
  const FindRideWidget({
    super.key,
    required this.isFindingRide,
    required this.receivedRidesList,
    required this.totalPriceList,
    required this.directionResponseList,
  });

  final bool isFindingRide;
  final List<Ride> receivedRidesList;
  final List<double> totalPriceList;
  final List<DirectionGoogleApiResponseDto> directionResponseList;

  @override
  State<FindRideWidget> createState() => _FindRideWidgetState();
}

class _FindRideWidgetState extends State<FindRideWidget> {
  Timer? _initialTimer;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initialTimer = Timer.periodic(
      const Duration(milliseconds: 100),
      (timer) {
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
        padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
        child: widget.isFindingRide
            ? widget.receivedRidesList == []
                ? const Center(
                    child: Text("Finding Ride..."),
                  )
                : ListView.builder(
                    itemCount: widget.receivedRidesList.length,
                    itemBuilder: (context, index) {
                      log(widget.receivedRidesList[index].riderId);
                      return Row(
                        children: [
                          Column(
                            children: [
                              Text(widget.receivedRidesList[index].id
                                  .toString()),
                              Text(widget.totalPriceList[index].toString()),
                              Text(widget.directionResponseList[index]
                                      .distanceText ??
                                  "N/A"),
                            ],
                          ),
                          CircularCountDownTimer(
                              width: 40,
                              height: 40,
                              duration: 30,
                              isReverse: true,
                              fillColor: KColors.kSuccessColor,
                              ringColor: KColors.kPrimaryColor)
                        ],
                      );
                    })
            : const Center(
                child:
                    Text("Press \"Power\" button to start searching for ride."),
              ));
  }
}
