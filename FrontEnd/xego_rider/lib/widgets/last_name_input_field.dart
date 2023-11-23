import 'package:flutter/material.dart';
import 'package:xego_rider/widgets/input_field.dart';

class LastNameInputField extends StatelessWidget {
  final TextEditingController controller;

  const LastNameInputField({
    super.key,
    required this.controller,
  });

  @override
  Widget build(BuildContext context) {
    return InputField(
      label: 'Enter your last name',
      controller: controller,
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Please enter your last name.';
        }
        return null;
      },
    );
  }
}
