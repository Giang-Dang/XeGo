class LoginRequestDto {
  const LoginRequestDto(
      {required this.phoneNumber,
      required this.password,
      this.fromApp = "DRIVER"});

  final String phoneNumber;
  final String password;
  final String fromApp;

  factory LoginRequestDto.fromJson(Map<String, dynamic> json) {
    return LoginRequestDto(
      phoneNumber: json['phoneNumber'],
      password: json['password'],
      fromApp: json['fromApp'] ?? "DRIVER",
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
