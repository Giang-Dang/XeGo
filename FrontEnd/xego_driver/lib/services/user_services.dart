import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:geolocator/geolocator.dart';
import 'package:xego_driver/models/Dto/login_request_dto.dart';
import 'package:xego_driver/models/Dto/login_response_dto.dart';
import 'package:xego_driver/models/Dto/registration_request_dto.dart';
import 'package:xego_driver/models/Dto/tokens_dto.dart';
import 'package:xego_driver/models/Dto/user_dto.dart';
import 'package:xego_driver/services/api_services.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class UserServices {
  static bool isAuthorized = false;
  static UserDto? userDto;
  static double currentLongitude = 0.0;
  static double currentLatitude = 0.0;
  final apiServices = ApiServices();

  Future<bool> login(LoginRequestDto requestDto) async {
    const subApiUrl = 'api/auth/user/login';
    final url = Uri.http(KSecret.kApiIp, subApiUrl);

    final jsonData = requestDto.toJson();

    final response = await apiServices.post(url.toString(),
        headers: Constants.kJsonHeader, data: jsonData);

    if (!response.data['isSuccess']) {
      return false;
    }
    LoginResponseDto loginResponseDto =
        LoginResponseDto.fromJson(response.data);

    userDto = loginResponseDto.userDto;
    ApiServices.tokensDto = loginResponseDto.tokensDto;

    await _deleteAllStoredLoginInfo();
    await _saveTokensDto(loginResponseDto.tokensDto);
    await _saveUserDto(loginResponseDto.userDto);

    return true;
  }

  Future<Response> register(RegistrationRequestDto requestDto) async {
    const subApiUrl = 'api/auth/user/register';
    final url = Uri.http(KSecret.kApiIp, subApiUrl);

    final jsonData = requestDto.toJson();

    final response = await apiServices.post(url.toString(),
        headers: Constants.kJsonHeader, data: jsonData);

    if (!response.data['isSuccess']) {
      return response;
    }

    userDto = UserDto(
      userId: response.data['data']['id'],
      userName: response.data['data']['userName'],
      email: response.data['data']['email'],
      phoneNumber: response.data['data']['phoneNumber'],
      firstName: response.data['data']['firstName'],
      lastName: response.data['data']['lastName'],
      address: response.data['data']['address'],
    );

    return response;
  }

  Future<void> getUserLocation() async {
    final locationServices = LocationServices();
    const maxAttempts = 10;
    const desiredAccuracy = 100.0;

    Position locationData = await locationServices.determinePosition();

    for (int attempts = 1; attempts < maxAttempts; attempts++) {
      log(locationData.accuracy.toString());
      if (locationData.accuracy <= desiredAccuracy) {
        break;
      }
      locationData = await locationServices.determinePosition();
    }

    currentLatitude = locationData.latitude;
    currentLongitude = locationData.longitude;
  }

  bool isValidEmail(String? email) {
    RegExp validRegex = RegExp(
        r'^[a-zA-Z0-9.a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z]{2,})+');
    if (email == null) {
      return false;
    }
    return validRegex.hasMatch(email);
  }

  Future<bool> updateUserDtoFromStorage() async {
    final storage = _getSecureStorage();
    final userId = await storage.read(key: Constants.kUserIdKeyName);
    final userName = await storage.read(key: Constants.kUserNameKeyName);
    final email = await storage.read(key: Constants.kEmailKeyName);
    final phoneNumber = await storage.read(key: Constants.kPhoneNumberKeyName);
    final firstName = await storage.read(key: Constants.kFirstNameKeyName);
    final lastName = await storage.read(key: Constants.kLastNameKeyName);
    final address = await storage.read(key: Constants.kAddressKeyName);

    if (userId == null || phoneNumber == null) {
      return false;
    }

    userDto = UserDto(
      userId: userId ?? '',
      userName: userName ?? '',
      email: email ?? '',
      phoneNumber: phoneNumber ?? '',
      firstName: firstName ?? '',
      lastName: lastName ?? '',
      address: address ?? '',
    );

    return true;
  }

  Future<bool> updateTokensDtoFromStorage() async {
    final storage = _getSecureStorage();
    final refreshToken =
        await storage.read(key: Constants.kRefreshTokenKeyName);
    final accessToken = await storage.read(key: Constants.kAccessTokenKeyName);

    if (refreshToken == null || accessToken == null) {
      return false;
    }

    final apiService = ApiServices();
    final tokensDto = TokensDto(
      refreshToken: refreshToken,
      accessToken: accessToken,
    );

    apiService.setTokensDto(tokensDto);

    return true;
  }

  bool isValidUsername(String? username) {
    RegExp validCharacters = RegExp(r'^[a-zA-Z0-9_]+$');
    if (username == null) {
      return false;
    }
    return username.length >= 4 &&
        username.length <= 30 &&
        validCharacters.hasMatch(username);
  }

  bool isValidPhoneNumber(String? phoneNumber) {
    // Regular expression pattern to match valid phone numbers
    String pattern = r'^(0|\+84)(3|5|7|8|9)[0-9]{8}$';
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

  _saveLoginInfo() async {}

  Future<void> _saveTokensDto(
    TokensDto tokens,
  ) async {
    final storage = _getSecureStorage();
    await storage.write(
        key: Constants.kAccessTokenKeyName, value: tokens.accessToken);
    await storage.write(
        key: Constants.kRefreshTokenKeyName, value: tokens.refreshToken);
  }

  Future<void> _saveUserDto(UserDto user) async {
    final storage = _getSecureStorage();
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
