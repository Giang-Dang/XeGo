import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:signalr_netcore/hub_connection.dart';
import 'package:signalr_netcore/hub_connection_builder.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/models/Entities/ride.dart';
import 'package:xego_driver/services/location_services.dart';
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
  List<Ride> _receivedRidesList = [];
  List<double> _totalPriceList = [];
  List<DirectionGoogleApiResponseDto> _directionResponseList = [];

  HubConnection? _rideHubConnection;

  final _locationServices = LocationServices();

  final List<bool> _isAppBarShow = [true, false, false];

  final List<bool> _isFloatingButtonShow = [true, false, false];
  void _selectPage(int index) async {
    if (mounted) {
      setState(() {
        _selectedPageIndex = index;
      });
    }
  }

  //Functions
  _updateLocationToDb() async {
    final position = await _locationServices.determinePosition();

    final isUpdateLocationToDbSuccess =
        await _locationServices.updateLocationToDb(
            UserServices.userDto!.userId,
            true,
            LatLng(position.latitude, position.longitude),
            UserServices.userDto!.userName);

    log('isUpdateLocationToDbSuccess: $isUpdateLocationToDbSuccess');
  }

  _deleteLocationInDb() async {
    final isDeleteLocationInDbSuccess = await _locationServices
        .deleteLocationInDb(UserServices.userDto!.userId, true);

    log('isDeleteLocationInDbSuccess: $isDeleteLocationInDbSuccess');
  }

  _onFloatingActionButtonPressed() {
    if (_isFindRideButtonOn) {
      _deleteLocationInDb();
      _disconnectRideHub();
    } else {
      _updateLocationToDb();
      _connectRideHub();
    }
    if (mounted) {
      setState(() {
        _isFindRideButtonOn = !_isFindRideButtonOn;
      });
    }
  }

  _handleReceiveRide(List<dynamic>? jsons) {
    log("receive $jsons");
    if (jsons == null) {
      log("_handleReceiveRide > jsons null!");
    }
    log(jsons![0]);
    final cRide = Ride.fromHubJson(jsonDecode(jsons![0]));
    double cTotalPrice = jsons[1];
    log(jsons[2]);
    final cDirectionResponse =
        DirectionGoogleApiResponseDto.fromHubJson(jsonDecode(jsons[2]));

    if (!_receivedRidesList.contains(cRide)) {
      if (context.mounted) {
        setState(() {
          _receivedRidesList.add(cRide);
          _totalPriceList.add(cTotalPrice);
          _directionResponseList.add(cDirectionResponse);
        });
      }
    }
  }

  _connectRideHub() async {
    const subHubUrl = 'hubs/ride-hub';
    final hubUrl = Uri.http(KSecret.kApiIp, subHubUrl);
    _rideHubConnection =
        HubConnectionBuilder().withUrl(hubUrl.toString()).build();
    _rideHubConnection!.onclose((error) => log("Ride Hub Connection Closed"));
    _rideHubConnection!.on("receiveRide", _handleReceiveRide);
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
        receivedRidesList: _receivedRidesList,
        totalPriceList: _totalPriceList,
        directionResponseList: _directionResponseList,
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
