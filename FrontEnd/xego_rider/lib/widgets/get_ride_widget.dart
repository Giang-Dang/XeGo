import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/info_section_container.dart';

class GetRideWidget extends StatefulWidget {
  const GetRideWidget({super.key});

  @override
  State<GetRideWidget> createState() => _GetRideWidgetState();
}

class _GetRideWidgetState extends State<GetRideWidget> {
  final _formGetRideKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
      child: Form(
        key: _formGetRideKey,
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            Text(
              'Where to?',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kTertiaryColor,
                    fontSize: 36,
                  ),
            ),
            const Gap(20),
            InfoSectionContainer(
              title: '',
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
