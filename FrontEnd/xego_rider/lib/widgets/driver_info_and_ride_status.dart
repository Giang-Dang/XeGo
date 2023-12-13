import 'package:flutter/material.dart';
import 'package:xego_rider/models/Entities/driver.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/models/Entities/vehicle.dart';
import 'package:xego_rider/services/ride_services.dart';
import 'package:xego_rider/settings/kColors.dart';

class DriverInfoAndRideStatus extends StatelessWidget {
  DriverInfoAndRideStatus({
    super.key,
    required this.height,
    required this.width,
    this.driver,
    required this.ride,
    this.vehicle,
  });

  final double height;
  final double width;
  final Driver? driver;
  final Ride ride;
  final Vehicle? vehicle;

  final _rideServices = RideServices();

  @override
  Widget build(BuildContext context) {
    return Container(
      height: height,
      width: width,
      padding: EdgeInsets.fromLTRB((driver == null) ? 40 : 10, 5, 50, 5),
      decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(15),
          color: KColors.kWhite,
          border: Border.all(color: KColors.kTertiaryColor)),
      child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          mainAxisAlignment: (driver == null)
              ? MainAxisAlignment.center
              : MainAxisAlignment.start,
          children: [
            if (driver != null)
              RichText(
                text: TextSpan(
                  children: [
                    TextSpan(
                      text: 'Name: ',
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                          ),
                    ),
                    TextSpan(
                      text: "${driver!.firstName}, ${driver!.firstName}",
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.bold,
                          ),
                    ),
                  ],
                ),
              ),
            if (driver != null)
              RichText(
                text: TextSpan(
                  children: [
                    TextSpan(
                      text: 'Tel.: ',
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                          ),
                    ),
                    TextSpan(
                      text: driver!.phoneNumber,
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
                            color: KColors.kTertiaryColor,
                            fontWeight: FontWeight.bold,
                          ),
                    ),
                  ],
                ),
              ),
            if (vehicle != null)
              RichText(
                text: TextSpan(
                  children: [
                    TextSpan(
                      text: 'Plate Number: ',
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
                            color: KColors.kColor6,
                            fontWeight: FontWeight.bold,
                          ),
                    ),
                    TextSpan(
                      text: vehicle!.plateNumber,
                      style: Theme.of(context).textTheme.bodySmall!.copyWith(
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
                    text: _rideServices.getShowingRideStatus(ride.status),
                    style: Theme.of(context).textTheme.bodyMedium!.copyWith(
                          color: KColors.kPrimaryColor,
                          fontWeight: FontWeight.bold,
                        ),
                  ),
                ],
              ),
            ),
          ]),
    );
  }
}
