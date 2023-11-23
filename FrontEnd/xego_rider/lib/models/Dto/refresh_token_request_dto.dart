class RefreshTokenRequestDto {
  RefreshTokenRequestDto({
    required this.refreshToken,
    required this.userId,
    required this.fromApp,
  });

  final String refreshToken;
  final String userId;
  final String fromApp;

  factory RefreshTokenRequestDto.fromJson(Map<String, dynamic> json) {
    return RefreshTokenRequestDto(
      refreshToken: json['refreshToken'],
      userId: json['userId'],
      fromApp: json['fromApp'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'refreshToken': refreshToken,
      'userId': userId,
      'fromApp': fromApp,
    };
  }
}
