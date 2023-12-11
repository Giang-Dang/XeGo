import 'dart:async';
import 'dart:convert';
import 'dart:developer';

import 'package:flutter/material.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_driver/models/Dto/direction_google_api_response_dto.dart';
import 'package:xego_driver/services/location_services.dart';
import 'package:xego_driver/settings/kColors.dart';

class MapWidget extends StatefulWidget {
  final LatLng? pickUpLocation;
  final LatLng? destinationLocation;
  final List<LatLng> driverLocationsList;
  final LatLng? riderLocation;
  final LatLng? myCarLocation;
  final DirectionGoogleApiResponseDto? directionGoogleApiDto;
  final bool? mapZoomControllerEnabled;
  final bool? mapMyLocationEnabled;
  final double? markerOutterPadding;

  const MapWidget({
    super.key,
    this.pickUpLocation,
    this.destinationLocation,
    this.driverLocationsList = const [],
    this.riderLocation,
    this.directionGoogleApiDto,
    this.mapZoomControllerEnabled,
    this.mapMyLocationEnabled,
    this.markerOutterPadding,
    this.myCarLocation,
  });

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
  BitmapDescriptor? _myCarIcon;
  BitmapDescriptor? _riderIcon;
  DirectionGoogleApiResponseDto? _directionResponse;
  GoogleMapController? _googleMapController;
  final Completer<GoogleMapController> _controller =
      Completer<GoogleMapController>();

  _initialize() async {
    if (widget.directionGoogleApiDto == null &&
        (widget.pickUpLocation == null || widget.destinationLocation == null)) {
      return;
    }
    _directionResponse = widget.directionGoogleApiDto ??
        await _locationServices.getPlaceDirectionDetails(
          widget.pickUpLocation!,
          widget.destinationLocation!,
        );

    log("_MapWidgetState > _directionResponse: ${jsonEncode(_directionResponse?.toJson())}");
    if (_directionResponse == null) {
      return;
    }

    final polylines = await _locationServices.getDirectionPolylines(
      _directionResponse!,
      directionColor: KColors.kBlue,
    );
    if (context.mounted) {
      setState(() {
        _polylines = polylines;
      });
    }
  }

  Future<void> _loadMarkerIcon() async {
    final pickUpIcon = await BitmapDescriptor.fromAssetImage(
      const ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/start_location.png', // Replace with the path to your icon
    );
    final destinationIcon = await BitmapDescriptor.fromAssetImage(
      const ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/destination_location_icon.png', // Replace with the path to your icon
    );
    final driverIcon = await BitmapDescriptor.fromAssetImage(
      const ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/car_location_icon.png', // Replace with the path to your icon
    );
    final riderIcon = await BitmapDescriptor.fromAssetImage(
      const ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/user_location.png', // Replace with the path to your icon
    );
    final myCarIcon = await BitmapDescriptor.fromAssetImage(
      const ImageConfiguration(devicePixelRatio: 2.5),
      'assets/images/red_car_top_view.png', // Replace with the path to your icon
    );
    if (mounted) {
      setState(() {
        _pickUpIcon = pickUpIcon;
        _destinationIcon = destinationIcon;
        _driverIcon = driverIcon;
        _riderIcon = riderIcon;
        _myCarIcon = myCarIcon;
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

    _initTimer = Timer.periodic(const Duration(milliseconds: 500), (timer) {
      _initialize();
      _initTimer?.cancel();
    });
  }

  @override
  void dispose() {
    // TODO: implement dispose
    _initTimer?.cancel();
    _googleMapController?.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    log("_MapWidgetState > build > _directionResponse: ${jsonEncode(_directionResponse?.toJson())}");

    return GoogleMap(
      initialCameraPosition: CameraPosition(
        target: widget.riderLocation ??
            widget.pickUpLocation ??
            LocationServices.currentLocation!,
        zoom: 15,
      ),
      myLocationEnabled: widget.mapMyLocationEnabled ?? true,
      zoomControlsEnabled: widget.mapZoomControllerEnabled ?? true,
      markers: <Marker>{
        if (widget.pickUpLocation != null)
          Marker(
            markerId: const MarkerId('pickupLocation'),
            position: widget.pickUpLocation!,
            infoWindow: const InfoWindow(title: 'Pick Up Location'),
            icon: _pickUpIcon ?? BitmapDescriptor.defaultMarker,
          ),
        if (widget.destinationLocation != null)
          Marker(
            markerId: const MarkerId('destinationLocation'),
            position: widget.destinationLocation!,
            infoWindow: const InfoWindow(title: 'Destination Location'),
            icon: _destinationIcon ?? BitmapDescriptor.defaultMarker,
          ),
        if (widget.myCarLocation != null)
          Marker(
            markerId: const MarkerId('myCarLocation'),
            position: widget.myCarLocation!,
            infoWindow: const InfoWindow(title: 'My Location'),
            icon: _myCarIcon ?? BitmapDescriptor.defaultMarker,
          ),
        for (var i = 0; i < widget.driverLocationsList.length; i++)
          Marker(
            markerId: MarkerId('driverLocation$i'),
            position: widget.driverLocationsList[i],
            infoWindow: const InfoWindow(title: 'Driver Location'),
            icon: _driverIcon ?? BitmapDescriptor.defaultMarker,
          ),
        if (widget.riderLocation != null)
          Marker(
            markerId: const MarkerId('riderLocation'),
            position: widget.riderLocation!,
            infoWindow: const InfoWindow(title: 'Rider Location'),
            icon: _riderIcon ?? BitmapDescriptor.defaultMarker,
          ),
      },
      polylines: _polylines,
      onMapCreated: (GoogleMapController controller) async {
        _controller.complete(controller);
        _googleMapController = controller;
        await _loadMarkerIcon();
        if (widget.destinationLocation == null ||
            widget.pickUpLocation == null) {
          return;
        }
        try {
          await Future.delayed(const Duration(milliseconds: 1200));
          _googleMapController!.animateCamera(CameraUpdate.newLatLngBounds(
              _boundsFromLatLngList([
                widget.destinationLocation!,
                widget.pickUpLocation!,
                ...widget.driverLocationsList,
              ]),
              widget.markerOutterPadding ?? 40));
        } catch (e) {
          log(e.toString());
        }
      },
    );
  }
}
