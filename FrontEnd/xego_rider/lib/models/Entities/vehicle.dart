class Vehicle {
  final int id;
  final String plateNumber;
  final int typeId;
  final dynamic vehicleType;
  final dynamic driverId;
  final bool isActive;
  final String createdBy;
  final String createdDate;
  final String lastModifiedBy;
  final String lastModifiedDate;

  Vehicle({
    required this.id,
    required this.plateNumber,
    required this.typeId,
    this.vehicleType,
    this.driverId,
    required this.isActive,
    required this.createdBy,
    required this.createdDate,
    required this.lastModifiedBy,
    required this.lastModifiedDate,
  });

  factory Vehicle.fromJson(Map<String, dynamic> json) {
    return Vehicle(
      id: json['id'],
      plateNumber: json['plateNumber'],
      typeId: json['typeId'],
      vehicleType: json['vehicleType'],
      driverId: json['driverId'],
      isActive: json['isActive'],
      createdBy: json['createdBy'],
      createdDate: json['createdDate'],
      lastModifiedBy: json['lastModifiedBy'],
      lastModifiedDate: json['lastModifiedDate'],
    );
  }
}
