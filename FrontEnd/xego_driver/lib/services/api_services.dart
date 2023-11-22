import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:xego_driver/models/Dto/refresh_token_request_dto.dart';
import 'package:xego_driver/models/Dto/tokens_dto.dart';
import 'package:xego_driver/services/user_services.dart';
import 'package:xego_driver/settings/constants.dart';
import 'package:xego_driver/settings/kSecrets.dart';

class ApiServices {
  late Dio dio;
  static TokensDto tokensDto = TokensDto(refreshToken: '', accessToken: '');

  ApiServices() {
    dio = Dio();
    dio.interceptors.add(
      InterceptorsWrapper(
        onRequest: (options, handler) {
          options.headers['Authorization'] = 'Bearer ${tokensDto.accessToken}';
          return handler.next(options);
        },
        onError: (DioException e, handler) async {
          if (e.response?.statusCode == 401) {
            bool isRefreshSuccessful = await refreshToken();
            if (isRefreshSuccessful) {
              e.requestOptions.headers['Authorization'] =
                  'Bearer ${tokensDto.accessToken}';
              return handler.resolve(await dio.fetch(e.requestOptions));
            } else {
              throw Exception('Refresh token failed');
            }
          }
          return handler.next(e);
        },
      ),
    );
  }

  Future<bool> refreshToken() async {
    //implement later
    const subApiUrl = 'api/auth/user/refresh-token';
    final userId = UserServices.userDto?.userId;
    if (userId == null) {
      return false;
    }
    final url = Uri.http(KSecret.kApiIp, subApiUrl).toString();
    final data = RefreshTokenRequestDto(
            refreshToken: tokensDto.refreshToken,
            userId: userId,
            fromApp: Constants.kFromAppValue)
        .toJson();
    final response = await post(url, data: data);

    final newAccessToken = response.data['data'];

    tokensDto = TokensDto(
        refreshToken: tokensDto.refreshToken, accessToken: newAccessToken);

    return true;
  }

  setTokensDto(TokensDto input) {
    tokensDto = input;
  }

  Future<Response> get(String url, {Map<String, dynamic>? headers}) async {
    return await dio.get(
      url,
      options: Options(
        headers: headers,
      ),
    );
  }

  Future<Response> post(String url,
      {required Map<String, dynamic> data,
      Map<String, dynamic>? headers}) async {
    return await dio.post(
      url,
      data: data,
      options: Options(
        headers: headers,
      ),
    );
  }

  Future<Response> put(String url,
      {required Map<String, dynamic> data,
      Map<String, dynamic>? headers}) async {
    return await dio.put(
      url,
      data: data,
      options: Options(
        headers: headers,
      ),
    );
  }

  Future<Response> delete(String url, {Map<String, dynamic>? headers}) async {
    return await dio.delete(
      url,
      options: Options(
        headers: headers,
      ),
    );
  }
}
