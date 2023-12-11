import 'package:flutter/material.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/settings/kColors.dart';

class RideInfoWidget extends StatelessWidget {
  const RideInfoWidget(
      {super.key, this.ride, this.totalPrice, this.directionResponse});

  final Ride? ride;
  final double? totalPrice;
  final DirectionGoogleApiResponseDto? directionResponse;

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Ride Info:',
            textAlign: TextAlign.start,
            style: TextStyle(
                fontSize: Theme.of(context).textTheme.titleMedium!.fontSize,
                color: KColors.kPrimaryColor,
                fontWeight: FontWeight.bold),
          ),
          Container(
            padding: EdgeInsets.fromLTRB(10, 10, 10, 10),
            decoration: BoxDecoration(
              color: KColors.kSuperLightSecondaryColor,
              borderRadius: BorderRadius.circular(10.0),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                RichText(
                  text: TextSpan(
                    children: [
                      TextSpan(
                        text: 'Pick Up: ',
                        style: Theme.of(context).textTheme.bodySmall!.copyWith(
                              color: KColors.kColor6,
                              fontWeight: FontWeight.bold,
                            ),
                      ),
                      TextSpan(
                        text: ride == null ? "N/A" : ride!.startAddress,
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
                        text: 'Drop off: ',
                        style: Theme.of(context).textTheme.bodySmall!.copyWith(
                              color: KColors.kColor6,
                              fontWeight: FontWeight.bold,
                            ),
                      ),
                      TextSpan(
                        text: ride == null ? "N/A" : ride!.destinationAddress,
                        style: Theme.of(context).textTheme.bodySmall!.copyWith(
                              color: KColors.kTertiaryColor,
                              fontWeight: FontWeight.bold,
                            ),
                      ),
                    ],
                  ),
                ),
                Row(children: [
                  Expanded(
                    flex: 1,
                    child: RichText(
                      text: TextSpan(
                        children: [
                          TextSpan(
                            text: 'Distance: ',
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                          TextSpan(
                            text: directionResponse == null
                                ? "N/A"
                                : directionResponse!.distanceText,
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  Expanded(
                    flex: 1,
                    child: RichText(
                      textAlign: TextAlign.center,
                      text: TextSpan(
                        children: [
                          TextSpan(
                            text: 'ETA: ',
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                          TextSpan(
                            text: directionResponse == null
                                ? "N/A"
                                : directionResponse!.durationText,
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                        ],
                      ),
                    ),
                  ),
                  Expanded(
                    flex: 1,
                    child: RichText(
                      text: TextSpan(
                        children: [
                          TextSpan(
                            text: 'Total Price: ',
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                          TextSpan(
                            text: totalPrice == null
                                ? "N/A"
                                : '\$${totalPrice.toString()}',
                            style:
                                Theme.of(context).textTheme.bodySmall!.copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                          ),
                        ],
                      ),
                    ),
                  ),
                ])
              ],
            ),
          ),
        ],
      ),
    );
  }
}
