class Ride {
  final int id;
  final String riderId;
  String? driverId;
  int? discountId;
  int? vehicleId;
  final int vehicleTypeId;
  final String status;
  final double startLatitude;
  final double startLongitude;
  final String startAddress;
  final double destinationLatitude;
  final double destinationLongitude;
  final String destinationAddress;
  final DateTime pickupTime;
  final bool isScheduleRide;
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
