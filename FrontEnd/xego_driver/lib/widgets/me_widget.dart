import 'dart:async';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_driver/screens/login_screen.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/image_size_constants.dart';
import 'package:xego_driver/settings/kColors.dart';
import 'package:xego_driver/widgets/info_section_container.dart';

class Me extends StatefulWidget {
  const Me({super.key});

  @override
  State<Me> createState() => _MeState();
}

class _MeState extends State<Me> {
  final _userServices = UserServices();

  String _showingImageUrl = 'assets/images/person_male.png';

  Timer? _initialTimer;

  _initialize() async {
    final avatarImageUrl = await _userServices.getAvatarUrl(
        UserServices.userDto!.userId, ImageSizeConstants.origin);
    if (avatarImageUrl == null) {
      return;
    }
    if (mounted) {
      setState(() {
        _showingImageUrl = avatarImageUrl;
      });
    }
  }

  _logOut() async {
    await _userServices.deleteAllStoredLoginInfo();
    if (mounted) {
      Navigator.of(context).pushAndRemoveUntil(
        MaterialPageRoute(builder: (context) => const LoginScreen()),
        (Route<dynamic> route) => false,
      );
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initialTimer = Timer.periodic(
      const Duration(milliseconds: 100),
      (timer) {
        _initialize();
        _initialTimer?.cancel();
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    final screenHeight = MediaQuery.of(context).size.height;
    final screenWidth = MediaQuery.of(context).size.width;

    final user = UserServices.userDto!;

    ImageProvider showingImageProvider =
        const AssetImage('assets/images/person_male.png');

    if (_showingImageUrl.startsWith('http')) {
      showingImageProvider = NetworkImage(_showingImageUrl);
    } else {
      showingImageProvider = AssetImage(_showingImageUrl);
    }

    return Stack(
      children: [
        Container(
          color: KColors.kPrimaryColor,
        ),
        Positioned(
          bottom: 0,
          left: 0,
          right: 0,
          child: Container(
            height: screenHeight - 280,
            padding: const EdgeInsets.fromLTRB(10, 100, 10, 0),
            color: KColors.kBackgroundColor,
            child: Column(
              children: [
                InfoSectionContainer(
                  title: 'User Informations:',
                  titleColor: KColors.kPrimaryColor,
                  innerPadding: EdgeInsets.fromLTRB(10, 5, 10, 10),
                  children: [
                    Card(
                      child: ListTile(
                        leading: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: 'Name: ',
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                        title: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: '${user.firstName}, ${user.lastName}',
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    Card(
                      child: ListTile(
                        leading: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: 'Tel: ',
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                        title: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: user.phoneNumber,
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    Card(
                      child: ListTile(
                        leading: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: 'Email: ',
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                        title: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: user.email,
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                    Card(
                      child: ListTile(
                        leading: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: 'Address: ',
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kColor6,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                        title: RichText(
                          text: TextSpan(
                            children: [
                              TextSpan(
                                text: user.address,
                                style: Theme.of(context)
                                    .textTheme
                                    .bodyLarge!
                                    .copyWith(
                                      color: KColors.kTertiaryColor,
                                      fontWeight: FontWeight.bold,
                                    ),
                              ),
                            ],
                          ),
                        ),
                      ),
                    ),
                  ],
                ),
                ElevatedButton(
                  onPressed: () {
                    _logOut();
                  },
                  child: Text('Log out'),
                ),
              ],
            ),
          ),
        ),
        Positioned(
          top: 200 - 80,
          left: 0,
          right: 0,
          child: Container(
            width: 180,
            height: 180,
            decoration: BoxDecoration(
              color: KColors.kWhite,
              shape: BoxShape.circle,
              border: Border.all(color: KColors.kTertiaryColor),
              image: DecorationImage(
                  image: showingImageProvider, fit: BoxFit.contain),
            ),
          ),
        )
      ],
    );
  }
}
