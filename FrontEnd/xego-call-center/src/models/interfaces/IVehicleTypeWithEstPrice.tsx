import IVehicleType from "./IVehicleType";

export default interface IVehicleTypeWithEstPrice extends IVehicleType {
  estimatedPrice: number;
}
