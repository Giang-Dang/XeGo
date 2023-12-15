import 'dart:async';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:intl/intl.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/screens/ride_screen.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/services/price_services.dart';
import 'package:xego_driver/services/ride_services.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/services/vehicle_services.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/ride_status_constants.dart';
import 'package:xego_driver/widgets/info_section_single_child_scroll.dart';

class CompletedRidesWidget extends StatefulWidget {
  const CompletedRidesWidget({super.key});

  @override
  State<CompletedRidesWidget> createState() => _CompletedRidesWidgetState();
}

class _CompletedRidesWidgetState extends State<CompletedRidesWidget> {
  final _priceServices = PriceServices();
  final _locationServices = LocationServices();
  final _vehicleServices = VehicleServices();
  final _rideServices = RideServices();

  Timer? _initialTimer;
  List<Ride> _completedRides = [];

  _initialize() async {
    final scheduledRides = await _rideServices.getAllRides(
      driverId: UserServices.userDto!.userId,
      status: RideStatusConstants.completed,
    );
    _completedRides.sort((a, b) => b.pickupTime.compareTo(a.pickupTime));
    if (mounted) {
      setState(() {
        _completedRides = scheduledRides;
      });
    }
  }

  // _onCompleteRideTap(Ride ride) async {
  //   try {
  //     final startLatLng = LatLng(ride.startLatitude, ride.startLongitude);
  //     final endLatLng =
  //         LatLng(ride.destinationLatitude, ride.destinationLongitude);

  //     final price = await _priceServices.getPriceByRideId(ride.id);
  //     final directionResponse = await _locationServices
  //         .getPlaceDirectionDetails(startLatLng, endLatLng);
  //     final vehicleTypes = await _vehicleServices.getAllActiveVehicleTypes();
  //     final vehicleType =
  //         vehicleTypes.firstWhere((v) => v.id == ride.vehicleTypeId);

  //     if (price == null || directionResponse == null) {
  //       log('_onCompleteRideTap:');
  //       log('price == null || directionResponse == null');
  //       return;
  //     }
  //     if (mounted) {
  //       Navigator.of(context).push(MaterialPageRoute(
  //           builder: (context) => RideScreen(
  //               ride: ride,
  //               directionResponse: directionResponse,
  //               totalPrice: price.totalPrice)));
  //     }
  //   } catch (e) {
  //     log('_onCompleteRideTap Error:');
  //     log(e.toString());
  //   }
  // }

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
    final screenHeight = MediaQuery.of(context).size.height;
    return InfoSectionSingleChildScroll(
      padding: EdgeInsets.all(10),
      innerPadding: EdgeInsets.all(10),
      haveBoxBorder: true,
      maxHeight: screenHeight - 260,
      children: [
        if (_completedRides.isEmpty)
          Center(
            child: Text(
              'There is no scheduled rides!',
              style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                    fontSize: 16,
                  ),
            ),
          ),
        if (_completedRides.isNotEmpty)
          ..._completedRides.map((r) => Card(
                child: ListTile(
                  onTap: () {
                    // _onCompleteRideTap(r);
                  },
                  title: RichText(
                    text: TextSpan(
                      children: [
                        TextSpan(
                          text: 'Ride ${r.id}',
                          style:
                              Theme.of(context).textTheme.titleMedium!.copyWith(
                                    color: KColors.kBlue,
                                    fontWeight: FontWeight.bold,
                                  ),
                        ),
                      ],
                    ),
                  ),
                  subtitle: Column(
                    crossAxisAlignment: CrossAxisAlignment.start,
                    children: [
                      const Gap(5),
                      RichText(
                        text: TextSpan(
                          children: [
                            TextSpan(
                              text: 'Pickup Time: ',
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kColor6,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                            TextSpan(
                              text: DateFormat('dd MMM yyyy   HH:mm')
                                  .format(r.pickupTime),
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kTertiaryColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                          ],
                        ),
                      ),
                      RichText(
                        text: TextSpan(
                          children: [
                            TextSpan(
                              text: 'Pickup: ',
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kColor6,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                            TextSpan(
                              text: r.startAddress,
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kTertiaryColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                          ],
                        ),
                      ),
                      RichText(
                        text: TextSpan(
                          children: [
                            TextSpan(
                              text: 'Dropoff: ',
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kColor6,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                            TextSpan(
                              text: r.destinationAddress,
                              style: Theme.of(context)
                                  .textTheme
                                  .bodySmall!
                                  .copyWith(
                                    color: KColors.kTertiaryColor,
                                    fontWeight: FontWeight.bold,
                                  ),
                            ),
                          ],
                        ),
                      ),
                    ],
                  ),
                ),
              ))
      ],
    );
  }
}
