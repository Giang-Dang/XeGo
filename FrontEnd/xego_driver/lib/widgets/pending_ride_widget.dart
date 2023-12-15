import 'package:circular_countdown_timer/circular_countdown_timer.dart';
import 'package:flutter/material.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/settings/kColors.dart';

class PendingRideWidget extends StatefulWidget {
  const PendingRideWidget({
    super.key,
    required this.ride,
    required this.totalPrice,
    required this.directionResponse,
    required this.onTap,
    required this.itemIndex,
    required this.showButtons,
    required this.onAccept,
    required this.onDecline,
    required this.onRemove,
  });

  final int itemIndex;
  final Ride ride;
  final double totalPrice;
  final DirectionGoogleApiResponseDto directionResponse;
  final void Function(int?) onTap;
  final bool showButtons;
  final void Function(int) onAccept;
  final void Function(int) onDecline;
  final void Function(int) onRemove;

  @override
  State<PendingRideWidget> createState() => _PendingRideWidgetState();
}

class _PendingRideWidgetState extends State<PendingRideWidget> {
  @override
  Widget build(BuildContext context) {
    return Column(
      children: [
        Card(
          // color: widget.ride.isScheduleRide ? KColors.kSuperLightDanger : null,
          child: ListTile(
            onTap: () {
              if (widget.showButtons) {
                widget.onTap(null);
              } else {
                widget.onTap(widget.itemIndex);
              }
            },
            trailing: CircularCountDownTimer(
              width: 40,
              height: 40,
              duration: 45,
              isReverse: true,
              autoStart: true,
              onComplete: () {
                widget.onRemove(widget.itemIndex);
              },
              textStyle: Theme.of(context).textTheme.bodySmall!.copyWith(),
              backgroundColor: KColors.kWhite,
              fillColor: KColors.kWhite,
              ringColor: KColors.kPrimaryColor,
            ),
            title: Text(
              widget.ride.isScheduleRide ? 'Scheduled Ride' : 'Normal Ride',
              style: Theme.of(context).textTheme.titleMedium!.copyWith(
                  color: widget.ride.isScheduleRide
                      ? KColors.kDanger
                      : KColors.kColor4,
                  fontWeight: FontWeight.bold),
            ),
            subtitle: Row(
              children: [
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
                          text: widget.directionResponse.distanceText,
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
                          text: '\$${widget.totalPrice.toString()}',
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
              ],
            ),
          ),
        ),
        if (widget.showButtons)
          Row(
            mainAxisAlignment: MainAxisAlignment.spaceAround,
            children: [
              ElevatedButton(
                onPressed: () {
                  widget.onDecline(widget.itemIndex);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: KColors.kDanger,
                  foregroundColor: KColors.kWhite,
                ),
                child: Text(
                  'Decline Ride',
                  style: Theme.of(context).textTheme.bodySmall!.copyWith(
                      color: KColors.kWhite, fontWeight: FontWeight.bold),
                ),
              ),
              ElevatedButton(
                onPressed: () {
                  widget.onAccept(widget.itemIndex);
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: KColors.kSuccessColor,
                  foregroundColor: KColors.kWhite,
                ),
                child: Text(
                  'Accept Ride',
                  style: Theme.of(context).textTheme.bodySmall!.copyWith(
                      color: KColors.kWhite, fontWeight: FontWeight.bold),
                ),
              ),
            ],
          ),
      ],
    );
  }
}
