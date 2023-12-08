import IBaseEntity from "./IBaseEntity";

export default interface IRide extends IBaseEntity {
  id: number;
  riderId: string;
  driverId?: string | null;
  discountId?: number | null;
  vehicleId?: number | null;
  vehicleTypeId: number;
  status: string;
  startLatitude: number;
  startLongitude: number;
  startAddress: string;
  destinationLatitude: number;
  destinationLongitude: number;
  destinationAddress: string;
  pickupTime: Date;
  isScheduleRide: boolean;
  cancelledBy?: string | null;
  cancellationReason?: string | null;
}
