export default class TokensDto {
  refreshToken: string;
  accessToken: string;

  constructor(refreshToken: string, accessToken: string) {
    this.refreshToken = refreshToken;
    this.accessToken = accessToken;
  }

  static fromJson(json: { refreshToken: string; accessToken: string }) {
    return new TokensDto(json.refreshToken, json.accessToken);
  }
}
