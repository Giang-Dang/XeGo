import 'package:flutter/material.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/widgets/input_field.dart';

class UsernameInputField extends StatelessWidget {
  final TextEditingController controller;
  final UserServices _userServices;

  const UsernameInputField({
    super.key,
    required this.controller,
    required UserServices userServices,
  }) : _userServices = userServices;

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.person,
      label: 'Enter your username',
      controller: controller,
      validator: (value) {
        if (_userServices.isValidUsername(value)) {
          return null;
        }
        return 'Invalid username. Must be 4-30 characters and only contain letters, numbers, and underscores.';
      },
    );
  }
}
