import 'package:flutter/material.dart';

class InputField extends StatelessWidget {
  final IconData? icon;
  final String label;
  final TextEditingController controller;
  final String? Function(String?)? validator;
  final TextInputType keyboardType;
  final bool obscureText;
  final Widget? suffixIcon;

  const InputField({
    super.key,
    this.icon,
    required this.label,
    required this.controller,
    this.validator,
    this.keyboardType = TextInputType.text,
    this.obscureText = false,
    this.suffixIcon,
  });

  @override
  Widget build(BuildContext context) {
    return Row(
      mainAxisAlignment: MainAxisAlignment.center,
      crossAxisAlignment: CrossAxisAlignment.center,
      children: [
        Icon(icon, size: 27),
        const SizedBox(width: 15),
        Expanded(
          child: TextFormField(
            decoration: InputDecoration(
              label: Text(label),
              suffixIcon: suffixIcon,
            ),
            controller: controller,
            validator: validator,
            keyboardType: keyboardType,
            obscureText: obscureText,
          ),
        ),
      ],
    );
  }
}
