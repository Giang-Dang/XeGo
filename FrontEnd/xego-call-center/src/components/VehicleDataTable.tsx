// import { TableProps } from "antd";
import { useEffect, useState } from "react";
import IVehicle from "../models/interfaces/IVehicle";
import { DataGrid, GridColDef, GridValueGetterParams } from "@mui/x-data-grid";
import { format } from "date-fns";
import VehicleService from "../services/VehicleServices";
import { convertUtcToVn } from "../utils/DateUtils";
import { Button, Form, Input, Modal, Select } from "antd";
import VehicleInfoCard from "./VehicleInfoCard";
import { FindAllUsersByPhoneNumberForm } from "./FindAllUsersByPhoneNumberForm";
import UserDto from "../models/dto/UserDto";
import UserServices from "../services/UserServices";
import DriverInfoCard from "./DriverInfoCard";
import DriverService from "../services/DriverServices";
import IDriver from "../models/interfaces/IDriver";
import IVehicleType from "../models/interfaces/IVehicleType";

const VehicleDataTable: React.FC = () => {
  const [vehicleData, setVehicleData] = useState<IVehicle[]>([]);
  const [rowData, setRowData] = useState<IVehicle | null>(null);
  const [modalAssignDriverVisible, setModalAssignDriverVisible] = useState(false);
  const [selectedDriver, setSelectedDriver] = useState<UserDto>();
  const [isAssigning, setAssigning] = useState<boolean>(false);
  const [modalAssignedVisible, setModalAssignedVisible] = useState(false);
  const [assignedDriver, setAssignedDriver] = useState<IDriver>();
  const [modalAddNewOpens, setModalAddNewOpens] = useState<boolean>();
  const [isSubmittingNewVehicle, setIsSubmittingNewVehicle] = useState<boolean>();
  const [vehicleTypeList, setVehicleTypeList] = useState<IVehicleType[]>([]);

  const [addNewVehicleForm] = Form.useForm(); 
  // const [pagination, setPagination] = useState<TableProps<IVehicle>['pagination']>({ current: 1, pageSize: 10});

  const onRefreshClick = async () => {
    const response = await VehicleService().getAllVehicles({});
    if (response?.isSuccess) {
      setVehicleData(() => response.data);
    }
  };

  const onAddNewVehicleClick = async () => {
    setModalAddNewOpens(() => true);
  }

  const onAddNewSuccess = async (values : {plateNumber: string, typeId: number }) => {
    setIsSubmittingNewVehicle(() => true);

    const createdVehicle = await VehicleService().createVehicle({
      plateNumber: values.plateNumber,
      typeId: values.typeId,
      isActive: true,
      modifiedBy: UserServices().getLoginInfo()!.data.user!.userName
    });

    console.log(createdVehicle);

    if (!createdVehicle) {
      Modal.error({
        title: "Error",
        content: "Something has gone wrong! Cannot add new vehicle!"
      });
      setIsSubmittingNewVehicle(() => false);

      return;
    }

    const response = await VehicleService().getAllVehicles({});
    if (response?.isSuccess) {
      setVehicleData(() => response.data);
    }

    setIsSubmittingNewVehicle(() => false);
    setModalAddNewOpens(() => false);
    addNewVehicleForm.resetFields();
  }

  const onModalAddNewCancel = () => {
    setModalAddNewOpens(() => false);
  }

  const submitAddNewVehicle = () => {
      addNewVehicleForm.submit();
  } 

  const onAssignDriverClick = (vehicle: IVehicle) : void  => {
    setRowData(() => vehicle);
    setModalAssignDriverVisible(() => true);
  }

  const onAssignedClick = async (vehicle: IVehicle): Promise<void> => {
    setRowData(() => vehicle);
    setModalAssignedVisible(() => true);

    const driver = await DriverService().getDriverAssignedToVehicle(vehicle.id);
    
    if(driver == null) {
      return;
    }

    setAssignedDriver(() => driver);
  }

  const handleModalAssignedCancel = () => {
    setModalAssignedVisible(() => false);
  }

  const handleModalAssignDriver = async () => {
    setAssigning(() => true);

    console.log(rowData);
    console.log(selectedDriver);
    console.log(UserServices().getLoginInfo()?.data.user);

    if (!rowData && !selectedDriver && !UserServices().getLoginInfo()?.data.user) {
      setAssigning(() => false);
      return;
    }
    const assignSuccess = await VehicleService().assignVehicle({
      vehicleId: rowData!.id,
      driverId: selectedDriver!.userId,
      modifiedBy: UserServices().getLoginInfo()!.data.user!.userId,
    });
  
    if(!assignSuccess) {
      Modal.error({
        title: 'Error',
        content: 'Something has gone wrong!'
      })
    } else {
      Modal.success({
        title: 'Assigned!',
        content: 'Vehicle has been assigned to the selected Driver.'
      })

      const response = await VehicleService().getAllVehicles({});
      if (response?.isSuccess) {
        setVehicleData(() => response.data);
      }
    }
    setAssigning(() => false);
    
    setModalAssignDriverVisible(false);
  };

  const handleModalAssignDriverCancel = () => {
    setModalAssignDriverVisible(false);
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
      field: "assign",
      headerName: "Assigned?",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      renderCell: (params) =>
        params.row.isAssigned ? (
          // <div className="w-full text-center" style={{ color: "green" }}>
            <Button 
              style={{ borderColor: 'green', color: 'green' }}
              onClick={() => onAssignedClick(params.row)}
            >
              Assigned
            </Button>
          // </div>
        ) : (
          // <div className="w-full flex justify-center">
            <Button danger onClick={() => onAssignDriverClick(params.row)}>
              Assign Driver
            </Button>
          // </div>
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
    const fecthAllVehicleType = async () => {
      const response = await VehicleService().getAllVehicleType();
      setVehicleTypeList(() => response);
    }
    fetchAllVehicles();
    fecthAllVehicleType();
  }, []);



  return (
    <>
      <div className="flex justify-between">
        <h1 className="text-green-700">Vehicles</h1>
        <div>
          <Button style={{ marginRight: "1rem" }} type="primary" onClick={onAddNewVehicleClick}>
            Add New Vehicle
          </Button>
          <Button onClick={onRefreshClick}>Refresh</Button>
        </div>
      </div>
      <div className="h-2/5">
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
          open={modalAssignDriverVisible}
          width={1000}
          onCancel={handleModalAssignDriverCancel}
          footer={[
            <Button key="cancel" onClick={handleModalAssignDriverCancel}>
              Cancel
            </Button>,
            <Button
              key="assign"
              type="primary"
              loading={isAssigning}
              onClick={handleModalAssignDriver}
            >
              Assign
            </Button>,
          ]}
        >
          {rowData && (
            <>
              <div className="flex justify-between">
                <VehicleInfoCard vehicle={rowData} />
                <FindAllUsersByPhoneNumberForm
                  key={rowData.id}
                  setSelectedDriver={setSelectedDriver}
                />
              </div>
            </>
          )}
        </Modal>
        <Modal
          title="Assigned Driver"
          open={modalAssignedVisible}
          width={1000}
          onCancel={handleModalAssignedCancel}
          footer={[
            <Button key="close" onClick={handleModalAssignedCancel}>
              Close
            </Button>,
          ]}
        >
          {rowData && (
            <>
              <div className="flex justify-between">
                <VehicleInfoCard vehicle={rowData} />
                <DriverInfoCard key={rowData.id} driver={assignedDriver} />
              </div>
            </>
          )}
        </Modal>
        <Modal
          title="Add New Vehicle"
          open={modalAddNewOpens}
          width={600}
          onCancel={onModalAddNewCancel}
          footer={[
            <Button key="cancel" onClick={onModalAddNewCancel}>
              Cancel
            </Button>,
            <Button
              key="addNewVehicle"
              type="primary"
              onClick={submitAddNewVehicle}
              loading={isSubmittingNewVehicle}
            >
              Add New Vehicle
            </Button>,
          ]}
        >
          <Form
            form={addNewVehicleForm}
            name="addNewVehicle"
            labelCol={{ span: 8 }}
            wrapperCol={{ span: 16 }}
            style={{ maxWidth: 500 }}
            onFinish={onAddNewSuccess}
            autoComplete="off"
          >
            <Form.Item
              label="Plate Number"
              name="plateNumber"
              rules={[
                { required: true, message: "Please enter the plate number for the new vehicle." },
              ]}
            >
              <Input placeholder="Plate Number" />
            </Form.Item>
            <Form.Item
              label="Type"
              name="typeId"
              rules={[{ required: true, message: "Please select type for the new vehicle." }]}
            >
              <Select
                options={vehicleTypeList.map((t) => {
                  return {
                    value: t.id,
                    label: t.name,
                  };
                })}
              />
            </Form.Item>
          </Form>
        </Modal>
      </div>
    </>
  );
}

export default VehicleDataTable;