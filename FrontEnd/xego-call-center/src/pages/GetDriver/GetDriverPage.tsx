import { useLocation, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import IDriver from "../../models/interfaces/IDriver";
import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { Button, Modal } from "antd";
import DriverService from "../../services/DriverServices";
import IVehicle from "../../models/interfaces/IVehicle";
import { editRide } from "../../services/RideServices";
import { sendFcmNotification, sendScheduledSms, sendSms } from "../../services/NotificationServices";
import UserServices from "../../services/UserServices";
import RideStatusConstants from "../../constants/RideStatusConstants";

export default function GetDriverPage() : React.ReactElement {
  const location = useLocation();
  const navigate = useNavigate();

  const [drivers, setDrivers] = useState<IDriver[]>([]);
  const [currentVehicle, setCurrentVehicle] = useState<IVehicle | null>(null);
  const [openVehicleInfoModal, setOpenVehicleInfoModal] = useState<boolean>(false);
  const pageState = location.state;

  const columns: GridColDef[] = [
    {
      field: "userId",
      headerName: "User Id",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "firstName",
      headerName: "First Name",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "lastName",
      headerName: "Last Name",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "phoneNumber",
      headerName: "Phone Number",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "vehicle",
      headerName: "Vehicle Info",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      renderCell: (params) => (
        <Button onClick={() => onVehicleClick(params.row)}>Info</Button>
      ),
    },
    {
      field: "action",
      headerName: "Action",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      renderCell: (params) => (
        <Button onClick={() => onAssignDriverClick(params.row)}>Assign Driver</Button>
      ),
    },
  ];

  const onAssignDriverClick = async (driver : IDriver) => {
    const user = await UserServices().getLoginInfo()!.data.user;
    const response = await editRide({
      id: pageState.ride.id,
      driverId: driver.userId,
      modifiedBy: user!.userId,
      status: RideStatusConstants().scheduled,
    });

    if(response) {
      const rider = pageState.rider;
      const ride = pageState.ride;
      const rideId = ride.id;
      const formattedDriverPhoneNumber = "+84" + driver.phoneNumber.substring(1);
      const formattedRiderPhoneNumber = "+84" + rider.phoneNumber.substring(1);
      
      const assignedVehicle = await DriverService().getAssignedVehicle(driver.userId);

      if(ride.isScheduleRide) {
        const pickupTime = new Date(ride.pickupTime);
        const sendSmsTime = new Date();
        console.log(pickupTime.getMinutes().toString());
        sendSmsTime.setMinutes(pickupTime.getMinutes() - 60);
        //driver
        sendFcmNotification({
          userId: driver.userId,
          title: `Ride ${rideId} Booked`,
          message: `Ride Id: ${rideId}. Rider Name: ${rider.firstName}, ${
            rider.lastName
          } at ${pickupTime.toLocaleDateString()} ${pickupTime.toLocaleTimeString()}`,
        });
        sendSms({
          phoneNumber: formattedDriverPhoneNumber,
          message: `Ride Id: ${rideId}. Rider Name: ${rider.firstName}, ${
            rider.lastName
          }. Plate Number: ${
            assignedVehicle?.plateNumber ?? "N/A"
          } at ${pickupTime.toLocaleDateString()} ${pickupTime.toLocaleTimeString()}`,
        });
        sendScheduledSms({
          phoneNumber: formattedDriverPhoneNumber,
          message: `Reminder: You have Ride Id: ${rideId}. Rider Name: ${rider.firstName}, ${
            rider.lastName
          } at ${pickupTime.toLocaleDateString()} ${sendSmsTime.toLocaleTimeString()}`,
          sendTime: sendSmsTime.toISOString(),
        });

        //rider
        sendFcmNotification({
          userId: ride.riderId,
          title: `Ride ${rideId} Booked`,
          message: `Ride Id: ${rideId}. Driver Name: ${driver.firstName}, ${
            driver.lastName
          }. Plate Number: ${
            assignedVehicle?.plateNumber ?? "N/A"
          } at ${pickupTime.toLocaleDateString()} ${pickupTime.toLocaleTimeString()}`,
        });
        sendSms({
          phoneNumber: formattedRiderPhoneNumber,
          message: `Ride Id: ${rideId}. Driver Name: ${driver.firstName}, ${
            driver.lastName
          }. Plate Number: ${
            assignedVehicle?.plateNumber ?? "N/A"
          } at ${pickupTime.toLocaleDateString()} ${pickupTime.toLocaleTimeString()}`,
        });
      } else {
        //driver
        sendFcmNotification({
          userId: driver.userId,
          title: `Ride ${rideId} Booked`,
          message: `Ride Id: ${rideId}. Rider Name: ${rider.firstName}, ${rider.lastName}`,
        });
        sendSms({
          phoneNumber: formattedDriverPhoneNumber,
          message: `Ride Id: ${rideId}. Rider Name: ${rider.firstName}, ${rider.lastName}`,
        });

        //rider
        sendFcmNotification({
          userId: ride.riderId,
          title: `Ride ${rideId} Booked`,
          message: `Ride Id: ${rideId}. Driver Name: ${driver.firstName}, ${
            driver.lastName
          }. Plate Number: ${assignedVehicle?.plateNumber ?? "N/A"}`,
        });
        sendSms({
          phoneNumber: formattedRiderPhoneNumber,
          message: `Ride Id: ${rideId}. Driver Name: ${driver.firstName}, ${
            driver.lastName
          }. Plate Number: ${assignedVehicle?.plateNumber ?? "N/A"}`,
        });
      }
      

      Modal.success({
        title: "Driver Assigned",
        content: "Driver has been assigned successfully!",
        afterClose: () => {
          navigate("/order-ride");
        },
      });
    }
  }

  const onVehicleClick = async (driver : IDriver)  => {
    const vehicle = await DriverService().getAssignedVehicle(driver.userId);
    if(vehicle) {
      setCurrentVehicle(() => vehicle);
      setOpenVehicleInfoModal(() => true);
    }
  }

  const handleVehicleInfoModalCancel = () => {
    setOpenVehicleInfoModal(() => false);
  }

  useEffect(() => {
    const fetchAllDriversByVehicleTypeId = async () => {
      const response = await DriverService().GetAllDriversByVehicleTypeId(
        pageState.ride.vehicleTypeId
      );
      if (response.length > 0) {
        setDrivers(() => response);
      }
      console.log(response);
    };

    fetchAllDriversByVehicleTypeId();
  }, [pageState.ride.vehicleTypeId]);

  return (
    <>
      <div className="p-[40px]">
        <div className="">
          <h1 className="text-green-700">Get Driver For Ride {pageState.ride.id}</h1>
        </div>
        <div className="h-full">
          <DataGrid
            rows={drivers}
            columns={columns}
            getRowId={(row) => row.userId}
            initialState={{
              pagination: {
                paginationModel: { page: 0, pageSize: 10 },
              },
            }}
          />
          <Modal
            title="Vehicle Info"
            open={openVehicleInfoModal}
            width={300}
            onCancel={handleVehicleInfoModalCancel}
            footer={[
              <Button key="close" onClick={handleVehicleInfoModalCancel}>
                Close
              </Button>,
            ]}
          >
            {currentVehicle && (
              <>
                <div>
                  <p>
                    <strong>Plate Number: </strong>
                    {currentVehicle.plateNumber}
                  </p>
                </div>
              </>
            )}
          </Modal>
        </div>
      </div>
    </>
  ); 
}