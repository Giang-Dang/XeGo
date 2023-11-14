class LoginRequestDto {
  const LoginRequestDto(
      {required this.userName,
      required this.password,
      this.fromApp = "DRIVER"});

  final String userName;
  final String password;
  final String fromApp;
}
