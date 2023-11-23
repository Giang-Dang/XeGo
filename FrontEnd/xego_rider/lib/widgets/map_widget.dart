import 'dart:async';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/settings/kColors.dart';

class MapWidget extends StatefulWidget {
  final LatLng pickUpLocation;
  final LatLng destinationLocation;
  final List<LatLng> driverLocationsList;

  const MapWidget(
      {super.key,
      required this.pickUpLocation,
      required this.destinationLocation,
      this.driverLocationsList = const []});

  @override
  _MapWidgetState createState() => _MapWidgetState();
}

class _MapWidgetState extends State<MapWidget> {
  final LocationServices _locationServices = LocationServices();
  Set<Polyline> _polylines = {};
  Timer? _initTimer;
  BitmapDescriptor? _pickUpIcon;
  BitmapDescriptor? _destinationIcon;
  BitmapDescriptor? _driverIcon;
  final Completer<GoogleMapController> _controller =
      Completer<GoogleMapController>();

  _initialize() async {
    final polylines = await _locationServices.getDirectionPolylines(
        widget.pickUpLocation, widget.destinationLocation,
        directionColor: KColors.kBlue);
    if (context.mounted) {
      setState(() {
        _polylines = polylines;
      });
    }
    await _loadMarkerIcon();
  }

  Future<void> _loadMarkerIcon() async {
    final pickUpIcon = await BitmapDescriptor.fromAssetImage(
      ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/pick_up_location_icon.png', // Replace with the path to your icon
    );
    final destinationIcon = await BitmapDescriptor.fromAssetImage(
      ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/destination_location_icon.png', // Replace with the path to your icon
    );
    final driverIcon = await BitmapDescriptor.fromAssetImage(
      ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/car_location_icon.png', // Replace with the path to your icon
    );
    if (mounted) {
      setState(() {
        _pickUpIcon = pickUpIcon;
        _destinationIcon = destinationIcon;
        _driverIcon = driverIcon;
      });
    }
  }

  LatLngBounds _boundsFromLatLngList(List<LatLng> list) {
    double? x0;
    double x1 = 0, y0 = 0, y1 = 0;
    for (LatLng latLng in list) {
      if (x0 == null) {
        x0 = x1 = latLng.latitude;
        y0 = y1 = latLng.longitude;
      } else {
        if (latLng.latitude > x1) x1 = latLng.latitude;
        if (latLng.latitude < x0) x0 = latLng.latitude;
        if (latLng.longitude > y1) y1 = latLng.longitude;
        if (latLng.longitude < y0) y0 = latLng.longitude;
      }
    }
    return LatLngBounds(northeast: LatLng(x1, y1), southwest: LatLng(x0!, y0));
  }

  @override
  void initState() {
    super.initState();

    _initTimer = Timer.periodic(const Duration(milliseconds: 100), (timer) {
      _initialize();
      _initTimer?.cancel();
    });
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _initTimer?.cancel();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return GoogleMap(
      initialCameraPosition: CameraPosition(
        target: widget.pickUpLocation,
        zoom: 15,
      ),
      myLocationEnabled: true,
      markers: <Marker>{
        Marker(
          markerId: const MarkerId('pickupLocation'),
          position: widget.pickUpLocation,
          infoWindow: const InfoWindow(title: 'Pick Up Location'),
          icon: _pickUpIcon ?? BitmapDescriptor.defaultMarker,
        ),
        Marker(
          markerId: const MarkerId('destinationLocation'),
          position: widget.destinationLocation,
          infoWindow: const InfoWindow(title: 'Destination Location'),
          icon: _destinationIcon ?? BitmapDescriptor.defaultMarker,
        ),
        for (var i = 0; i < widget.driverLocationsList.length; i++)
          Marker(
            markerId: MarkerId('driverLocation$i'),
            position: widget.driverLocationsList[i],
            infoWindow: const InfoWindow(title: 'Driver Location'),
            icon: _driverIcon ?? BitmapDescriptor.defaultMarker,
          ),
      },
      polylines: _polylines,
      onMapCreated: (GoogleMapController controller) {
        _controller.complete(controller);
        Future.delayed(
            const Duration(milliseconds: 200),
            () => controller.animateCamera(CameraUpdate.newLatLngBounds(
                _boundsFromLatLngList([
                  widget.destinationLocation,
                  widget.pickUpLocation,
                  ...widget.driverLocationsList,
                ]),
                30)));
      },
    );
  }
}
