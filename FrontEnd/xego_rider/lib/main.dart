import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/screens/splash_screen.dart';
import 'package:xego_rider/settings/constants.dart';
import 'package:xego_rider/settings/kTheme.dart';
import 'package:xego_rider/widgets/choose_location_map_widget.dart';
import 'package:xego_rider/widgets/map_widget.dart';

void main() {
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
        // body: MapWidget(
        //   pickUpLocation: LatLng(10.762622, 106.964172),
        //   destinationLocation: LatLng(10.768511, 106.664817),
        // ),
        body: ChooseLocationMapWidget(
          onLocationSelected: (location) {},
        ),
      ),
    );
  }
}
