import IBaseEntity from "./IBaseEntity";

export default interface IDriverLocation extends IBaseEntity {
  userId: string;
  geohash: string;
  latitude: string;
  longitude: string;
}