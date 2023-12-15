class Price {
  int rideId;
  int? discountId;
  int vehicleTypeId;
  int distanceInMeters;
  double totalPrice;
  String createdBy;
  String createdDate;
  String lastModifiedBy;
  String lastModifiedDate;

  Price({
    required this.rideId,
    this.discountId,
    required this.vehicleTypeId,
    required this.distanceInMeters,
    required this.totalPrice,
    required this.createdBy,
    required this.createdDate,
    required this.lastModifiedBy,
    required this.lastModifiedDate,
  });

  factory Price.fromJson(Map<String, dynamic> json) {
    return Price(
      rideId: json['rideId'],
      discountId: json['discountId'],
      vehicleTypeId: json['vehicleTypeId'],
      distanceInMeters: json['distanceInMeters'],
      totalPrice: json['totalPrice'],
      createdBy: json['createdBy'],
      createdDate: json['createdDate'],
      lastModifiedBy: json['lastModifiedBy'],
      lastModifiedDate: json['lastModifiedDate'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'rideId': rideId,
      'discountId': discountId,
      'vehicleTypeId': vehicleTypeId,
      'distanceInMeters': distanceInMeters,
      'totalPrice': totalPrice,
      'createdBy': createdBy,
      'createdDate': createdDate,
      'lastModifiedBy': lastModifiedBy,
      'lastModifiedDate': lastModifiedDate,
    };
  }
}
