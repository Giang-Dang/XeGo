class LoginRequestDto {
  phoneNumber: string;
  password: string;
  fromApp: string;

  constructor(
    phoneNumber: string,
    password: string,
    fromApp: string = "RIDER"
  ) {
    this.phoneNumber = phoneNumber;
    this.password = password;
    this.fromApp = fromApp;
  }

  static fromJson(json: {
    phoneNumber: string;
    password: string;
    fromApp?: string;
  }) {
    return new LoginRequestDto(
      json.phoneNumber,
      json.password,
      json.fromApp ?? "RIDER"
    );
  }

  toJson() {
    return {
      phoneNumber: this.phoneNumber,
      password: this.password,
      fromApp: this.fromApp,
    };
  }
}

export default LoginRequestDto;
