import VehicleDataTable from "../../components/VehicleDataTable";

export function VehiclePage(): React.ReactElement {
  return (
    <>
      <div className="min-h-screen bg-white p-6">
        <h1 className="text-green-700">Vehicles</h1>
        <VehicleDataTable />
      </div>
    </>
  );
  
}
