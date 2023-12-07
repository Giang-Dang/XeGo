import UserDto from "./UserDto";
import TokensDto from "./TokensDto";

export default class LoginResponseDto {
  data: {
    user: UserDto | null;
    tokens: TokensDto | null;
  };
  isSuccess: boolean;
  message: string;

  constructor(
    userDto: UserDto | null,
    tokensDto: TokensDto | null,
    isSuccess: boolean,
    message: string
  ) {
    this.data = {user: userDto, tokens: tokensDto};
    this.isSuccess = isSuccess;
    this.message = message;
  }

  static fromJson(json: {
    data: { user: UserDto; tokens: TokensDto };
    isSuccess: boolean;
    message: string;
  }) {
    return new LoginResponseDto(
      UserDto.fromJson(json.data.user),
      TokensDto.fromJson(json.data.tokens),
      json.isSuccess,
      json.message
    );
  }
}
