import 'dart:developer';

import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:geolocator/geolocator.dart';
import 'package:xego_rider/models/Dto/login_request_dto.dart';
import 'package:xego_rider/models/Dto/login_response_dto.dart';
import 'package:xego_rider/models/Dto/registration_request_dto.dart';
import 'package:xego_rider/models/Dto/tokens_dto.dart';
import 'package:xego_rider/models/Dto/user_dto.dart';
import 'package:xego_rider/services/api_services.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/settings/app_constants.dart';
import 'package:xego_rider/settings/kSecrets.dart';

class UserServices {
  static bool isAuthorized = false;
  static UserDto? userDto;
  static String? riderType;
  static double currentLongitude = 0.0;
  static double currentLatitude = 0.0;
  final apiServices = ApiServices();

  Future<bool> login(LoginRequestDto requestDto) async {
    const subApiUrl = 'api/auth/user/login';
    final url = Uri.http(KSecret.kApiIp, subApiUrl);

    final jsonData = requestDto.toJson();

    final response = await apiServices.post(url.toString(),
        headers: AppConstants.kJsonHeader, data: jsonData);

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
        headers: AppConstants.kJsonHeader, data: jsonData);

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

  Future<bool> updateRiderType(String userId) async {
    const subApiUrl = 'api/auth/user/rider-type';
    final url = Uri.http(KSecret.kApiIp, subApiUrl, {"id": userId});

    final response = await apiServices.get(url.toString());

    log(response.data.toString());

    if (response.data['isSuccess']) {
      riderType = response.data['data'];
      log(riderType ?? "null");
      return true;
    }

    return false;
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
    final userId = await storage.read(key: AppConstants.kUserIdKeyName);
    final userName = await storage.read(key: AppConstants.kUserNameKeyName);
    final email = await storage.read(key: AppConstants.kEmailKeyName);
    final phoneNumber =
        await storage.read(key: AppConstants.kPhoneNumberKeyName);
    final firstName = await storage.read(key: AppConstants.kFirstNameKeyName);
    final lastName = await storage.read(key: AppConstants.kLastNameKeyName);
    final address = await storage.read(key: AppConstants.kAddressKeyName);

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
        await storage.read(key: AppConstants.kRefreshTokenKeyName);
    final accessToken =
        await storage.read(key: AppConstants.kAccessTokenKeyName);

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
        key: AppConstants.kAccessTokenKeyName, value: tokens.accessToken);
    await storage.write(
        key: AppConstants.kRefreshTokenKeyName, value: tokens.refreshToken);
  }

  Future<void> _saveUserDto(UserDto user) async {
    final storage = _getSecureStorage();
    await storage.write(key: AppConstants.kUserIdKeyName, value: user.userId);
    await storage.write(
        key: AppConstants.kUserNameKeyName, value: user.userName);
    await storage.write(key: AppConstants.kEmailKeyName, value: user.email);
    await storage.write(
        key: AppConstants.kPhoneNumberKeyName, value: user.phoneNumber);
    await storage.write(
        key: AppConstants.kFirstNameKeyName, value: user.firstName);
    await storage.write(
        key: AppConstants.kLastNameKeyName, value: user.lastName);
    await storage.write(key: AppConstants.kAddressKeyName, value: user.address);
  }

  Future<void> _deleteAllStoredLoginInfo() async {
    final storage = _getSecureStorage();
    await storage.delete(key: AppConstants.kAccessTokenKeyName);
    await storage.delete(key: AppConstants.kRefreshTokenKeyName);
    await storage.delete(key: AppConstants.kUserIdKeyName);
    await storage.delete(key: AppConstants.kUserNameKeyName);
    await storage.delete(key: AppConstants.kEmailKeyName);
    await storage.delete(key: AppConstants.kPhoneNumberKeyName);
    await storage.delete(key: AppConstants.kFirstNameKeyName);
    await storage.delete(key: AppConstants.kLastNameKeyName);
    await storage.delete(key: AppConstants.kAddressKeyName);
  }

  Future<String?> getAccessToken() async {
    final storage = _getSecureStorage();
    return await storage.read(key: AppConstants.kAccessTokenKeyName);
  }

  Future<String?> getRefreshToken() async {
    final storage = _getSecureStorage();
    return await storage.read(key: AppConstants.kRefreshTokenKeyName);
  }

  Future<String?> getStoredUserId() async {
    final storage = _getSecureStorage();
    return await storage.read(key: AppConstants.kUserIdKeyName);
  }

  FlutterSecureStorage _getSecureStorage() {
    AndroidOptions getAndroidOptions() => const AndroidOptions(
          encryptedSharedPreferences: true,
        );
    final storage = FlutterSecureStorage(aOptions: getAndroidOptions());
    return storage;
  }
}
