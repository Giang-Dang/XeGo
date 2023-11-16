import 'package:flutter/gestures.dart';
import 'package:flutter/material.dart';
import 'package:xego_driver/settings/kColors.dart';

class NavigationRichText extends StatelessWidget {
  final String navigationText;
  final Widget destinationScreen;

  const NavigationRichText({
    super.key,
    required this.navigationText,
    required this.destinationScreen,
  });

  @override
  Widget build(BuildContext context) {
    return RichText(
      text: TextSpan(
        style: Theme.of(context).textTheme.bodyLarge!.copyWith(
              color: KColors.kLightTextColor,
            ),
        children: [
          const TextSpan(text: 'Click '),
          TextSpan(
            text: ' here ',
            style: Theme.of(context).textTheme.bodySmall!.copyWith(
                  color: Colors.blue[700],
                  fontWeight: FontWeight.bold,
                  fontSize: 14,
                ),
            recognizer: TapGestureRecognizer()
              ..onTap = () {
                Navigator.of(context).pushReplacement(
                  MaterialPageRoute(
                    builder: (context) => destinationScreen,
                  ),
                );
              },
          ),
          TextSpan(text: navigationText),
        ],
      ),
    );
  }
}
