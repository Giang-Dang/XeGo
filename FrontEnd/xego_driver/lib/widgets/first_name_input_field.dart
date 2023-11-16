import 'package:flutter/material.dart';
import 'package:xego_driver/widgets/input_field.dart';

class FirstNameInputField extends StatelessWidget {
  final TextEditingController controller;

  const FirstNameInputField({
    super.key,
    required this.controller,
  });

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.person,
      label: 'Enter your first name',
      controller: controller,
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Please enter your first name.';
        }
        return null;
      },
    );
  }
}
