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
import 'package:xego_driver/widgets/info_section_single_child_scroll.dart';

class ScheduledRidesWidget extends StatefulWidget {
  const ScheduledRidesWidget({super.key});

  @override
  State<ScheduledRidesWidget> createState() => _ScheduledRidesWidgetState();
}

class _ScheduledRidesWidgetState extends State<ScheduledRidesWidget> {
  final _priceServices = PriceServices();
  final _locationServices = LocationServices();
  final _vehicleServices = VehicleServices();
  final _rideServices = RideServices();

  Timer? _initialTimer;
  List<Ride> _scheduledRides = [];

  _initialize() async {
    final scheduledRides = await _rideServices.getAllRides(
      driverId: UserServices.userDto!.userId,
      isScheduleRide: true,
      rideFinished: false,
    );
    _scheduledRides.sort((a, b) => b.pickupTime.compareTo(a.pickupTime));
    if (mounted) {
      setState(() {
        _scheduledRides = scheduledRides;
      });
    }
  }

  _onScheduleRideTap(Ride ride) async {
    try {
      final startLatLng = LatLng(ride.startLatitude, ride.startLongitude);
      final endLatLng =
          LatLng(ride.destinationLatitude, ride.destinationLongitude);

      final price = await _priceServices.getPriceByRideId(ride.id);
      final directionResponse = await _locationServices
          .getPlaceDirectionDetails(startLatLng, endLatLng);
      final vehicleTypes = await _vehicleServices.getAllActiveVehicleTypes();
      final vehicleType =
          vehicleTypes.firstWhere((v) => v.id == ride.vehicleTypeId);

      if (price == null || directionResponse == null) {
        log('_onScheduleRideTap:');
        log('price == null || directionResponse == null');
        return;
      }
      if (mounted) {
        Navigator.of(context).push(MaterialPageRoute(
            builder: (context) => RideScreen(
                ride: ride,
                directionResponse: directionResponse,
                totalPrice: price.totalPrice)));
      }
    } catch (e) {
      log('_onScheduleRideTap Error:');
      log(e.toString());
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
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    final screenHeight = MediaQuery.of(context).size.height;
    return InfoSectionSingleChildScroll(
      title: 'Scheduled Rides:',
      titleFontSize: 19,
      titleFontWeight: FontWeight.bold,
      titleColor: KColors.kPrimaryColor,
      padding: EdgeInsets.all(10),
      innerPadding: EdgeInsets.all(10),
      haveBoxBorder: true,
      maxHeight: screenHeight - 250,
      children: [
        if (_scheduledRides.isEmpty)
          Center(
            child: Text(
              'There is no scheduled rides!',
              style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                    fontSize: 16,
                  ),
            ),
          ),
        if (_scheduledRides.isNotEmpty)
          ..._scheduledRides.map((r) => Card(
                child: ListTile(
                  onTap: () {
                    _onScheduleRideTap(r);
                  },
                  title: Row(
                    children: [
                      Expanded(
                        child: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: 'Ride ${r.id}',
                                style: Theme.of(context)
                                    .textTheme
                                    .titleMedium!
                                    .copyWith(
                                      color: KColors.kPrimaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                      Expanded(
                        child: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: r.driverId == null
                                    ? 'Not Assign Driver Yet!'
                                    : 'Driver Assigned',
                                style: Theme.of(context)
                                    .textTheme
                                    .titleSmall!
                                    .copyWith(
                                      color: r.driverId == null
                                          ? KColors.kDanger
                                          : KColors.kSuccessColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ],
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
