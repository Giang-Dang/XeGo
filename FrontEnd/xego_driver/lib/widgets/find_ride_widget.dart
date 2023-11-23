import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/image_input.dart';
import 'package:xego_driver/widgets/info_section_container.dart';

class FindRideWidget extends StatefulWidget {
  const FindRideWidget({super.key});

  @override
  State<FindRideWidget> createState() => _FindRideWidgetState();
}

class _FindRideWidgetState extends State<FindRideWidget> {
  final _formCreateRideKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
      child: Form(
        key: _formCreateRideKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Where to?',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kPrimaryColor,
                    fontSize: 36,
                  ),
            ),
            const Gap(20),
            InfoSectionContainer(
              title: 'Avatar',
              titleColor: KColors.kTertiaryColor,
              padding: const EdgeInsets.all(14.0),
              innerPadding: const EdgeInsets.all(14.0),
              children: [],
            ),
          ],
        ),
      ),
    );
  }
}
