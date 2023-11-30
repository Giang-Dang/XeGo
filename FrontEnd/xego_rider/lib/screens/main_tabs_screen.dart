import 'package:flutter/material.dart';
import 'package:xego_rider/settings/constants.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/home_widget.dart';
import 'package:xego_rider/widgets/history_widget.dart';
import 'package:xego_rider/widgets/me_widget.dart';

class MainTabsScreen extends StatefulWidget {
  const MainTabsScreen({super.key});

  @override
  State<MainTabsScreen> createState() => _MainTabsScreenState();
}

class _MainTabsScreenState extends State<MainTabsScreen> {
  int _selectedPageIndex = 0;
  bool _isFindRideButtonOn = false;

  final List<Widget> _pages = [
    const HomeWidget(),
    const HistoryWidget(),
    const Me(),
  ];

  final List<bool> _isAppBarShow = [true, false, false];

  final List<bool> _isFloatingButtonShow = [false, false, false];
  void _selectPage(int index) async {
    if (mounted) {
      setState(() {
        _selectedPageIndex = index;
      });
    }
  }

  _onFloatingActionButtonPressed() {
    if (mounted) {
      setState(() {
        _isFindRideButtonOn = !_isFindRideButtonOn;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    AppBar? appBar = !_isAppBarShow[_selectedPageIndex]
        ? null
        : AppBar(
            backgroundColor: KColors.kBackgroundColor,
            title: Text(
              Constants.kTopScreenAppTitle,
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
                    _isFindRideButtonOn ? KColors.kAppleGreen : KColors.kDanger,
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
