import { useEffect, useState } from "react";
import ILatLng from "../models/interfaces/ILatLng";
import { getAddress } from "../services/GoogleMapServices";

export default function AddressBox({
  latLng,
  setReturnedAddress,
}: {
  latLng: ILatLng;
  setReturnedAddress: React.Dispatch<React.SetStateAction<string>>;
}) {
  const [address, setAddress] = useState<string>("N/A");

  useEffect(() => {
    const fetchAddress = async () => {
      const res = await getAddress(latLng.lat, latLng.lng);
      setAddress(() => res);
      setReturnedAddress(() => res);
    };
    fetchAddress();
  }, [latLng]);

  return (
    <>
      <div className="m-2">
        <p>
          <strong>Address: </strong>
          {address}
        </p>
      </div>
    </>
  );
}