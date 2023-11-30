import 'dart:developer';

import 'package:firebase_messaging/firebase_messaging.dart';
import 'package:flutter/material.dart';
import 'package:google_api_availability/google_api_availability.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/screens/splash_screen.dart';
import 'package:xego_rider/settings/constants.dart';
import 'package:xego_rider/settings/kTheme.dart';
import 'package:xego_rider/widgets/choose_location_map_widget.dart';
import 'package:xego_rider/widgets/map_widget.dart';

import 'package:firebase_core/firebase_core.dart';
import 'firebase_options.dart';

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
      title: Constants.kTopScreenAppTitle,
      theme: KTheme.kTheme,
      home: Scaffold(
        // body: SplashScreen(),
        body: MapWidget(
          pickUpLocation: LatLng(10.762622, 106.964172),
          destinationLocation: LatLng(10.763511, 106.944817),
          driverLocationsList: [LatLng(10.760622, 106.968172)],
        ),
        // body: ChooseLocationMapWidget(
        //   onLocationSelected: (location) {},
        // ),
      ),
    );
  }
}
