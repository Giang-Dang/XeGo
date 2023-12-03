import 'package:flutter/material.dart';
import 'package:xego_driver/screens/login_screen.dart';
import 'package:xego_driver/screens/main_tabs_screen.dart';
import 'package:xego_driver/screens/pick_location_screen.dart';
import 'package:xego_driver/screens/splash_screen.dart';
import 'package:xego_driver/screens/driver_registration_screen.dart';
import 'package:xego_driver/screens/do_not_have_vehicle_screen.dart';
import 'package:xego_driver/settings/app_constants.dart';
import 'package:xego_driver/settings/kTheme.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      title: AppConstants.kTopScreenAppTitle,
      theme: KTheme.kTheme,
      home: Scaffold(
        body: SplashScreen(),
        // body: const MainTabsScreen(),
      ),
    );
  }
}
