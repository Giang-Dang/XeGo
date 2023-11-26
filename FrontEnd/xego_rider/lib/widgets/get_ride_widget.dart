import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/info_section_container.dart';
import 'package:xego_rider/widgets/where_to_box_widget.dart';

class GetRideWidget extends StatefulWidget {
  const GetRideWidget({super.key});

  @override
  State<GetRideWidget> createState() => _GetRideWidgetState();
}

class _GetRideWidgetState extends State<GetRideWidget> {
  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          const InfoSectionContainer(
            title: 'Where to?',
            titleFontSize: 26,
            titleFontWeight: FontWeight.w700,
            titleColor: KColors.kTertiaryColor,
            padding: EdgeInsets.all(1.0),
            innerPadding: EdgeInsets.all(1.0),
            children: [
              WhereToBoxWidget(),
            ],
            haveBoxBorder: false,
          ),
          InfoSectionContainer(
            title: 'Discount',
            titleFontSize: 26,
            titleFontWeight: FontWeight.w500,
            titleColor: KColors.kColor4,
            padding: EdgeInsets.all(1.0),
            innerPadding: EdgeInsets.all(1.0),
            children: [
              WhereToBoxWidget(),
            ],
            haveBoxBorder: false,
          ),
        ],
      ),
    );
  }
}
