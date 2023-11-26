import 'package:flutter/material.dart';
import 'package:xego_rider/models/Entities/vehicle_type.dart';
import 'package:xego_rider/models/Entities/vehicle_type_price.dart';

class ChooseVehicleWidget extends StatefulWidget {
  const ChooseVehicleWidget(
      {super.key,
      required this.vehicleTypeList,
      required this.vehicleTypePriceList});

  final List<VehicleType> vehicleTypeList;
  final List<VehicleTypePrice> vehicleTypePriceList;

  @override
  State<ChooseVehicleWidget> createState() => _ChooseVehicleWidgetState();
}

class _ChooseVehicleWidgetState extends State<ChooseVehicleWidget> {
  @override
  Widget build(BuildContext context) {
    return DraggableScrollableSheet(
      initialChildSize: 0.1, // Initial height of the Widget
      minChildSize: 0.1, // Minimum height of the Widget
      maxChildSize: 0.5, // Maximum height of the Widget
      builder: (BuildContext context, ScrollController scrollController) {
        return Container(
          decoration: BoxDecoration(
            color: Colors.blue[100],
            borderRadius: const BorderRadius.only(
              topLeft: Radius.circular(24.0),
              topRight: Radius.circular(24.0),
            ),
          ),
          child: ListView.builder(
            controller: scrollController,
            itemCount: 25,
            itemBuilder: (BuildContext context, int index) {
              return ListTile(title: Text('Item ${index + 1}'));
            },
          ),
        );
      },
    );
  }
}
