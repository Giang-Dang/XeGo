import { useLoadScript, GoogleMap, MarkerF, Polyline } from "@react-google-maps/api";
import { GOOGLE_MAP_API_KEY } from "../secret/SecretKeys";
import React, { useCallback, useRef } from "react";
import PickUpMarkerIcon from "../assets/images/start_location.png";
import DropOffMarkerIcon from "../assets/images/destination_location_icon.png";
import ILatLng from "../models/interfaces/ILatLng";
import { decode } from "@mapbox/polyline";

function MapWithDirection({
  pickUpLocation,
  dropOffLocation,
  mapHeight,
  mapWidth,
  polyline,
}: {
  pickUpLocation: ILatLng | null;
  dropOffLocation: ILatLng | null;
  mapHeight: string;
  mapWidth: string;
  polyline?: string | null | undefined;
}) {
  const { isLoaded, loadError } = useLoadScript({
    googleMapsApiKey: GOOGLE_MAP_API_KEY,
  });

  const mapRef = useRef<google.maps.Map | null>();
  const onMapLoad = useCallback((map: google.maps.Map) => {
    mapRef.current = map;
  }, []);

  const path = polyline ? decode(polyline).map(([lat, lng]) => ({ lat, lng })) : null;

  if (loadError) return <div>Error loading maps</div>;
  if (!isLoaded) return <div>Loading...</div>;

  return (
    <>
      <div>
        <GoogleMap
          mapContainerStyle={{
            height: `${mapHeight}`,
            width: `${mapWidth}`,
          }}
          center={pickUpLocation ?? undefined}
          zoom={16}
          onLoad={onMapLoad}
        >
          {path && <Polyline path={path} options={{ strokeColor: "#2C6FE7" }} />}
          {pickUpLocation && (
            <MarkerF
              position={pickUpLocation}
              icon={{
                url: PickUpMarkerIcon,
                scaledSize: new window.google.maps.Size(45, 45),
              }}
            />
          )}
          {dropOffLocation && (
            <MarkerF
              position={dropOffLocation}
              icon={{
                url: DropOffMarkerIcon,
                scaledSize: new window.google.maps.Size(45, 45),
              }}
            />
          )}
        </GoogleMap>
      </div>
    </>
  );
}

export default React.memo(MapWithDirection);
