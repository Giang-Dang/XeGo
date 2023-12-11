import 'dart:async';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:xego_driver/screens/do_not_have_vehicle_screen.dart';
import 'package:xego_driver/screens/login_screen.dart';
import 'package:xego_driver/screens/main_tabs_screen.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/services/notification_services.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/services/vehicle_services.dart';
import 'package:xego_driver/settings/kcolors.dart';

class SplashScreen extends StatefulWidget {
  const SplashScreen({super.key});

  @override
  State<SplashScreen> createState() => _SplashScreenState();
}

class _SplashScreenState extends State<SplashScreen>
    with SingleTickerProviderStateMixin {
  late AnimationController _textAnimationController;
  late Animation<double> _textAnimation;
  Timer? _loginTimer;
  final _userServices = UserServices();
  final _apiServices = ApiServices();
  final _vehicleServices = VehicleServices();
  final NotificationServices _notificationServices = NotificationServices();
  final _locationServices = LocationServices();

  _initialize() async {
    await _login();
  }

  _login() async {
    await _locationServices.updateCurrentLocation();

    final updateResults = await Future.wait<bool>([
      _userServices.updateUserDtoFromStorage(),
      _userServices.updateTokensDtoFromStorage()
    ]);

    var refreshTokenSuccess = false;
    if (updateResults[0] && updateResults[1]) {
      refreshTokenSuccess = await _apiServices.refreshToken();
    }

    if (!refreshTokenSuccess) {
      if (context.mounted) {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(
            builder: (context) => const LoginScreen(),
          ),
        );
      }
      return;
    }

    log(UserServices.userDto!.userId);
    try {
      _notificationServices.saveFcmTokenToDb(
        UserServices.userDto!.userId,
        NotificationServices.fcmToken!,
      );
    } catch (e) {
      log(e.toString());
    }

    final driverHasVehicle =
        await _vehicleServices.isDriverAssigned(UserServices.userDto!.userId);

    if (!driverHasVehicle) {
      if (context.mounted) {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(
            builder: (context) => const DoNotHaveVehicleScreen(),
          ),
        );
      }
      return;
    }
    if (context.mounted) {
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(
          builder: (context) => const MainTabsScreen(),
        ),
      );
    }
  }

  @override
  void initState() {
    super.initState();
    //animation
    _textAnimationController = AnimationController(
      vsync: this,
      duration: const Duration(seconds: 2),
    )..repeat();
    _textAnimation =
        Tween(begin: 0.0, end: 1.0).animate(_textAnimationController);

    //login
    _loginTimer = Timer.periodic(
      const Duration(milliseconds: 100),
      (timer) {
        _initialize();
        _loginTimer?.cancel();
      },
    );
  }

  @override
  void dispose() {
    _textAnimationController.dispose();
    _loginTimer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          crossAxisAlignment: CrossAxisAlignment.center,
          children: [
            Text(
              'Welcome',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kColor4,
                    fontSize: 35,
                  ),
            ),
            const SizedBox(height: 10),
            Text(
              'We are getting your location. Please wait.',
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kColor4,
                    fontSize: 18,
                  ),
            ),
            const SizedBox(height: 40),
            Image.asset(
              'assets/images/getting_location.gif',
              height: 80,
              width: 80,
            ),
            const SizedBox(height: 15),
            AnimatedBuilder(
              animation: _textAnimation,
              builder: (context, child) {
                if (_textAnimation.value < 0.25) {
                  return SizedBox(
                    width: 90,
                    child: Text(
                      'Loading',
                      textAlign: TextAlign.start,
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 20,
                          ),
                    ),
                  );
                } else if (_textAnimation.value >= 0.25 &&
                    _textAnimation.value < 0.5) {
                  return SizedBox(
                    width: 90,
                    child: Text(
                      'Loading.',
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 20,
                          ),
                    ),
                  );
                } else if (_textAnimation.value >= 0.5 &&
                    _textAnimation.value < 0.75) {
                  return SizedBox(
                    width: 90,
                    child: Text(
                      'Loading..',
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 20,
                          ),
                    ),
                  );
                } else {
                  return SizedBox(
                    width: 90,
                    child: Text(
                      'Loading...',
                      style: Theme.of(context).textTheme.bodyLarge!.copyWith(
                            fontSize: 20,
                          ),
                    ),
                  );
                }
              },
            ),
            const SizedBox(height: 90)
          ],
        ),
      ),
    );
  }
}
