import 'package:xego_rider/models/Entities/vehicle.dart';

class VehicleResponseDto {
  final List<Vehicle> vehicles;
  final bool isSuccess;
  final String message;

  VehicleResponseDto({
    required this.vehicles,
    required this.isSuccess,
    required this.message,
  });

  factory VehicleResponseDto.fromJson(Map<String, dynamic> json) {
    return VehicleResponseDto(
      vehicles: (json['data'] as List)
          .map((vehicleJson) => Vehicle.fromJson(vehicleJson))
          .toList(),
      isSuccess: json['isSuccess'],
      message: json['message'],
    );
  }
}
