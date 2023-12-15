import 'dart:convert';
import 'dart:developer';

import 'package:circular_countdown_timer/circular_countdown_timer.dart';
import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/map_widget.dart';
import 'package:xego_driver/widgets/pending_ride_widget.dart';
import 'package:xego_driver/widgets/ride_info_widget.dart';

class ReceivedRidesWidget extends StatefulWidget {
  const ReceivedRidesWidget(
      {super.key,
      required this.receivedRidesList,
      required this.totalPriceList,
      required this.directionResponseList,
      required this.onRemoveItemInList,
      required this.onAcceptRide});

  final List<Ride> receivedRidesList;
  final List<double> totalPriceList;
  final List<DirectionGoogleApiResponseDto> directionResponseList;
  final void Function(int) onRemoveItemInList;
  final void Function(int) onAcceptRide;

  @override
  State<ReceivedRidesWidget> createState() => _ReceivedRidesWidgetState();
}

class _ReceivedRidesWidgetState extends State<ReceivedRidesWidget> {
  int? _selectedIndex;
  LatLng? _pickUpLocation;
  LatLng? _dropOffLocation;
  DirectionGoogleApiResponseDto? _directionResponse;

  _onSelectRide(int? selectedRideIndex) async {
    if (!context.mounted) {
      return;
    }
    if (selectedRideIndex != null && widget.receivedRidesList.isNotEmpty) {
      log(jsonEncode(widget.directionResponseList[selectedRideIndex].toJson())
          .toString());
      setState(() {
        _pickUpLocation = LatLng(
            widget.receivedRidesList[selectedRideIndex].startLatitude,
            widget.receivedRidesList[selectedRideIndex].startLongitude);
        _dropOffLocation = LatLng(
            widget.receivedRidesList[selectedRideIndex].destinationLatitude,
            widget.receivedRidesList[selectedRideIndex].destinationLongitude);
        _directionResponse = widget.directionResponseList[selectedRideIndex];
        log("_onSelectRide: ${jsonEncode(_directionResponse?.toJson() ?? "_directionResponse: null")}");
      });
    } else {
      setState(() {
        _pickUpLocation = null;
        _dropOffLocation = null;
        _directionResponse = null;
        _selectedIndex = null;
      });
    }

    setState(() {
      _selectedIndex = selectedRideIndex;
    });
  }

  _onAcceptRide(int index) {
    widget.onAcceptRide(index);
  }

  _onDeclineRide(int index) {
    widget.onRemoveItemInList(index);
    if (mounted) {
      setState(() {
        _pickUpLocation = null;
        _dropOffLocation = null;
        _directionResponse = null;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    double screenWidth = MediaQuery.of(context).size.width;
    double screenHeight = MediaQuery.of(context).size.height;
    double minDraggableSheetChildSize = 0.2;
    double initialDraggableSheetChildSize = 0.2;
    log("ReceivedRide > _directionResponse: ${jsonEncode(_directionResponse?.toJson())}");
    return Stack(children: [
      Container(
        height: screenHeight,
        width: screenWidth,
        color: Colors.amber,
      ),
      SizedBox(
        height: screenHeight * (1 - minDraggableSheetChildSize) - 115,
        child: MapWidget(
          key: Key(_selectedIndex.toString()),
          pickUpLocation: _pickUpLocation,
          destinationLocation: _dropOffLocation,
          directionGoogleApiDto: _directionResponse,
          myCarLocation: LocationServices.currentLocation!,
          mapMyLocationEnabled: false,
          mapZoomControllerEnabled: true,
          markerOutterPadding: 100,
        ),
      ),
      DraggableScrollableSheet(
          initialChildSize: initialDraggableSheetChildSize,
          minChildSize: minDraggableSheetChildSize,
          maxChildSize: 1,
          builder: (BuildContext context, ScrollController scrollController) {
            return Container(
              decoration: const BoxDecoration(
                color: KColors.kOnBackgroundColor,
                borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(24.0),
                  topRight: Radius.circular(24.0),
                ),
              ),
              child: Container(
                margin: const EdgeInsets.fromLTRB(5, 5, 5, 5),
                padding: const EdgeInsets.fromLTRB(10, 10, 10, 10),
                child: ListView.builder(
                    controller: scrollController,
                    padding: const EdgeInsets.only(top: 0),
                    itemCount: widget.receivedRidesList.length + 2,
                    itemBuilder: (context, index) {
                      if (index == 0) {
                        return const Center(
                          child: Icon(
                            Icons.drag_handle,
                            color: KColors.kTertiaryColor,
                            size: 30,
                          ),
                        );
                      } else if (index == 1) {
                        return widget.receivedRidesList.isEmpty
                            ? Center(
                                child: Column(
                                  children: [
                                    const Gap(10),
                                    Image.asset(
                                      'assets/images/looking_for_driver.gif',
                                      height: 40,
                                      width: 40,
                                    ),
                                    const Gap(5),
                                    Text(
                                      'Waiting for rides...',
                                      textAlign: TextAlign.center,
                                      style: TextStyle(
                                        fontSize: Theme.of(context)
                                            .textTheme
                                            .bodyMedium!
                                            .fontSize,
                                      ),
                                    ),
                                  ],
                                ),
                              )
                            : Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  RideInfoWidget(
                                    ride: widget.receivedRidesList.isEmpty ||
                                            _selectedIndex == null
                                        ? null
                                        : widget
                                            .receivedRidesList[_selectedIndex!],
                                    totalPrice: widget.totalPriceList.isEmpty ||
                                            _selectedIndex == null
                                        ? null
                                        : widget
                                            .totalPriceList[_selectedIndex!],
                                    directionResponse:
                                        widget.directionResponseList.isEmpty ||
                                                _selectedIndex == null
                                            ? null
                                            : widget.directionResponseList[
                                                _selectedIndex!],
                                  ),
                                  const Divider(
                                    color: KColors.kSecondaryColor,
                                  ),
                                  Text(
                                    'Pending Rides:',
                                    textAlign: TextAlign.start,
                                    style: TextStyle(
                                        fontSize: Theme.of(context)
                                            .textTheme
                                            .titleMedium!
                                            .fontSize,
                                        color: KColors.kPrimaryColor,
                                        fontWeight: FontWeight.bold),
                                  ),
                                ],
                              );
                      } else {
                        int actualIndex = index - 2;
                        log(widget.receivedRidesList[actualIndex].riderId);

                        return PendingRideWidget(
                          itemIndex: actualIndex,
                          showButtons: _selectedIndex == actualIndex,
                          ride: widget.receivedRidesList[actualIndex],
                          totalPrice: widget.totalPriceList[actualIndex],
                          directionResponse:
                              widget.directionResponseList[actualIndex],
                          onTap: _onSelectRide,
                          onAccept: _onAcceptRide,
                          onDecline: _onDeclineRide,
                          onRemove: _onDeclineRide,
                        );
                      }
                    }),
              ),
            );
          }),
    ]);
  }
}
