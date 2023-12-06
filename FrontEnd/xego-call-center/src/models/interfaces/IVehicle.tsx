import IBaseEntity from "./IBaseEntity";
import IVehicleType from "./IVehicleType";

export default interface IVehicle extends IBaseEntity {
  id: number;
  plateNumber: string;
  typeId: number;
  driverId?: string;
  vehicleType: IVehicleType;
}