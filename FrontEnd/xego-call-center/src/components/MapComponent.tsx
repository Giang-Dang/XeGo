import { useLoadScript, GoogleMap, MarkerF, Polyline } from "@react-google-maps/api";
import { GOOGLE_MAP_API_KEY } from "../secret/SecretKeys";
import React, { useCallback, useRef } from "react";
import MarkerIcon from '../assets/images/start_location.png';
import ILatLng from "../models/interfaces/ILatLng";
import { decode } from "@mapbox/polyline";

function MapComponent({
  selectedLocation,
  setSelectedLocation,
  setSelectedBySearchBox,
  isMarkerCentered,
  mapHeight,
  mapWidth,
  polyline,
  markerIconUrl,
}: {
  selectedLocation: ILatLng;
  setSelectedLocation: (location: ILatLng) => void;
  setSelectedBySearchBox: React.Dispatch<React.SetStateAction<boolean>>;
  isMarkerCentered: boolean;
  mapHeight: string;
  mapWidth: string;
  polyline?: string | null | undefined;
  markerIconUrl?: string | null | undefined;
}) {
  const { isLoaded, loadError } = useLoadScript({
    googleMapsApiKey: GOOGLE_MAP_API_KEY,
  });

  const mapRef = useRef<google.maps.Map | null>();
  const onMapLoad = useCallback((map: google.maps.Map) => {
    mapRef.current = map;
  }, []);

  const onMapClick = useCallback((event: google.maps.MapMouseEvent) => {
    if (!event.latLng) {
      return;
    }
    setSelectedBySearchBox(() => false);
    setSelectedLocation({
      lat: event.latLng.lat(),
      lng: event.latLng.lng(),
    });
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
          center={isMarkerCentered ? selectedLocation : undefined}
          zoom={16}
          onLoad={onMapLoad}
          onClick={onMapClick}
        >
          {path && <Polyline path={path} options={{ strokeColor: "#FF0000" }} />}
          <MarkerF
            position={selectedLocation}
            icon={{
              url: markerIconUrl ?? MarkerIcon,
              scaledSize: new window.google.maps.Size(45, 45),
            }}
          />
        </GoogleMap>
      </div>
    </>
  );
}

export default React.memo(MapComponent);