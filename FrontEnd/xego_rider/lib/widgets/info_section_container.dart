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
  final Color? disableColor;
  final bool haveBoxBorder;
  final bool isDisable;

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
      this.disableColor,
      this.haveBoxBorder = true,
      this.isDisable = false})
      : super(key: key);

  @override
  Widget build(BuildContext context) {
    final cDisableColor = disableColor ?? KColors.kGrey;
    final Color titleColor = isDisable
        ? cDisableColor
        : (this.titleColor ?? KColors.kLightTextColor);
    final Color borderColor = KColors.kPrimaryColor.withOpacity(0.3);
    const Color containerColor = KColors.kOnBackgroundColor;

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
                  color: titleColor,
                  fontFamily: titleFontFamily,
                  fontWeight: titleFontWeight),
            ),
          const SizedBox(
            height: 10,
          ),
          Container(
            decoration: BoxDecoration(
              color: containerColor,
              borderRadius: BorderRadius.circular(10.0),
              border: haveBoxBorder
                  ? (boxBorder ??
                      Border.all(
                        color: borderColor,
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
