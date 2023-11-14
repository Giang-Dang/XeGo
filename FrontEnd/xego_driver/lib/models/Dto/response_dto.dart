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
        data: json['Data'],
        isSuccess: json['IsSuccess'],
        message: json['Message'],
      );

  Map<String, dynamic> toJson() => {
        'Data': data,
        'IsSuccess': isSuccess,
        'Message': message,
      };
}
