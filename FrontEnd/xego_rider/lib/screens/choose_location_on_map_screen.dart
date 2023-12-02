import 'dart:async';

import 'package:flutter/material.dart';
import 'package:gap/gap.dart';
import 'package:geocoding/geocoding.dart';
import 'package:geolocator/geolocator.dart';
import 'package:google_maps_flutter/google_maps_flutter.dart';
import 'package:xego_rider/services/location_services.dart';
import 'package:xego_rider/settings/kColors.dart';

class ChooseLocationOnMapScreen extends StatefulWidget {
  const ChooseLocationOnMapScreen({
    super.key,
    required this.onLocationConfirmed,
    this.initialLatLng,
  });

  final Function(LatLng, String) onLocationConfirmed;
  final LatLng? initialLatLng;

  @override
  State<ChooseLocationOnMapScreen> createState() =>
      _ChooseLocationOnMapScreenState();
}

class _ChooseLocationOnMapScreenState extends State<ChooseLocationOnMapScreen> {
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

  void _confirmDestination() {
    if (_currentCenter != null && _address != '') {
      widget.onLocationConfirmed(_currentCenter!, _address);
    }
    if (context.mounted) {
      Navigator.of(context).pop();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Stack(
        children: <Widget>[
          GoogleMap(
            initialCameraPosition: CameraPosition(
              target: widget.initialLatLng ??
                  LocationServices.currentLocation ??
                  const LatLng(10.762622, 106.964172),
              zoom: 15,
            ),
            minMaxZoomPreference: const MinMaxZoomPreference(9, 50),
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
                  offset: const Offset(0, -20),
                  child: const Icon(Icons.location_on,
                      size: 48.0, color: KColors.kDanger)),
            ),
          Positioned(
            top: 50,
            left: 0,
            right: 0,
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Container(
                decoration: BoxDecoration(
                  color: KColors.kWhite,
                  borderRadius: BorderRadius.circular(8.0),
                  border: Border.all(color: KColors.kColor4, width: 1.0),
                ),
                padding: const EdgeInsets.fromLTRB(10, 0, 10, 0),
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
                      icon: const Icon(Icons.search),
                    ),
                    border: InputBorder.none,
                  ),
                ),
              ),
            ),
          ),
          Positioned(
            bottom: 230,
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
                borderRadius: const BorderRadius.only(
                    topLeft: Radius.circular(8.0),
                    topRight: Radius.circular(8.0)),
                border: Border.all(color: KColors.kColor4, width: 1.0),
              ),
              padding: const EdgeInsets.fromLTRB(20, 20, 20, 30),
              child: Column(
                mainAxisSize: MainAxisSize.min,
                children: [
                  Container(
                    padding: const EdgeInsets.all(10.0),
                    decoration: const BoxDecoration(
                      color: KColors.kLightGrey,
                      borderRadius: BorderRadius.all(Radius.circular(8.0)),
                    ),
                    child: Text(
                      _isLoading ? 'Loading...' : _address,
                      style: const TextStyle(
                          fontSize: 16.0, color: KColors.kTextColor),
                      textAlign: TextAlign.center,
                    ),
                  ),
                  const Gap(20),
                  ElevatedButton(
                    onPressed: _confirmDestination,
                    child: const Text('Confirm Destination'),
                  ),
                ],
              ),
            ),
          ),
        ],
      ),
    );
  }
}
