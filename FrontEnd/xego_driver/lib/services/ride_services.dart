import 'package:xego_driver/settings/ride_status_constants.dart';

class RideServices {
  String getShowingRideStatus(String rideStatus) {
    switch (rideStatus) {
      case RideStatusConstants.findingDriver:
        return "Finding Driver...";
      case RideStatusConstants.awaitingPickup:
        return "Driver is waiting for you...";
      case RideStatusConstants.driverAccepted:
        return "Driver is heading to the pickup location..";
      case RideStatusConstants.inProgress:
        return "In Progress";
      case RideStatusConstants.cancelled:
        return "Cancelled";
      case RideStatusConstants.completed:
        return "Completed";
      default:
        return rideStatus;
    }
  }
}
