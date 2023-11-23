import 'package:flutter/material.dart';
import 'package:xego_rider/services/user_services.dart';
import 'package:xego_rider/widgets/input_field.dart';

class EmailInputField extends StatelessWidget {
  final TextEditingController controller;
  final UserServices _userServices;

  const EmailInputField({
    super.key,
    required this.controller,
    required UserServices userServices,
  }) : _userServices = userServices;

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.email,
      label: 'Enter your email',
      controller: controller,
      keyboardType: TextInputType.emailAddress,
      validator: (value) {
        if (_userServices.isValidEmail(value)) {
          return null;
        }
        return 'Please enter a valid email.';
      },
    );
  }
}
