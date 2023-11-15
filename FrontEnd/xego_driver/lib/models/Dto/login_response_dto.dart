import 'package:xego_driver/models/Dto/tokens_dto.dart';
import 'package:xego_driver/models/Dto/user_dto.dart';

class LoginResponseDto {
  final UserDto userDto;
  final TokensDto tokensDto;
  final bool isSuccess;
  final String message;

  LoginResponseDto({
    required this.userDto,
    required this.tokensDto,
    required this.isSuccess,
    required this.message,
  });

  factory LoginResponseDto.fromJson(Map<String, dynamic> json) {
    return LoginResponseDto(
      userDto: UserDto.fromJson(json['data']['user']),
      tokensDto: TokensDto.fromJson(json['data']['tokens']),
      isSuccess: json['isSuccess'],
      message: json['message'],
    );
  }
}
