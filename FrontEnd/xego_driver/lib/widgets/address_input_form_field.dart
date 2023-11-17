import 'package:flutter/material.dart';
import 'package:xego_driver/widgets/input_field.dart';

class AddressInputFormField extends StatelessWidget {
  final TextEditingController controller;

  const AddressInputFormField({
    super.key,
    required this.controller,
  });

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.home,
      label: 'Enter your address',
      controller: controller,
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Please enter your address.';
        }
        return null;
      },
    );
  }
}
