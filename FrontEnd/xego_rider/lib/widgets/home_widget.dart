import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:intl/intl.dart';
import 'package:xego_rider/models/Dto/vehicle_type_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/screens/ride_screen.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/services/price_services.dart';
import 'package:xego_rider/services/ride_services.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/services/vehicle_services.dart';
import 'package:xego_rider/settings/app_constants.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/info_section_container.dart';
import 'package:xego_rider/widgets/info_section_single_child_scroll.dart';
import 'package:xego_rider/widgets/where_to_box_widget.dart';

class HomeWidget extends StatefulWidget {
  const HomeWidget({super.key});

  @override
  State<HomeWidget> createState() => _HomeWidgetState();
}

class _HomeWidgetState extends State<HomeWidget> {
  final _userServices = UserServices();
  final _rideServices = RideServices();
  final _priceServices = PriceServices();
  final _locationServices = LocationServices();
  final _vehicleServices = VehicleServices();

  List<Ride> _scheduledRides = [];
  bool? _isVipRider = false;

  _initialize() async {
    _userServices.updateRiderType(UserServices.userDto!.userId);
    final isVipRider = UserServices.riderType == AppConstants.kRiderType_Vip;

    if (isVipRider) {
      final scheduledRides = await _rideServices.getAllRides(
        riderId: UserServices.userDto!.userId,
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

    if (mounted) {
      setState(() {
        _isVipRider = isVipRider;
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
                rideInfo: ride,
                directionResponse: directionResponse,
                vehicleTypeName: vehicleType.name,
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
    _initialize();
  }

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const InfoSectionContainer(
            title: 'Welcome!',
            titleColor: KColors.kPrimaryColor,
            titleFontSize: 26,
            titleFontWeight: FontWeight.bold,
            padding: EdgeInsets.all(1.0),
            innerPadding: EdgeInsets.symmetric(vertical: 20),
            haveBoxBorder: true,
            children: [
              WhereToBoxWidget(),
            ],
          ),
          const Divider(
            color: KColors.kColor5,
          ),
          Expanded(
            child: InfoSectionSingleChildScroll(
              title: 'Scheduled Rides:',
              titleFontSize: 22,
              titleFontWeight: FontWeight.bold,
              titleColor: KColors.kPrimaryColor,
              padding: EdgeInsets.all(1.0),
              innerPadding: EdgeInsets.all(10),
              haveBoxBorder: true,
              maxHeight: 400,
              children: [
                if (_isVipRider == false)
                  Center(
                    child: Text(
                      'Only available for Vip user!',
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 16,
                          ),
                    ),
                  ),
                if (_isVipRider == true && _scheduledRides.isEmpty)
                  Center(
                    child: Text(
                      'There is no scheduled rides!',
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 16,
                          ),
                    ),
                  ),
                if (_isVipRider == true && _scheduledRides.isNotEmpty)
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
            ),
          ),
        ],
      ),
    );
  }
}
