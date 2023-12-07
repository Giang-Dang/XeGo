import axios from "axios";
import IVehicle from "../models/interfaces/IVehicle";
import getAppConstants from "../constants/AppConstants";
import IVehicleType from "../models/interfaces/IVehicleType";

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

  async function createVehicle(params: {
    plateNumber: string;
    typeId: number;
    isActive: boolean;
    modifiedBy: string;
  }) : Promise<IVehicle | null> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/vehicles`;

    let res: IVehicle | null = null;

    try {
      const response = await axios.post(url, {
        plateNumber: params.plateNumber,
        typeId: params.typeId,
        isActive: true,
        modifiedBy: params.modifiedBy
      });

      if(response.data.isSuccess) {
        res = response.data.data as IVehicle;
      }
    } catch (error) {
      console.error(error);
    } 
    return res;
  }

  async function editVehicle(params: {
    id: number;
    plateNumber?: string;
    typeId?: number;
    isActive?: boolean;
    isAssigned?: boolean;
    modifiedBy: string;
  }) : Promise<IVehicle | null> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/vehicles`;

    let res: IVehicle | null = null;

    try {
      const response = await axios.post(url, {
        id: params.id,
        plateNumber: params.plateNumber,
        typeId: params.typeId,
        isActive: params.isActive,
        isAssigned: params.isAssigned,
        modifiedBy: params.modifiedBy,
      })

      if (response.data.isSuccess) {
        res = response.data.data as IVehicle;
      }
    } catch (error) {
      console.error(error);
      
    }
    return res;

  }

  async function getAllVehicleType() : Promise<IVehicleType[]> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/vehicles/types`;

    let res: IVehicleType[] = [];
    try {
      const response = await axios.get(url);
      if(response.data.isSuccess) {
        res = response.data.data as IVehicleType[];
      }
    } catch (error) {
      console.error(error);
    }
    return res;
  }

  async function assignVehicle(params: {
    vehicleId: number,
    driverId: string,
    modifiedBy: string,
  }) : Promise<boolean> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/vehicles/assign`;
    try {
      const response = await axios.post(url, {
        vehicleId: params.vehicleId,
        driverId: params.driverId,
        modifiedBy: params.modifiedBy,
      });
      console.log(response.data);
      return response.data.isSuccess;
    } catch (error) {
      console.error(error);
      return false;
    }
  }

  return {
    getAllVehicles: getAllVehicles,
    assignVehicle: assignVehicle,
    createVehicle: createVehicle,
    editVehicle: editVehicle,
    getAllVehicleType: getAllVehicleType,
  };
}