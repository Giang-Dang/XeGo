class Vehicle {
  final int id;
  final String plateNumber;
  final int typeId;
  final bool isActive;
  final bool isAssigned;
  final String createdBy;
  final String createdDate;
  final String lastModifiedBy;
  final String lastModifiedDate;

  Vehicle({
    required this.typeId,
    required this.id,
    required this.plateNumber,
    required this.isActive,
    required this.isAssigned,
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
      isActive: json['isActive'],
      isAssigned: json['isAssigned'],
      createdBy: json['createdBy'],
      createdDate: json['createdDate'],
      lastModifiedBy: json['lastModifiedBy'],
      lastModifiedDate: json['lastModifiedDate'],
    );
  }
}
