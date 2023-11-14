import 'package:flutter/material.dart';
import 'package:xego_driver/screens/splash_screen.dart';
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
      title: 'XeGo - Driver',
      theme: KTheme.kTheme,
      home: const Scaffold(
        body: SplashScreen(),
      ),
    );
  }
}
