import 'dart:async';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/widgets/image_input.dart';
import 'package:xego_driver/widgets/info_section_container.dart';

class FindRideWidget extends StatefulWidget {
  const FindRideWidget({
    super.key,
    required this.isFindingRide,
  });

  final bool isFindingRide;

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
            ? const Center(
                child: Text("Finding Ride..."),
              )
            : const Center(
                child:
                    Text("Press \"Power\" button to start searching for ride."),
              ));
  }
}
