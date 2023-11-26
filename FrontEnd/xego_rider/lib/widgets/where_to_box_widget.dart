import 'package:flutter/material.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/choose_end_point_locations_widget.dart';

class WhereToBoxWidget extends StatefulWidget {
  const WhereToBoxWidget({Key? key}) : super(key: key);

  @override
  State<WhereToBoxWidget> createState() => _WhereToBoxWidgetState();
}

class _WhereToBoxWidgetState extends State<WhereToBoxWidget> {
  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
      child: GestureDetector(
        onTap: () {
          if (context.mounted) {
            Navigator.of(context).push(MaterialPageRoute(
              builder: (context) => const ChooseEndPointLocations(),
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
          child: const Row(
            children: [
              Expanded(
                child: Text(
                  'Search',
                  style: TextStyle(
                    color: Colors.grey,
                  ),
                ),
              ),
              Icon(Icons.route),
            ],
          ),
        ),
      ),
    );
  }
}
