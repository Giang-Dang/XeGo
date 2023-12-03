import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_driver/settings/app_constants.dart';
import 'package:xego_driver/settings/kColors.dart';

class DoNotHaveVehicleScreen extends StatefulWidget {
  const DoNotHaveVehicleScreen({super.key});

  @override
  State<DoNotHaveVehicleScreen> createState() => _DoNotHaveVehicleScreenState();
}

class _DoNotHaveVehicleScreenState extends State<DoNotHaveVehicleScreen> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(AppConstants.kTopScreenAppTitle),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              'Sorry',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kColor4,
                    fontSize: 35,
                  ),
            ),
            const Gap(10),
            Text(
              'We are processing your registration. Please wait.',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kColor4,
                    fontSize: 18,
                  ),
            ),
            const Gap(10),
            Image.asset(
              'assets/images/car_loading.gif',
              height: 80,
              width: 80,
            ),
            const Gap(10),
            ElevatedButton(
              onPressed: () {},
              child: const Text('Refresh'),
            ),
            const Gap(20),
            ElevatedButton(
              onPressed: () {},
              style: ElevatedButton.styleFrom(
                foregroundColor: KColors.kDanger,
              ),
              child: const Text('Exit'),
            ),
            const Gap(80),
          ],
        ),
      ),
    );
  }
}
