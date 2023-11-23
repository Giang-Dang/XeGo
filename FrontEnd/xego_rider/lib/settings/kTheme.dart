import 'package:flutter/material.dart';
import 'package:google_fonts/google_fonts.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/util/material_color_creator.dart';

class KTheme {
  static final kColorScheme = ColorScheme.fromSwatch(
    primarySwatch: MaterialColorCreator.createMaterialColor(
      KColors.kPrimaryColor,
    ),
  );

  static final kTheme = ThemeData(
    textTheme: GoogleFonts.nunitoTextTheme(),
  ).copyWith(
    useMaterial3: true,
    colorScheme: kColorScheme,
    textTheme: GoogleFonts.nunitoTextTheme().copyWith(
      titleSmall: GoogleFonts.dosis(
        fontWeight: FontWeight.bold,
      ),
      titleMedium: GoogleFonts.dosis(
        fontWeight: FontWeight.bold,
      ),
      titleLarge: GoogleFonts.dosis(
        fontWeight: FontWeight.bold,
      ),
    ),
    cardTheme: const CardTheme().copyWith(
      color: KColors.kOnBackgroundColor,
    ),
  );
}
