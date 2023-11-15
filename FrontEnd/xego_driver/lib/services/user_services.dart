import 'dart:convert';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:xego_driver/models/Dto/login_request_dto.dart';
import 'package:xego_driver/models/Dto/login_response_dto.dart';
import 'package:xego_driver/models/Dto/tokens_dto.dart';
import 'package:xego_driver/models/Dto/user_dto.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class UserServices {
  static bool isAuthorized = false;
  static UserDto? userDto;
  static double currentLongitude = 0.0;
  static double currentLatitude = 0.0;
  final apiService = ApiService();

  Future<bool> login(LoginRequestDto requestDto) async {
    const subApiUrl = 'api/auth/user/login';
    final url = Uri.http(KSecret.kApiUrl, subApiUrl);

    final jsonData = requestDto.toJson();

    final response = await apiService.post(url.toString(),
        headers: Constants.kJsonHeader, data: jsonData);

    LoginResponseDto loginResponseDto =
        LoginResponseDto.fromJson(response.data);

    if (!loginResponseDto.isSuccess) {
      return false;
    }

    userDto = loginResponseDto.userDto;
    ApiService.tokensDto = loginResponseDto.tokensDto;

    await _deleteAllStoredLoginInfo();
    await _saveTokensDto(loginResponseDto.tokensDto);
    await _saveUserDto(loginResponseDto.userDto);

    return true;
  }

  bool isValidEmail(String? email) {
    RegExp validRegex = RegExp(
        r'^[a-zA-Z0-9.a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z]{2,})+');
    if (email == null) {
      return false;
    }
    return validRegex.hasMatch(email);
  }

  bool isValidPhoneNumber(String? phoneNumber) {
    // Regular expression pattern to match valid phone numbers
    String pattern =
        r'^(0|\+84)(3[2-9]|5[689]|7[06-9]|8[1-6]|9[0-46-9])[0-9]{7}$|^(0|\+84)(2[0-9]{1}|[3-9]{1})[0-9]{8}$';
    RegExp regExp = RegExp(pattern);

    if (phoneNumber == null) {
      return false;
    }
    // Check if the phone number matches the pattern
    if (regExp.hasMatch(phoneNumber)) {
      return true;
    } else {
      return false;
    }
  }

  bool isValidVietnameseName(String? name) {
    if (name == null) {
      return false;
    }

    final RegExp nameRegExp = RegExp(
      r'^[a-zA-Z\u00C0-\u1EF9]+(?:\s[a-zA-Z\u00C0-\u1EF9]+)*$',
    );
    return nameRegExp.hasMatch(name);
  }

  Future<void> _saveTokensDto(TokensDto tokens) async {
    final storage = _getSecureStorage();
    await storage.write(
        key: Constants.kAccessTokenKeyName, value: tokens.accessToken);
    await storage.write(
        key: Constants.kRefreshTokenKeyName, value: tokens.refreshToken);
  }

  Future<void> _saveUserDto(UserDto user) async {
    final storage = _getSecureStorage();
    await storage.deleteAll(); // Delete all previous keys
    await storage.write(key: Constants.kUserIdKeyName, value: user.userId);
    await storage.write(key: Constants.kUserNameKeyName, value: user.userName);
    await storage.write(key: Constants.kEmailKeyName, value: user.email);
    await storage.write(
        key: Constants.kPhoneNumberKeyName, value: user.phoneNumber);
    await storage.write(
        key: Constants.kFirstNameKeyName, value: user.firstName);
    await storage.write(key: Constants.kLastNameKeyName, value: user.lastName);
    await storage.write(key: Constants.kAddressKeyName, value: user.address);
  }

  Future<void> _deleteAllStoredLoginInfo() async {
    final storage = _getSecureStorage();
    await storage.delete(key: Constants.kAccessTokenKeyName);
    await storage.delete(key: Constants.kRefreshTokenKeyName);
    await storage.delete(key: Constants.kUserIdKeyName);
    await storage.delete(key: Constants.kUserNameKeyName);
    await storage.delete(key: Constants.kEmailKeyName);
    await storage.delete(key: Constants.kPhoneNumberKeyName);
    await storage.delete(key: Constants.kFirstNameKeyName);
    await storage.delete(key: Constants.kLastNameKeyName);
    await storage.delete(key: Constants.kAddressKeyName);
  }

  Future<String?> getAccessToken() async {
    final storage = _getSecureStorage();
    return await storage.read(key: Constants.kAccessTokenKeyName);
  }

  Future<String?> getRefreshToken() async {
    final storage = _getSecureStorage();
    return await storage.read(key: Constants.kRefreshTokenKeyName);
  }

  Future<String?> getStoredUserId() async {
    final storage = _getSecureStorage();
    return await storage.read(key: Constants.kUserIdKeyName);
  }

  FlutterSecureStorage _getSecureStorage() {
    AndroidOptions getAndroidOptions() => const AndroidOptions(
          encryptedSharedPreferences: true,
        );
    final storage = FlutterSecureStorage(aOptions: getAndroidOptions());
    return storage;
  }
}
