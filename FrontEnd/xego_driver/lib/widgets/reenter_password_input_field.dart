import 'package:flutter/material.dart';
import 'package:xego_driver/widgets/input_field.dart';

class ReEnterPasswordInputField extends StatefulWidget {
  final TextEditingController controller;
  final TextEditingController originalPasswordController;

  const ReEnterPasswordInputField({
    super.key,
    required this.controller,
    required this.originalPasswordController,
  });

  @override
  _ReEnterPasswordInputFieldState createState() =>
      _ReEnterPasswordInputFieldState();
}

class _ReEnterPasswordInputFieldState extends State<ReEnterPasswordInputField> {
  bool _isReEnterPasswordObscured = true;

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.lock,
      label: 'Re-type your password',
      controller: widget.controller,
      obscureText: _isReEnterPasswordObscured,
      suffixIcon: IconButton(
        icon: _isReEnterPasswordObscured
            ? const Icon(
                Icons.visibility,
                size: 20,
              )
            : const Icon(
                Icons.visibility_off,
                size: 20,
              ),
        onPressed: () {
          setState(() {
            _isReEnterPasswordObscured = !_isReEnterPasswordObscured;
          });
        },
      ),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Please re-type your password.';
        }
        if (value != widget.originalPasswordController.text) {
          return 'The re-entered password does not match the original password. Please try again.';
        }
        return null;
      },
    );
  }
}
