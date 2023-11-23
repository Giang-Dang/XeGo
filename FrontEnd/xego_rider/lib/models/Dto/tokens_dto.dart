class TokensDto {
  final String refreshToken;
  final String accessToken;

  TokensDto({
    required this.refreshToken,
    required this.accessToken,
  });

  factory TokensDto.fromJson(Map<String, dynamic> json) {
    return TokensDto(
      refreshToken: json['refreshToken'],
      accessToken: json['accessToken'],
    );
  }
}
