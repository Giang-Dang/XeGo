class LoginRequestDto {
  const LoginRequestDto(
      {required this.phoneNumber,
      required this.password,
      this.fromApp = "RIDER"});

  final String phoneNumber;
  final String password;
  final String fromApp;

  factory LoginRequestDto.fromJson(Map<String, dynamic> json) {
    return LoginRequestDto(
      phoneNumber: json['phoneNumber'],
      password: json['password'],
      fromApp: json['fromApp'] ?? "RIDER",
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'phoneNumber': this.phoneNumber,
      'password': this.password,
      'fromApp': this.fromApp,
    };
  }
}
