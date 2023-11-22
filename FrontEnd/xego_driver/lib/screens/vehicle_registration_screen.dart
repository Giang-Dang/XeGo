import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/info_section_container.dart';

class VehicleRegistrationScreen extends StatefulWidget {
  const VehicleRegistrationScreen({super.key});

  @override
  State<VehicleRegistrationScreen> createState() =>
      _VehicleRegistrationScreenState();
}

class _VehicleRegistrationScreenState extends State<VehicleRegistrationScreen> {
  final _formVehicleRegisterKey = GlobalKey<FormState>();

  bool _isRegistering = false;

  _onNextPressed() async {}

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(Constants.kTopScreenAppTitle),
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.fromLTRB(10, 15, 15, 30),
        child: Form(
          key: _formVehicleRegisterKey,
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Text(
                'User Registration',
                style: Theme.of(context).textTheme.titleLarge!.copyWith(
                      color: KColors.kPrimaryColor,
                      fontSize: 30,
                    ),
              ),
              const Gap(20),
              InfoSectionContainer(
                title: 'Required Info',
                titleColor: KColors.kPrimaryColor.withOpacity(0.8),
                padding: const EdgeInsets.all(14.0),
                innerPadding: const EdgeInsets.all(14.0),
                children: [],
              ),
              Center(
                child: ElevatedButton(
                  onPressed: () {
                    _onNextPressed();
                  },
                  child: _isRegistering
                      ? const CircularProgressIndicator()
                      : const Text('Next'),
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
