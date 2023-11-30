import 'package:flutter/material.dart';
import 'package:omni_datetime_picker/omni_datetime_picker.dart';
import 'package:intl/intl.dart';
import 'package:xego_rider/settings/kColors.dart'; // Add this import to format the date and time

class DateTimePickerWidget extends StatefulWidget {
  final bool isDisabled;
  final Function(DateTime) setPickUpTime;

  DateTimePickerWidget({this.isDisabled = false, required this.setPickUpTime});

  @override
  _DateTimePickerWidgetState createState() => _DateTimePickerWidgetState();
}

class _DateTimePickerWidgetState extends State<DateTimePickerWidget> {
  DateTime? _pickedDateTime;
  String? _formattedDateTime;

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.fromLTRB(16, 0, 16, 0),
      child: GestureDetector(
        onTap: widget.isDisabled
            ? () {
                showDialog(
                  context: context,
                  builder: (BuildContext context) {
                    return AlertDialog(
                      title: Text('We\'re sorry!'),
                      content:
                          Text('Only VIP users have access to this function!'),
                      actions: [
                        TextButton(
                          child: Text('OK'),
                          onPressed: () {
                            Navigator.of(context).pop();
                          },
                        ),
                      ],
                    );
                  },
                );
              }
            : () async {
                DateTime? pickedDateTime = await showOmniDateTimePicker(
                  context: context,
                  initialDate: DateTime.now(),
                  firstDate: DateTime(2000),
                  lastDate: DateTime(2099),
                );
                if (pickedDateTime != null) {
                  widget.setPickUpTime(pickedDateTime);
                  setState(() {
                    _pickedDateTime = pickedDateTime;
                    _formattedDateTime =
                        DateFormat('HH:mm dd MMM yyyy').format(pickedDateTime);
                  });
                }
              },
        child: Container(
          padding: const EdgeInsets.symmetric(vertical: 5.0, horizontal: 15),
          decoration: BoxDecoration(
            border: Border.all(
              color: widget.isDisabled ? Colors.grey : KColors.kTertiaryColor,
            ),
            borderRadius: BorderRadius.circular(20.0),
          ),
          child: Row(
            children: [
              Expanded(
                child: Text(
                  _formattedDateTime ?? 'Pick a date and time',
                  style: TextStyle(
                    color: widget.isDisabled
                        ? Colors.grey
                        : KColors.kTertiaryColor,
                    fontSize: 14,
                  ),
                ),
              ),
              const SizedBox(width: 5),
              Icon(
                Icons.calendar_month,
                color: widget.isDisabled ? Colors.grey : KColors.kTertiaryColor,
              ),
            ],
          ),
        ),
      ),
    );
  }
}
