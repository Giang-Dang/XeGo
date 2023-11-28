import 'package:flutter/material.dart';
import 'package:xego_rider/settings/kColors.dart';

class InfoSectionContainer extends StatelessWidget {
  final String? title;
  final List<Widget> children;
  final double? titleFontSize;
  final FontWeight? titleFontWeight;
  final Color? titleColor;
  final String? titleFontFamily;
  final EdgeInsets? padding;
  final EdgeInsets? innerPadding;
  final BoxBorder? boxBorder;
  final bool haveBoxBorder;

  const InfoSectionContainer(
      {Key? key,
      required this.children,
      this.title,
      this.titleFontSize,
      this.titleFontWeight,
      this.titleColor,
      this.titleFontFamily,
      this.padding,
      this.innerPadding,
      this.boxBorder,
      this.haveBoxBorder = true})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: padding ?? const EdgeInsets.fromLTRB(0, 0, 0, 0),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          if (title != null)
            Text(
              title!,
              textAlign: TextAlign.start,
              style: TextStyle(
                  fontSize: titleFontSize ??
                      Theme.of(context).textTheme.titleLarge!.fontSize,
                  color: titleColor ?? KColors.kLightTextColor,
                  fontFamily: titleFontFamily,
                  fontWeight: titleFontWeight),
            ),
          const SizedBox(
            height: 10,
          ),
          Container(
            decoration: BoxDecoration(
              color: KColors.kOnBackgroundColor,
              borderRadius: BorderRadius.circular(10.0),
              border: haveBoxBorder
                  ? (boxBorder ??
                      Border.all(
                        color: KColors.kPrimaryColor.withOpacity(0.3),
                        width: 1.0,
                      ))
                  : null,
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
