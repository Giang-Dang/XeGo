import 'package:flutter/material.dart';

class RoundedBorderContainerWidget extends StatelessWidget {
  final EdgeInsets padding;
  final Color borderColor;
  final List<Widget> children;
  final bool enableBorder;
  final Color? innerBoxColor;

  const RoundedBorderContainerWidget({
    Key? key,
    required this.padding,
    required this.borderColor,
    required this.children,
    this.enableBorder = true,
    this.innerBoxColor,
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: padding,
      child: Container(
        padding: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 15),
        decoration: BoxDecoration(
          border: enableBorder ? Border.all(color: borderColor) : null,
          borderRadius: BorderRadius.circular(20.0),
          color: innerBoxColor,
        ),
        child: Row(
          children: children,
        ),
      ),
    );
  }
}
