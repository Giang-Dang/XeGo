class VehicleTypeCalculatedPriceInfoDto {
  VehicleTypeCalculatedPriceInfoDto({
    required this.vehicleTypeId,
    required this.vehicleTypeName,
    required this.calculatedPrice,
  });
  final int vehicleTypeId;
  final String vehicleTypeName;
  final double calculatedPrice;
}
