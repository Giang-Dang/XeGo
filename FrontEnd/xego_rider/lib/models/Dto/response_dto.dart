class ResponseDto {
  ResponseDto({
    this.data,
    this.isSuccess = true,
    this.message = "",
  });

  final dynamic data;
  final bool isSuccess;
  final String message;

  factory ResponseDto.fromJson(Map<String, dynamic> json) => ResponseDto(
        data: json['data'],
        isSuccess: json['isSuccess'],
        message: json['message'],
      );

  Map<String, dynamic> toJson() => {
        'data': data,
        'isSuccess': isSuccess,
        'message': message,
      };
}
