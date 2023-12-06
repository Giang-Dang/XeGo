import IBaseEntity from "./IBaseEntity";

export default interface IVehicleType extends IBaseEntity {
  id: number;
  name: string;
  isActive: boolean;
}
