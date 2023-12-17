import IBaseEntity from "./IBaseEntity";

export default interface IPrice extends IBaseEntity {
  rideId: number;
  discountId?: number;
  vehicleTypeId: number;
  distanceInMeters: number;
  totalPrice: number;
}
