import { useState } from "react";
import MapComponent from "./MapComponent";
import SearchPlaceInput from "./SearchPlaceInput";
import ILatLng from "../models/interfaces/ILatLng";
import AddressBox from "./AddressBox";

export default function MapWithSearchBox({
  selectedLocation,
  setSelectedLocation,
  setSelectedAddress,
  mapHeight,
  mapWidth,
  markerIconUrl,
}: {
  selectedLocation: ILatLng;
  setSelectedLocation: React.Dispatch<React.SetStateAction<ILatLng>>;
  setSelectedAddress: React.Dispatch<React.SetStateAction<string>>;
  mapHeight: string;
  mapWidth: string;
  markerIconUrl?: string | null | undefined;
}) {
  const [selectedBySearchBox, setSelectedBySearchBox] = useState(true);

  return (
    <>
      <div className="bg-white p-0.5 mb-2 rounded-md" style={{ width: `${mapWidth}` }}>
        <SearchPlaceInput
          setSelectedLocation={setSelectedLocation}
          setSelectedBySearchBox={setSelectedBySearchBox}
        />
        <AddressBox latLng={selectedLocation} setReturnedAddress={setSelectedAddress} />
      </div>
      <MapComponent
        selectedLocation={selectedLocation}
        setSelectedLocation={setSelectedLocation}
        setSelectedBySearchBox={setSelectedBySearchBox}
        isMarkerCentered={selectedBySearchBox}
        mapHeight={mapHeight}
        mapWidth={mapWidth}
        markerIconUrl={markerIconUrl}
      />
    </>
  );
}