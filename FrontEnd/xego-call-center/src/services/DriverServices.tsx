import axios from "axios";
import getAppConstants from "../constants/AppConstants";
import IDriver from "../models/interfaces/IDriver";
import IDriverLocation from "../models/interfaces/IDriverLocation";
import IVehicle from "../models/interfaces/IVehicle";

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

  async function getAllDriverLocations(): Promise<IDriverLocation[]> {
    try {
      const { ApiUrl } = getAppConstants();
      const url = `http://${ApiUrl}/api/locations/drivers`;

      const response = await axios.get(url);
      if (response.data.isSuccess) {
        console.log(response.data.data);
        return response.data.data as IDriverLocation[];
      } else {
        console.error(response.data.message);
        return [];
      }
    } catch (error) {
      console.error(error);
      return [];
    }
  }

  async function GetAllDriversByVehicleTypeId(vehicleTypeId: number): Promise<IDriver[]> {
    try {
      const { ApiUrl } = getAppConstants();
      const url = `http://${ApiUrl}/api/drivers/by-vehicle-type/${vehicleTypeId}`;

      const response = await axios.get(url);
      if (response.data.isSuccess) {
        console.log(response.data.data);
        return response.data.data as IDriver[];
      } else {
        console.error(response.data.message);
        return [];
      }
    } catch (error) {
      console.error(error);
      return[];
    }
  }

  async function getAssignedVehicle(driverId: string) : Promise<IVehicle | null> {
    try {
      const { ApiUrl } = getAppConstants();
      const url = `http://${ApiUrl}/api/drivers/${driverId}/assigned-vehicle`;

      const response = await axios.get(url);
      if(response.data.isSuccess) {
        console.log(response.data.data);
        return response.data.data as IVehicle | null;
      } else {
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
    getAllDriverLocations: getAllDriverLocations,
    GetAllDriversByVehicleTypeId: GetAllDriversByVehicleTypeId,
    getAssignedVehicle: getAssignedVehicle,
  };

  
}