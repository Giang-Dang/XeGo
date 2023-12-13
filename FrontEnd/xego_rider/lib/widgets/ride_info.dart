import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_rider/models/Entities/ride.dart';
import 'package:xego_rider/settings/kColors.dart';

class RideInfo extends StatelessWidget {
  const RideInfo({
    super.key,
    required this.ride,
    required this.totalPrice,
    required this.height,
    required this.width,
    required this.directionResponse,
    required this.vehicleTypeName,
  });

  final double height;
  final double width;
  final Ride ride;
  final double totalPrice;
  final String vehicleTypeName;
  final DirectionGoogleApiResponseDto directionResponse;

  @override
  Widget build(BuildContext context) {
    return Container(
      height: height,
      width: width,
      decoration: BoxDecoration(
        color: KColors.kSecondaryColor,
        borderRadius: const BorderRadius.only(
            topLeft: Radius.circular(8.0), topRight: Radius.circular(8.0)),
        border: Border.all(color: KColors.kColor4, width: 1.0),
      ),
      padding: const EdgeInsets.fromLTRB(20, 70, 20, 30),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Expanded(
                flex: 2,
                child: Text(
                  'Pick Up Location: ',
                  style: TextStyle(
                    color: KColors.kColor6,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
              Expanded(
                flex: 3,
                child: Text(
                  ride.startAddress,
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontWeight: FontWeight.normal,
                    fontSize: 14,
                  ),
                ),
              ),
            ],
          ),
          const Gap(5),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Expanded(
                flex: 2,
                child: Text(
                  'Drop Off Location: ',
                  style: TextStyle(
                    color: KColors.kColor6,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
              Expanded(
                flex: 3,
                child: Text(
                  ride.destinationAddress,
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontWeight: FontWeight.normal,
                    fontSize: 14,
                  ),
                ),
              ),
            ],
          ),
          Row(
            children: [
              const Expanded(flex: 8, child: Text('')),
              Expanded(
                flex: 7,
                child: RichText(
                  text: TextSpan(
                    style: const TextStyle(
                      color: KColors.kTextColor,
                      fontSize: 12,
                    ),
                    children: <TextSpan>[
                      const TextSpan(
                          text: 'Distance: ',
                          style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: KColors.kColor6)),
                      TextSpan(
                        text: directionResponse.distanceText,
                        style: const TextStyle(
                          color: KColors.kTertiaryColor,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              Expanded(
                flex: 5,
                child: RichText(
                  text: TextSpan(
                    style: const TextStyle(
                      color: KColors.kTextColor,
                      fontSize: 12,
                    ),
                    children: <TextSpan>[
                      const TextSpan(
                          text: 'ETA: ',
                          style: TextStyle(
                              fontWeight: FontWeight.bold,
                              color: KColors.kColor6)),
                      TextSpan(
                        text: directionResponse.durationText,
                        style: const TextStyle(
                          color: KColors.kTertiaryColor,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
            ],
          ),
          const Gap(7),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Expanded(
                flex: 2,
                child: Text(
                  'Vehicle Type: ',
                  style: TextStyle(
                    color: KColors.kColor6,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
              Expanded(
                flex: 3,
                child: Text(
                  vehicleTypeName,
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontWeight: FontWeight.normal,
                    fontSize: 14,
                  ),
                ),
              ),
            ],
          ),
          const Divider(color: KColors.kTertiaryColor),
          Row(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              const Expanded(
                flex: 2,
                child: Text(
                  'Total Price: ',
                  style: TextStyle(
                    color: KColors.kColor6,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              ),
              Expanded(
                flex: 3,
                child: Text(
                  '\$$totalPrice',
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                  textAlign: TextAlign.end,
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }
}
