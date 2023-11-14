import 'dart:convert';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:xego_driver/models/Dto/login_request_dto.dart';
import 'package:xego_driver/models/Dto/response_dto.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kSecrets.dart';

import 'package:http/http.dart' as http;

class UserServices {
  static bool isAuthorized = false;
  static String accessToken = "";
  static String userId = "";
  static double currentLongitude = 0.0;
  static double currentLatitude = 0.0;

  Future<LoginResponseDto> login(LoginRequestDto requestDto) async {
    const subApiUrl = 'api/auth/register';
    final url = Uri.http(KSecret.kApiUrl, subApiUrl);

    final jsonData = json.encode(requestDto);

    final response =
        await http.post(url, headers: Constants.kJsonHeader, body: jsonData);

    final responseJson = json.decode(response.body);

    final responseDto = ResponseDto.fromJson(responseJson);

    if (!responseDto.isSuccess) {}
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

  Future<void> saveLoginInfo(
      String accessToken, String refreshToken, String userId) async {
    final storage = _getSecureStorage();
    await storage.delete(key: Constants.kAccessTokenKeyName);
    await storage.delete(key: Constants.kRefreshTokenKeyName);
    await storage.delete(key: Constants.kUserIdKeyName);
    await storage.write(key: Constants.kAccessTokenKeyName, value: accessToken);
    await storage.write(
        key: Constants.kRefreshTokenKeyName, value: refreshToken);
    await storage.write(key: Constants.kUserIdKeyName, value: userId);
  }

  Future<void> deleteStoredLoginInfo() async {
    final storage = _getSecureStorage();
    await storage.delete(key: Constants.kAccessTokenKeyName);
    await storage.delete(key: Constants.kRefreshTokenKeyName);
    await storage.delete(key: Constants.kUserIdKeyName);
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
