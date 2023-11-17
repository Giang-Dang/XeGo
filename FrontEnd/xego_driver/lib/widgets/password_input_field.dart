import 'package:flutter/material.dart';
import 'package:xego_driver/widgets/input_field.dart';

class PasswordInputField extends StatefulWidget {
  final TextEditingController controller;

  const PasswordInputField({
    super.key,
    required this.controller,
  });

  @override
  _PasswordInputFieldState createState() => _PasswordInputFieldState();
}

class _PasswordInputFieldState extends State<PasswordInputField> {
  bool _isPasswordObscured = true;

  @override
  Widget build(BuildContext context) {
    return InputField(
      icon: Icons.lock,
      label: 'Enter your password',
      controller: widget.controller,
      obscureText: _isPasswordObscured,
      suffixIcon: IconButton(
        icon: _isPasswordObscured
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
            _isPasswordObscured = !_isPasswordObscured;
          });
        },
      ),
      validator: (value) {
        if (value == null || value.isEmpty) {
          return 'Please enter your password.';
        }
        return null;
      },
    );
  }
}
