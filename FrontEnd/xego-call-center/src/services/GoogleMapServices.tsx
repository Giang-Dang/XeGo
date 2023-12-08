import axios from "axios";
import ILatLng from "../models/interfaces/ILatLng";
import { GOOGLE_MAP_API_KEY } from "../secret/SecretKeys";
import IDirectionApi from "../models/interfaces/IDirectionApi";
import getAppConstants from "../constants/AppConstants";

export const getLatLng = async (address: string) : Promise<ILatLng | null> => {
  try {
    const encodedAddress = encodeURIComponent(address);
    const response = await fetch(
      `https://maps.googleapis.com/maps/api/geocode/json?address=${encodedAddress}&key=${GOOGLE_MAP_API_KEY}`
    );
    const data = await response.json();
    if (data.results && data.results.length > 0) {
      const location = data.results[0].geometry.location;
      const res: ILatLng = {
        lat: location.lat,
        lng: location.lng,
      };
      return res;
    } else {
      console.error("No results found");
      return null;
    }
  } catch (error) {
    console.error(error);
    return null;
  }
};

export const getAddress = async (lat: number, lng: number): Promise<string> => {
  try {
    const url = `https://maps.googleapis.com/maps/api/geocode/json?latlng=${lat},${lng}&key=${GOOGLE_MAP_API_KEY}`;

    const response = await axios.get(url);
    return response.data.results[0].formatted_address;
  } catch (error) {
    console.error(error);
    return "Could not load this address!";
  }
};

export const getDirectionApi = async (
  origin: ILatLng,
  destination: ILatLng
): Promise<IDirectionApi | null> => {
  const { ApiUrl } = getAppConstants();
  const url = `http://${ApiUrl}/api/rides/directions?startLat=${origin.lat}&startLng=${origin.lng}&endLat=${destination.lat}&endLng=${destination.lng}`;

  console.log(url);

  const response = await axios.get(url);

  console.log(response);

  if(!response.data.isSuccess) {
    console.log(response);
    return null;
  }

  const data = JSON.parse(response.data.data);

  console.log(data);

  if (data.routes.length > 0) {
    const polyline = data.routes[0].overview_polyline.points;
    const legs = data.routes[0].legs[0];
    const directionData: IDirectionApi = {
      distanceValue: legs.distance.value,
      distanceText: legs.distance.text,
      etaValue: legs.duration.value,
      etaText: legs.duration.text,
      polyline: polyline,
    };
    return directionData;
  } else {
    console.error("No routes found");
    return null;
  }
};