import UserDto from "./UserDto";
import TokensDto from "./TokensDto";

export default class LoginResponseDto {
  user: UserDto | null;
  tokens: TokensDto | null;
  roles: string[] | null;
  isSuccess: boolean;
  message: string;

  constructor(
    userDto: UserDto | null,
    tokensDto: TokensDto | null,
    roles: string[] | null,
    isSuccess: boolean,
    message: string
  ) {
    this.user = userDto;
    this.tokens = tokensDto;
    this.roles = roles;
    this.isSuccess = isSuccess;
    this.message = message;
  }

  static fromJson(json: {
    data: { user: UserDto; tokens: TokensDto; roles: string[] };
    isSuccess: boolean;
    message: string;
  }) {
    return new LoginResponseDto(
      UserDto.fromJson(json.data.user),
      TokensDto.fromJson(json.data.tokens),
      json.data.roles,
      json.isSuccess,
      json.message
    );
  }
}
