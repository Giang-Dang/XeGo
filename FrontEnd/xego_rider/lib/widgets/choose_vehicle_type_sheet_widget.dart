import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:xego_rider/models/Dto/vehicle_type_calculated_price_info_dto.dart';
import 'package:xego_rider/models/Entities/vehicle_type.dart';
import 'package:xego_rider/models/Entities/vehicle_type_price.dart';
import 'package:xego_rider/settings/kColors.dart';
import 'package:xego_rider/widgets/rounded_border_container_widget.dart';

class ChooseVehicleTypeSheetWidget extends StatefulWidget {
  const ChooseVehicleTypeSheetWidget({
    super.key,
    required this.vehicleTypePriceList,
  });

  final List<VehicleTypeCalculatedPriceInfoDto> vehicleTypePriceList;

  @override
  State<ChooseVehicleTypeSheetWidget> createState() =>
      _ChooseVehicleTypeSheetWidgetState();
}

class _ChooseVehicleTypeSheetWidgetState
    extends State<ChooseVehicleTypeSheetWidget> {
  @override
  Widget build(BuildContext context) {
    return DraggableScrollableSheet(
      initialChildSize: 0.1,
      minChildSize: 0.1,
      maxChildSize: 0.5,
      builder: (BuildContext context, ScrollController scrollController) {
        return Container(
          decoration: BoxDecoration(
            color: KColors.kColor5,
            borderRadius: BorderRadius.only(
              topLeft: Radius.circular(24.0),
              topRight: Radius.circular(24.0),
            ),
            border: Border.all(
              color: KColors.kPrimaryColor,
            ),
          ),
          child: ListView.builder(
            controller: scrollController,
            itemCount: widget.vehicleTypePriceList.length + 1,
            padding: EdgeInsets.only(top: 0),
            itemBuilder: (BuildContext context, int index) {
              if (index == 0) {
                return const Center(
                  child: Icon(
                    Icons.drag_handle,
                    color: KColors.kTertiaryColor,
                  ),
                );
              } else {
                return GestureDetector(
                  onTap: () {},
                  child: RoundedBorderContainerWidget(
                    padding: const EdgeInsets.fromLTRB(16, 10, 16, 10),
                    borderColor: KColors.kPrimaryColor,
                    innerBoxColor: KColors.kSecondaryColor,
                    children: [
                      Expanded(
                        flex: 2,
                        child: Text(
                          widget
                              .vehicleTypePriceList[index - 1].vehicleTypeName,
                          style: const TextStyle(
                            color: KColors.kColor6,
                            fontSize: 14,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                      ),
                      Expanded(
                        flex: 1,
                        child: Text(
                          '\$${widget.vehicleTypePriceList[index - 1].calculatedPrice}',
                          style: const TextStyle(
                            color: KColors.kTertiaryColor,
                            fontSize: 14,
                          ),
                        ),
                      ),
                    ],
                  ),
                );
              }
            },
          ),
        );
      },
    );
  }
}
