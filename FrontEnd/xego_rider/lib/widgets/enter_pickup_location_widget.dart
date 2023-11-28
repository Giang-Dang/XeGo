import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/screens/choose_location_on_map_screen.dart';
import 'package:xego_rider/settings/kColors.dart';

class EnterPickUpLocationWidget extends StatefulWidget {
  const EnterPickUpLocationWidget({
    Key? key,
    this.initialLatLng,
    required this.setPickUpLocation,
  }) : super(key: key);

  final LatLng? initialLatLng;
  final Function(LatLng, String) setPickUpLocation;

  @override
  State<EnterPickUpLocationWidget> createState() =>
      _EnterPickUpLocationWidgetState();
}

class _EnterPickUpLocationWidgetState extends State<EnterPickUpLocationWidget> {
  String? _address;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
      child: GestureDetector(
        onTap: () {
          if (context.mounted) {
            Navigator.of(context).push(MaterialPageRoute(
              builder: (context) => ChooseLocationOnMapScreen(
                initialLatLng: widget.initialLatLng,
                onLocationConfirmed: (latlng, address) {
                  widget.setPickUpLocation(latlng, address);
                  if (mounted) {
                    setState(() {
                      _address = address;
                    });
                  }
                },
              ),
            ));
          }
        },
        child: Container(
          padding: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 15),
          decoration: BoxDecoration(
            border: Border.all(
              color: KColors.kTertiaryColor,
            ),
            borderRadius: BorderRadius.circular(20.0),
          ),
          child: Row(
            children: [
              Expanded(
                child: Text(
                  _address ?? 'Enter pickup location',
                  style: const TextStyle(
                    color: KColors.kTertiaryColor,
                    fontSize: 16,
                  ),
                ),
              ),
              const Gap(5),
              const Icon(
                Icons.map_outlined,
                color: KColors.kTertiaryColor,
              )
            ],
          ),
        ),
      ),
    );
  }
}
