import 'package:flutter/material.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/widgets/input_field.dart';

class PhoneNumberInputField extends StatelessWidget {
  final TextEditingController controller;
  final UserServices _userServices;

  const PhoneNumberInputField({
    super.key,
    required this.controller,
    required UserServices userServices,
  }) : _userServices = userServices;

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.phone,
      label: 'Enter your phone number',
      controller: controller,
      keyboardType: TextInputType.phone,
      validator: (value) {
        if (_userServices.isValidPhoneNumber(value)) {
          return null;
        }
        return 'Please enter a valid phone number.';
      },
    );
  }
}
