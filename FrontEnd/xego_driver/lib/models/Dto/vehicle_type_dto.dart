class VehicleTypeDto {
  VehicleTypeDto(
      {required this.id, required this.name, required this.isActive});
  final int id;
  final String name;
  final bool isActive;

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'isActive': isActive,
      };

  factory VehicleTypeDto.fromJson(Map<String, dynamic> json) => VehicleTypeDto(
        id: json['id'],
        name: json['name'],
        isActive: json['isActive'],
      );
}
