import axios from "axios";
import IVehicle from "../models/interfaces/IVehicle";
import getAppConstants from "../constants/AppConstants";

export default function VehicleService() {
  interface GetAllResponseDto {
    data: IVehicle[];
    isSuccess: boolean;
    message: string;
  }

  async function getAllVehicles(params: {
    id?: number;
    plateNumber?: string;
    type?: string;
    driverId?: string;
    isActive?: boolean;
    createdBy?: string;
    createdStartDate?: Date;
    createdEndDate?: Date;
    lastModifiedBy?: string;
    lastModifiedStartDate?: Date;
    lastModifiedEndDate?: Date;
    searchString?: string;
    pageNumber?: number;
    pageSize?: number;
  }) : Promise<GetAllResponseDto | null> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/vehicles`;
    let responseDto: GetAllResponseDto | null = null;
    try {
      const response = await axios.get<GetAllResponseDto>(url, { params });
      responseDto = response.data;
      console.log(response.data);
    } catch (error) {
      console.error(error);
    }

    return responseDto;
  }

  return {
    getAllVehicles: getAllVehicles,
  };
}