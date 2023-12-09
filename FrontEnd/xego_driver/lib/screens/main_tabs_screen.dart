import 'dart:async';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/app_constants.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/settings/kSecrets.dart';
import 'package:xego_driver/widgets/find_ride_widget.dart';
import 'package:xego_driver/widgets/history_widget.dart';
import 'package:xego_driver/widgets/me_widget.dart';

class MainTabsScreen extends StatefulWidget {
  const MainTabsScreen({super.key});

  @override
  State<MainTabsScreen> createState() => _MainTabsScreenState();
}

class _MainTabsScreenState extends State<MainTabsScreen> {
  int _selectedPageIndex = 0;
  bool _isFindRideButtonOn = false;
  HubConnection? _rideHubConnection;

  final List<bool> _isAppBarShow = [true, false, false];

  final List<bool> _isFloatingButtonShow = [true, false, false];
  void _selectPage(int index) async {
    if (mounted) {
      setState(() {
        _selectedPageIndex = index;
      });
    }
  }

  _onFloatingActionButtonPressed() {
    if (_isFindRideButtonOn) {
      _disconnectRideHub();
    } else {
      _connectRideHub();
    }
    if (mounted) {
      setState(() {
        _isFindRideButtonOn = !_isFindRideButtonOn;
      });
    }
  }

  _connectRideHub() async {
    const subHubUrl = 'hubs/ride-hub';
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);
    _rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    _rideHubConnection!.onclose((error) => log("Ride Hub Connection Closed"));

    try {
      await _rideHubConnection!.start();
      log('Ride Hub Connection started');

      var registerConnectionId = await _rideHubConnection!.invoke(
        'RegisterConnectionId',
        args: [UserServices.userDto!.userId],
      );
      log(registerConnectionId.toString());
    } catch (e) {
      log('Ride Hub Connection failed: $e');
    }
  }

  _disconnectRideHub() {
    _rideHubConnection?.stop();
  }

  @override
  Widget build(BuildContext context) {
    final List<Widget> _pages = [
      FindRideWidget(
        isFindingRide: _isFindRideButtonOn,
      ),
      const HistoryWidget(),
      const Me(),
    ];

    AppBar? appBar = !_isAppBarShow[_selectedPageIndex]
        ? null
        : AppBar(
            backgroundColor: KColors.kBackgroundColor,
            title: Text(
              AppConstants.kTopScreenAppTitle,
              style: Theme.of(context).textTheme.titleLarge!.copyWith(
                    color: KColors.kPrimaryColor,
                    fontSize: 24,
                  ),
            ),
          );

    return Scaffold(
      appBar: appBar,
      body: _pages[_selectedPageIndex],
      floatingActionButton: _isFloatingButtonShow[_selectedPageIndex]
          ? SizedBox(
              width: 50,
              height: 50,
              child: FloatingActionButton(
                onPressed: _onFloatingActionButtonPressed,
                elevation: 10.0,
                shape: const CircleBorder(),
                backgroundColor:
                    _isFindRideButtonOn ? KColors.kAppleGreen : KColors.kGrey,
                foregroundColor: KColors.kWhite,
                child: const Icon(Icons.power_settings_new),
              ),
            )
          : null,
      bottomNavigationBar: BottomNavigationBar(
        unselectedItemColor: KColors.kLightTextColor,
        unselectedFontSize: 10,
        selectedItemColor: KColors.kPrimaryColor,
        selectedFontSize: 12,
        showUnselectedLabels: true,
        currentIndex: _selectedPageIndex,
        onTap: (index) {
          _selectPage(index);
        },
        items: const [
          BottomNavigationBarItem(
            icon: Icon(
              Icons.drive_eta_outlined,
              color: KColors.kLightTextColor,
            ),
            label: 'Find Ride',
            activeIcon: Icon(
              Icons.drive_eta,
              color: KColors.kPrimaryColor,
            ),
            backgroundColor: KColors.kOnBackgroundColor,
          ),
          BottomNavigationBarItem(
            icon: Icon(
              Icons.history_outlined,
              color: KColors.kLightTextColor,
            ),
            label: 'History',
            activeIcon: Icon(
              Icons.history,
              color: KColors.kPrimaryColor,
            ),
            backgroundColor: KColors.kOnBackgroundColor,
          ),
          BottomNavigationBarItem(
            icon: Icon(
              Icons.person_outline,
              color: KColors.kLightTextColor,
            ),
            label: 'Me',
            activeIcon: Icon(
              Icons.person,
              color: KColors.kPrimaryColor,
            ),
            backgroundColor: KColors.kOnBackgroundColor,
          ),
        ],
      ),
    );
  }
}
