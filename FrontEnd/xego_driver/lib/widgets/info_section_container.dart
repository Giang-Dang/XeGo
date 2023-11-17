import 'package:flutter/material.dart';
import 'package:xego_driver/settings/kColors.dart';

class InfoSectionContainer extends StatelessWidget {
  final String title;
  final List<Widget> children;
  final double? titleFontSize;
  final Color? titleColor;
  final String? titleFontFamily;
  final EdgeInsets? padding;
  final EdgeInsets? innerPadding;

  const InfoSectionContainer({
    Key? key,
    required this.title,
    required this.children,
    this.titleFontSize,
    this.titleColor,
    this.titleFontFamily,
    this.padding,
    this.innerPadding,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: padding ?? const EdgeInsets.fromLTRB(0, 0, 0, 0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            title,
            textAlign: TextAlign.start,
            style: TextStyle(
              fontSize: titleFontSize ??
                  Theme.of(context).textTheme.titleLarge!.fontSize,
              color: titleColor ?? KColors.kLightTextColor,
              fontFamily: titleFontFamily,
            ),
          ),
          const SizedBox(
            height: 10,
          ),
          Container(
            decoration: BoxDecoration(
              color: KColors.kOnBackgroundColor,
              borderRadius: BorderRadius.circular(10.0),
              border: Border.all(
                color: KColors.kPrimaryColor.withOpacity(0.3),
                width: 1.0,
              ),
            ),
            padding: innerPadding ?? const EdgeInsets.fromLTRB(0, 0, 0, 0),
            child: Column(
              children: children,
            ),
          ),
          const SizedBox(height: 20),
        ],
      ),
    );
  }
}
