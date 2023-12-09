import { useEffect, useState } from "react";
import ILatLng from "../models/interfaces/ILatLng";
import { getDirectionApi } from "../services/GoogleMapServices";
import IDirectionApi from "../models/interfaces/IDirectionApi";
import MapWithDirection from "./MapWithDirection";
import { Card, Form, FormInstance, Select } from "antd";
import VehicleService from "../services/VehicleServices";
import IVehicleType from "../models/interfaces/IVehicleType";

export default function RiderInfoCard({
  pickUpLocation,
  pickUpAddress,
  dropOffLocation,
  dropOffAddress,
  cardWidth,
  cardHeight,
  directionApi,
  setDirectionApi,
  vehicleTypeForm,
  estimatedPrice,
  setEstimatedPrice,
}: {
  pickUpLocation: ILatLng | null;
  pickUpAddress?: string | null | undefined;
  dropOffLocation: ILatLng | null;
  dropOffAddress?: string | null | undefined;
  cardWidth: string;
  cardHeight: string;
  directionApi: IDirectionApi | null;
  setDirectionApi: React.Dispatch<React.SetStateAction<IDirectionApi | null>>;
  vehicleTypeForm: FormInstance;
  estimatedPrice: number | null;
  setEstimatedPrice: React.Dispatch<React.SetStateAction<number | null>>;
}) {
  const [allVehicleTypes, setAllVehicleTypes] = useState<IVehicleType[]>([]);
  const [selectedVehicleTypeId, setSelectedVehicleTypeId] = useState<number | null>(null);
  useEffect(() => {
    const fetchAllVehicles = async () => {
      const response = await VehicleService().getAllVehicleType();

      if (response) {
        setAllVehicleTypes(() => response);
      }
    };

    fetchAllVehicles();
  }, []);

  useEffect(() => {
    if (!pickUpLocation || !dropOffLocation) {
      return;
    }
    const fetchDirectionApi = async () => {
      const response = await getDirectionApi(pickUpLocation, dropOffLocation);
      if (!response) {
        console.error("Cannot get direction api");
        return;
      }
      setDirectionApi(() => response);
    };
    fetchDirectionApi();
  }, [pickUpLocation, dropOffLocation]);

  useEffect(() => {
    const fetchEstPrice = async () => {
      
      if (!directionApi || !selectedVehicleTypeId) {
        return;
      }
      const estPrice = await VehicleService().getEstPriceForVehicleType(
        selectedVehicleTypeId,
        directionApi!.distanceValue
      );

      console.log(estPrice);

      setEstimatedPrice(() => estPrice);
    };
    fetchEstPrice();
  }, [selectedVehicleTypeId, directionApi]);

  const onSelectVehicleTypeChange = (value: number) => {
    setSelectedVehicleTypeId(() => value);
  }

  return (
    <>
      <Card
        className=""
        style={{ width: `${cardWidth}`, height: `${cardHeight}` }}
        title="Ride Information:"
      >
        <Form form={vehicleTypeForm} className="mb-2">
          <Form.Item
            label="Vehicle Type"
            name="vehicleType"
            rules={[{ required: true, message: "Please select a vehicle type." }]}
          >
            <Select
              onChange={onSelectVehicleTypeChange}
              placeholder="Select a type"
              options={allVehicleTypes.map((t) => {
                return {
                  value: t.id,
                  label: t.name,
                };
              })}
            />
          </Form.Item>
        </Form>
        <div className="bg-white p-1 mb-2 rounded-2xl w-[625px]">
          <p>
            <strong>Pick Up Address: </strong>
            {pickUpAddress ?? "N/A"}
          </p>
          <p>
            <strong>Drop Off Address: </strong>
            {dropOffAddress ?? "N/A"}
          </p>
          <div className="flex justify-around">
            <p>
              <strong>Distance: </strong>
              {directionApi?.distanceText ?? "N/A"}
            </p>
            <p>
              <strong>ETA: </strong>
              {directionApi?.etaText ?? "N/A"}
            </p>
            <p>
              <strong>Price: </strong>
              {estimatedPrice ? `$${estimatedPrice}` : "N/A"}
            </p>
          </div>
        </div>
        <MapWithDirection
          pickUpLocation={pickUpLocation}
          dropOffLocation={dropOffLocation}
          polyline={directionApi?.polyline}
          mapHeight="280px"
          mapWidth="625px"
        />
      </Card>
    </>
  );
}