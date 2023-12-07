import axios from "axios";
import getAppConstants from "../constants/AppConstants";
import IDriver from "../models/interfaces/IDriver";

export default function DriverService() {
  async function getDriverById(driverId: string): Promise<IDriver | null> {
    try {
      const { ApiUrl } = getAppConstants();
      const url = `http://${ApiUrl}/api/drivers?userId=${driverId}`;
      const response = await axios.get(url);
      if (response.data.isSuccess) {
        console.log(response.data.data);
        return response.data.data as IDriver;
      } else {
        console.error(response.data.message);
        return null;
      }
    } catch (error) {
      console.error(error);
      return null;
    }
  }

  async function getDriverAssignedToVehicle(vehicleId: number): Promise<IDriver | null> {
    try{
      const { ApiUrl } = getAppConstants();
      const url = `http://${ApiUrl}/api/vehicles/${vehicleId}/assigned-driver`;

      const response = await axios.get(url);
      if (response.data.isSuccess) {
        console.log(response.data.data);
        return response.data.data as IDriver;
      } else {
        console.error(response.data.message);
        return null;
      }
    } catch (error) {
      console.error(error);
      return null;
    }
  }

  return {
    getDriverById: getDriverById,
    getDriverAssignedToVehicle: getDriverAssignedToVehicle,
  }
}