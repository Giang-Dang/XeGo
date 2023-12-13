import 'dart:developer';

import 'package:firebase_core/firebase_core.dart';
import 'package:firebase_messaging/firebase_messaging.dart';
import 'package:flutter/material.dart';
import 'package:google_api_availability/google_api_availability.dart';
import 'package:xego_driver/firebase_options.dart';
import 'package:xego_driver/screens/login_screen.dart';
import 'package:xego_driver/screens/main_tabs_screen.dart';
import 'package:xego_driver/screens/pick_location_screen.dart';
import 'package:xego_driver/screens/ride_screen.dart';
import 'package:xego_driver/screens/splash_screen.dart';
import 'package:xego_driver/screens/driver_registration_screen.dart';
import 'package:xego_driver/screens/do_not_have_vehicle_screen.dart';
import 'package:xego_driver/services/notification_services.dart';
import 'package:xego_driver/settings/app_constants.dart';
import 'package:xego_driver/settings/kTheme.dart';

void main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await Firebase.initializeApp(
    options: DefaultFirebaseOptions.currentPlatform,
  );

  const GoogleApiAvailability gaa = GoogleApiAvailability.instance;

  final result = await gaa.checkGooglePlayServicesAvailability();

  if (result != GooglePlayServicesAvailability.success) {
    await gaa.makeGooglePlayServicesAvailable();
  } else {
    log('GooglePlayServicesAvailability.success');
  }

  FirebaseMessaging messaging = FirebaseMessaging.instance;
  String? token = await messaging.getToken();

  log(token ?? "");
  NotificationServices.fcmToken = token;

  NotificationSettings settings = await messaging.requestPermission(
    alert: true,
    announcement: false,
    badge: true,
    carPlay: false,
    criticalAlert: false,
    provisional: false,
    sound: true,
  );

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
        // body: const RideScreen(),
      ),
    );
  }
}
