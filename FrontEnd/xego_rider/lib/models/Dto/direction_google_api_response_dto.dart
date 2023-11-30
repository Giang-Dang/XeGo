class DirectionGoogleApiResponseDto {
  DirectionGoogleApiResponseDto(
      {this.distanceValue,
      this.durationValue,
      this.distanceText,
      this.durationText,
      this.encodedPoints = ""});
  int? distanceValue;
  int? durationValue;
  String? distanceText;
  String? durationText;
  String encodedPoints;

  Map<String, dynamic> toJson() {
    return {
      'distanceValue': distanceValue,
      'durationValue': durationValue,
      'distanceText': distanceText,
      'durationText': durationText,
      'encodedPoints': encodedPoints,
    };
  }

  static DirectionGoogleApiResponseDto fromJson(Map<String, dynamic> map) {
    return DirectionGoogleApiResponseDto(
      distanceValue: map['distanceValue'],
      durationValue: map['durationValue'],
      distanceText: map['distanceText'],
      durationText: map['durationText'],
      encodedPoints: map['encodedPoints'],
    );
  }
}
