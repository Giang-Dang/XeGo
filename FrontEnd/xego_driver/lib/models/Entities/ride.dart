class Ride {
  int id;
  String riderId;
  String? driverId;
  int? discountId;
  int? vehicleId;
  int vehicleTypeId;
  String status;
  double startLatitude;
  double startLongitude;
  String startAddress;
  double destinationLatitude;
  double destinationLongitude;
  String destinationAddress;
  DateTime pickupTime;
  bool isScheduleRide;
  String? cancelledBy;
  String? cancellationReason;

  Ride({
    required this.id,
    required this.riderId,
    this.driverId,
    this.discountId,
    this.vehicleId,
    required this.vehicleTypeId,
    required this.status,
    required this.startLatitude,
    required this.startLongitude,
    required this.startAddress,
    required this.destinationLatitude,
    required this.destinationLongitude,
    required this.destinationAddress,
    required this.pickupTime,
    required this.isScheduleRide,
    this.cancelledBy,
    this.cancellationReason,
  });

  factory Ride.fromJson(Map<String, dynamic> json) {
    return Ride(
      id: json['id'],
      riderId: json['riderId'],
      driverId: json['driverId'],
      discountId: json['discountId'],
      vehicleId: json['vehicleId'],
      vehicleTypeId: json['vehicleTypeId'],
      status: json['status'],
      startLatitude: json['startLatitude'],
      startLongitude: json['startLongitude'],
      startAddress: json['startAddress'],
      destinationLatitude: json['destinationLatitude'],
      destinationLongitude: json['destinationLongitude'],
      destinationAddress: json['destinationAddress'],
      pickupTime: DateTime.parse(json['pickupTime']),
      isScheduleRide: json['isScheduleRide'],
      cancelledBy: json['cancelledBy'],
      cancellationReason: json['cancellationReason'],
    );
  }

  factory Ride.fromHubJson(Map<String, dynamic> json) {
    return Ride(
      id: json['Id'],
      riderId: json['RiderId'],
      driverId: json['DriverId'],
      discountId: json['DiscountId'],
      vehicleId: json['VehicleId'],
      vehicleTypeId: json['VehicleTypeId'],
      status: json['Status'],
      startLatitude: json['StartLatitude'],
      startLongitude: json['StartLongitude'],
      startAddress: json['StartAddress'],
      destinationLatitude: json['DestinationLatitude'],
      destinationLongitude: json['DestinationLongitude'],
      destinationAddress: json['DestinationAddress'],
      pickupTime: DateTime.parse(json['PickupTime']),
      isScheduleRide: json['IsScheduleRide'],
      cancelledBy: json['CancelledBy'],
      cancellationReason: json['CancellationReason'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'riderId': riderId,
      'driverId': driverId,
      'discountId': discountId,
      'vehicleId': vehicleId,
      'vehicleTypeId': vehicleTypeId,
      'status': status,
      'startLatitude': startLatitude,
      'startLongitude': startLongitude,
      'startAddress': startAddress,
      'destinationLatitude': destinationLatitude,
      'destinationLongitude': destinationLongitude,
      'destinationAddress': destinationAddress,
      'pickupTime': pickupTime.toIso8601String(),
      'isScheduleRide': isScheduleRide,
      'cancelledBy': cancelledBy,
      'cancellationReason': cancellationReason,
    };
  }
}
