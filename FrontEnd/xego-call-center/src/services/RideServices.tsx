import axios from "axios";
import getAppConstants from "../constants/AppConstants";
import IRide from "../models/interfaces/IRide";

export async function createRide(requestDto: {
  riderId: string;
  driverId?: string | null | undefined;
  discountId?: string | null | undefined;
  vehicleId?: string | null | undefined;
  vehicleTypeId: number;
  status?: string | null | undefined;
  startLatitude: number;
  startLongitude: number;
  startAddress: string;
  destinationLatitude: number;
  destinationLongitude: number;
  destinationAddress: string;
  distanceInMeters: number;
  pickupTime: string;
  isScheduleRide: boolean;
  modifiedBy: string;
}): Promise<IRide | null> {
  try {
    const { ApiUrl, JsonHeader } = getAppConstants();
    const url = `http://${ApiUrl}/api/rides`;

    const response = await axios.post(url, requestDto, {
      headers: JsonHeader,
    });

    if (!response.data.isSuccess) {
      console.log(response);
      return null;
    }

    return response.data.data as IRide;
  } catch (error) {
    console.error(error);
    return null;
  }
}