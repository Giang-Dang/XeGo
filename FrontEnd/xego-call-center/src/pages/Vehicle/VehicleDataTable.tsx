// import { TableProps } from "antd";
import { useEffect, useState } from "react";
import IVehicle from "../../models/interfaces/IVehicle";
import { DataGrid, GridColDef, GridValueGetterParams } from "@mui/x-data-grid";
import { format } from "date-fns";
import VehicleService from "../../services/VehicleServices";
import { convertUtcToVn } from "../../utils/DateUtils";
import { Button, Modal } from "antd";
import VehicleInfoBox from "../../components/VehicleInfoBox";

const VehicleDataTable: React.FC = () => {
  const [vehicleData, setVehicleData] = useState<IVehicle[]>([]);
  const [rowData, setRowData] = useState<IVehicle | null>(null);
  const [modalVisible, setModalVisible] = useState(false);
  // const [pagination, setPagination] = useState<TableProps<IVehicle>['pagination']>({ current: 1, pageSize: 10});

  const onAssignDriverClick = (vehicle: IVehicle) : void  => {
    setRowData(vehicle);
    setModalVisible(true);
  }

  const handleModalOk = () => {
    setModalVisible(false);
  };

  const handleModalCancel = () => {
    setModalVisible(false);
  };

  const columns: GridColDef[] = [
    {
      field: "plateNumber",
      headerName: "Plate Number",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "vehicleType.name",
      headerName: "Vehicle Type",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      valueGetter: (params: GridValueGetterParams) => params.row.vehicleType.name,
    },
    {
      field: "driverId",
      headerName: "Driver",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      renderCell: (params) =>
        params.row.driverId ? (
          params.row.driverId
        ) : (
          <Button danger onClick={() => onAssignDriverClick(params.row)}>
            Assign Driver
          </Button>
        ),
    },
    {
      field: "lastModifiedBy",
      headerName: "Modified By",
      headerClassName: "bg-gray-300 text-[1.02rem]",
    },
    {
      field: "lastModifiedDate",
      headerName: "Modified Date",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      valueGetter: (params: GridValueGetterParams) =>
        format(convertUtcToVn(params.row.lastModifiedDate), "dd/MM/yyyy HH:mm:ss"),
    },
    {
      field: "createdBy",
      headerName: "Created By",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "createdDate",
      headerName: "Created Date",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      valueGetter: (params: GridValueGetterParams) =>
        format(convertUtcToVn(params.row.createdDate), "dd/MM/yyyy HH:mm:ss"),
    },
  ];

  useEffect(() => {
    const fetchAllVehicles = async () => {
      const response = await VehicleService().getAllVehicles({});
      if(response?.isSuccess) {
        setVehicleData(() => response.data);
      }
    }
    fetchAllVehicles();
  }, []);

  return (
    <>
      <DataGrid
        rows={vehicleData}
        columns={columns}
        initialState={{
          pagination: {
            paginationModel: { page: 0, pageSize: 10 },
          },
        }}
      />
      <Modal
        title="Assign Driver"
        open={modalVisible}
        footer={[
          <Button key="back" onClick={handleModalCancel}>
            Return
          </Button>,
          <Button key="submit" type="primary" onClick={handleModalOk}>
            Submit
          </Button>,
        ]}
      >
        {rowData && 
          <VehicleInfoBox vehicle={rowData}/>
        }
      </Modal>
    </>
  );
}

export default VehicleDataTable;