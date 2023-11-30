import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/info_section_container.dart';
import 'package:xego_rider/widgets/where_to_box_widget.dart';

class HomeWidget extends StatefulWidget {
  const HomeWidget({super.key});

  @override
  State<HomeWidget> createState() => _HomeWidgetState();
}

class _HomeWidgetState extends State<HomeWidget> {
  @override
  Widget build(BuildContext context) {
    return const SingleChildScrollView(
      padding: EdgeInsets.fromLTRB(10, 15, 15, 30),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          InfoSectionContainer(
            title: null,
            padding: EdgeInsets.all(1.0),
            innerPadding: EdgeInsets.symmetric(vertical: 10),
            haveBoxBorder: false,
            children: [
              WhereToBoxWidget(),
            ],
          ),
          Divider(
            color: KColors.kColor5,
          ),
          InfoSectionContainer(
            title: 'Discount',
            titleFontSize: 26,
            titleFontWeight: FontWeight.w500,
            titleColor: KColors.kColor4,
            padding: EdgeInsets.all(1.0),
            innerPadding: EdgeInsets.all(1.0),
            haveBoxBorder: false,
            children: [],
          ),
        ],
      ),
    );
  }
}
