import { Card } from "antd";
import IVehicle from "../models/interfaces/IVehicle";
import { convertUtcToVn } from "../utils/DateUtils";
import { format } from "date-fns";

export default function VehicleInfoBox({ vehicle }: {vehicle: IVehicle}) : React.ReactElement {
  return (
    <>
      <Card title="Vehicle Infomation">
        <p>
          <strong>Plate Number:</strong> {vehicle.plateNumber}
        </p>
        <p>
          <strong>Vehicle Type:</strong> {vehicle.vehicleType.name}
        </p>
        <div className="flex justify-between">
          <div>
            <p>
              <strong>Last Modified By:</strong>
            </p>
            <p>{vehicle.lastModifiedBy}</p>
            <p>
              <strong>Last Modified Time:</strong>{" "}
            </p>
            <p>{format(convertUtcToVn(vehicle.lastModifiedDate), "dd/MM/yyyy HH:mm:ss")}</p>
          </div>
          <div>
            <p>
              <strong>Created By:</strong>
            </p>
            <p>{vehicle.createdBy}</p>
            <p>
              <strong>Created Time:</strong>
            </p>
            <p>{format(convertUtcToVn(vehicle.createdDate), "dd/MM/yyyy HH:mm:ss")}</p>
          </div>
        </div>
      </Card>
    </>
  );
}