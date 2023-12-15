import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter_rating_bar/flutter_rating_bar.dart';
import 'package:xego_rider/models/Dto/user_dto.dart';
import 'package:xego_rider/screens/main_tabs_screen.dart';
import 'package:xego_rider/services/user_rating_services.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/settings/role_constants.dart';

class RatingUserScreen extends StatefulWidget {
  const RatingUserScreen({
    Key? key,
    required this.rideId,
    required this.fromUserDto,
    required this.toUserDto,
  }) : super(key: key);

  final int rideId;
  final UserDto fromUserDto;
  final UserDto toUserDto;

  @override
  State<RatingUserScreen> createState() => _RatingUserScreenState();
}

class _RatingUserScreenState extends State<RatingUserScreen> {
  final UserRatingServices _userRatingServices = UserRatingServices();

  Timer? _initTimer;
  double _rating = 0.0;

  _showAlertDialog(String title, String message, void Function() onOkPressed) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: Text(title),
          content: Text(message),
          actions: [
            TextButton(
              child: const Text('OK'),
              onPressed: () {
                onOkPressed();
              },
            ),
          ],
        );
      },
    );
  }

  _onRatePressed() async {
    final response = await _userRatingServices.createRating(
      widget.rideId,
      widget.fromUserDto.userId,
      RoleConstants.rider,
      widget.toUserDto.userId,
      RoleConstants.driver,
      _rating,
      widget.fromUserDto.userId,
    );

    if (response) {
      _showAlertDialog('Rating Successed', 'Thank you for rating', () {
        if (context.mounted) {
          Navigator.of(context).pop();
          Navigator.of(context).pushReplacement(
              MaterialPageRoute(builder: (context) => const MainTabsScreen()));
        }
      });
    } else {
      _showAlertDialog('Rating Failed',
          'Unable to collect your rating at the moment. Please try again later.',
          () {
        if (context.mounted) {
          Navigator.of(context).pop();
        }
      });
    }
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    _initTimer = Timer.periodic(const Duration(milliseconds: 300), (timer) {
      _initTimer?.cancel();
    });
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _initTimer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text(
          'Rating Driver',
          style: TextStyle(color: KColors.kPrimaryColor),
        ),
        centerTitle: true,
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              '${widget.toUserDto.firstName}, ${widget.toUserDto.lastName}',
            ),
            const SizedBox(height: 30.0),
            RatingBar.builder(
              initialRating: _rating,
              minRating: 1,
              direction: Axis.horizontal,
              allowHalfRating: true,
              itemCount: 5,
              itemSize: 55,
              itemPadding: const EdgeInsets.symmetric(horizontal: 4.0),
              itemBuilder: (context, _) => const Icon(
                Icons.star,
                color: Colors.amber,
              ),
              onRatingUpdate: (rating) {
                setState(() {
                  _rating = rating;
                });
              },
            ),
            const SizedBox(height: 40.0),
            Text('Your rating: $_rating'),
            const SizedBox(height: 50.0),
            ElevatedButton(
              onPressed: _onRatePressed,
              child: const Text('Rate'),
            ),
            const SizedBox(height: 50.0),
          ],
        ),
      ),
    );
  }
}
