import 'dart:async';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:geocoding/geocoding.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/settings/kColors.dart';

class ChooseLocationMapWidget extends StatefulWidget {
  final Function(LatLng) onLocationSelected;
  final LatLng initialPosition;

  const ChooseLocationMapWidget(
      {required this.onLocationSelected,
      this.initialPosition = const LatLng(10.762622, 106.964172)});

  @override
  _ChooseLocationMapWidgetState createState() =>
      _ChooseLocationMapWidgetState();
}

class _ChooseLocationMapWidgetState extends State<ChooseLocationMapWidget> {
  GoogleMapController? _mapController;
  Completer<GoogleMapController> _controller = Completer();
  bool _isCameraMoving = false;
  String _address = '';
  LatLng? _currentCenter;
  bool _isLoading = false;
  String _searchAddress = '';
  bool _gettingCurrentLocation = false;
  final LocationServices _locationServices = LocationServices();

  void _onMapCreated(GoogleMapController controller) {
    _mapController = controller;
  }

  void _onCameraIdle() async {
    LatLngBounds visibleRegion = await _mapController!.getVisibleRegion();
    LatLng centerLatLng = LatLng(
      (visibleRegion.northeast.latitude + visibleRegion.southwest.latitude) / 2,
      (visibleRegion.northeast.longitude + visibleRegion.southwest.longitude) /
          2,
    );

    String address =
        await _locationServices.getAddressFromCoordinates(centerLatLng);

    setState(() {
      _currentCenter = centerLatLng;
      _isCameraMoving = false;
      _isLoading = false;
      _address = address;
    });
  }

  void _onCameraMove(CameraPosition cameraPosition) {
    setState(() {
      _isCameraMoving = true;
      _address = '';
      _isLoading = true;
    });
  }

  void _search() async {
    try {
      List<Location> locations = await locationFromAddress(_searchAddress);
      if (locations.isNotEmpty) {
        Location location = locations.first;
        _mapController!.animateCamera(
          CameraUpdate.newLatLng(
            LatLng(location.latitude, location.longitude),
          ),
        );
      }
    } catch (e) {
      print(e);
    }
  }

  void _moveToCurrentLocation() async {
    if (_mapController == null) {
      return;
    }

    if (context.mounted) {
      setState(() {
        _gettingCurrentLocation = true;
      });
    }
    try {
      final Position position = await _locationServices.determinePosition();
      _mapController!.animateCamera(
        CameraUpdate.newCameraPosition(
          CameraPosition(
            target: LatLng(position.latitude, position.longitude),
            zoom: 17.0,
          ),
        ),
      );
    } catch (e) {}
    if (context.mounted) {
      setState(() {
        _gettingCurrentLocation = false;
      });
    }
  }

  void _confirmDestination() {}

  @override
  Widget build(BuildContext context) {
    return Stack(
      children: <Widget>[
        GoogleMap(
          initialCameraPosition: CameraPosition(
            target: widget.initialPosition,
            zoom: 10,
          ),
          minMaxZoomPreference: MinMaxZoomPreference(9, 50),
          zoomControlsEnabled: false,
          onMapCreated: _onMapCreated,
          onCameraIdle: _onCameraIdle,
          onCameraMove: _onCameraMove,
          markers: <Marker>{
            if (_currentCenter != null && !_isCameraMoving)
              Marker(
                markerId: const MarkerId('pickupLocation'),
                position: _currentCenter!,
                infoWindow: const InfoWindow(title: 'Pick Up Location'),
                icon: BitmapDescriptor.defaultMarker,
              ),
          },
        ),
        if (_isCameraMoving)
          Center(
            child: Transform.translate(
                offset: Offset(0, -20),
                child: Icon(Icons.location_on,
                    size: 48.0, color: KColors.kDanger)),
          ),
        Positioned(
          top: 50,
          left: 0,
          right: 0,
          child: Padding(
            padding: EdgeInsets.all(16.0),
            child: Container(
              decoration: BoxDecoration(
                color: KColors.kWhite,
                borderRadius: BorderRadius.circular(8.0),
                border: Border.all(color: KColors.kColor4, width: 1.0),
              ),
              padding: EdgeInsets.fromLTRB(10, 0, 10, 0),
              child: TextField(
                onChanged: (value) {
                  _searchAddress = value;
                },
                onSubmitted: (value) {
                  _search();
                },
                decoration: InputDecoration(
                  hintText: 'Enter address',
                  suffixIcon: IconButton(
                    onPressed: _search,
                    icon: Icon(Icons.search),
                  ),
                  border: InputBorder.none,
                ),
              ),
            ),
          ),
        ),
        Positioned(
          bottom: 200,
          left: 0,
          right: 10,
          child: Container(
            alignment: Alignment.bottomRight,
            child: FloatingActionButton(
              onPressed: _moveToCurrentLocation,
              backgroundColor: KColors.kSecondaryColor,
              child: _gettingCurrentLocation
                  ? const CircularProgressIndicator(
                      color: KColors.kTertiaryColor,
                    )
                  : const Icon(Icons.my_location),
            ),
          ),
        ),
        Positioned(
          bottom: 0,
          left: 0,
          right: 0,
          child: Container(
            decoration: BoxDecoration(
              color: KColors.kWhite,
              borderRadius: BorderRadius.only(
                  topLeft: Radius.circular(8.0),
                  topRight: Radius.circular(8.0)),
              border: Border.all(color: KColors.kColor4, width: 1.0),
            ),
            padding: EdgeInsets.fromLTRB(20, 20, 20, 30),
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                Container(
                  padding: EdgeInsets.all(10.0),
                  decoration: BoxDecoration(
                    color: KColors.kLightGrey,
                    borderRadius: BorderRadius.all(Radius.circular(8.0)),
                  ),
                  child: Text(
                    _isLoading ? 'Loading...' : _address,
                    style: TextStyle(fontSize: 16.0, color: KColors.kTextColor),
                    textAlign: TextAlign.center,
                  ),
                ),
                Gap(20),
                ElevatedButton(
                  onPressed: _confirmDestination,
                  child: Text('Confirm Destination'),
                ),
              ],
            ),
          ),
        ),
      ],
    );
  }
}
