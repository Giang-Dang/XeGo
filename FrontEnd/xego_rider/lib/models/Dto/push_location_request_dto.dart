class PushLocationRequestDto {
  final String userId;
  final double latitude;
  final double longitude;
  final String createdBy;
  final String modifiedBy;

  PushLocationRequestDto({
    required this.userId,
    required this.latitude,
    required this.longitude,
    required this.createdBy,
    required this.modifiedBy,
  });

  Map<String, dynamic> toJson() => {
        'userId': userId,
        'latitude': latitude,
        'longitude': longitude,
        'createdBy': createdBy,
        'modifiedBy': modifiedBy,
      };

  factory PushLocationRequestDto.fromJson(Map<String, dynamic> json) {
    return PushLocationRequestDto(
      userId: json['userId'],
      latitude: json['latitude'],
      longitude: json['longitude'],
      createdBy: json['createdBy'],
      modifiedBy: json['modifiedBy'],
    );
  }
}
