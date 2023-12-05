class LoginRequestDto {
  phoneNumber: string;
  password: string;
  fromApp: string;

  constructor(phoneNumber: string, password: string, fromApp: string = "ADMIN") {
    this.phoneNumber = phoneNumber;
    this.password = password;
    this.fromApp = fromApp;
  }

  static fromJson(json: { phoneNumber: string; password: string; fromApp?: string }) {
    return new LoginRequestDto(json.phoneNumber, json.password, json.fromApp ?? "ADMIN");
  }

  toJson(): LoginRequestDtoJson {
    return {
      phoneNumber: this.phoneNumber,
      password: this.password,
      fromApp: this.fromApp,
    };
  }
}

interface LoginRequestDtoJson {
  phoneNumber: string;
  password: string;
  fromApp: string;
}

export default LoginRequestDto;
