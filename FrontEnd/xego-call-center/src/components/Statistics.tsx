import { ArcElement, Chart as ChartJS, Tooltip, Legend, BarElement } from 'chart.js/auto';
import { useEffect, useState } from 'react';
import { Bar, Doughnut } from 'react-chartjs-2';
import IRide from '../models/interfaces/IRide';
import { getAllPrices } from '../services/PriceServices';
import { getAllRides } from '../services/RideServices';
import IPrice from '../models/interfaces/IPrice';
import UserServices from '../services/UserServices';
import UserDto from '../models/dto/UserDto';
import RideStatusConstants from '../constants/RideStatusConstants';
import IVehicleType from '../models/interfaces/IVehicleType';
import VehicleService from '../services/VehicleServices';
import { Card } from 'antd';


export default function Statistics(): React.ReactElement {

  const [rides, setRides] = useState<IRide[]>([]);
  const [prices, setPrices] = useState<IPrice[]>([]);
  const [users, setUsers] = useState<UserDto[]>([]);
  const [vehicleTypes, setVehicleTypes] = useState<IVehicleType[]>([]);

  ChartJS.register(ArcElement, Tooltip, Legend, BarElement);

  useEffect(() => {
    const fetchAllPrices = async () => {
      const res = await getAllPrices();

      if(res.length > 0) {
        setPrices(() => res);
        console.log('Prices:');
        console.log(prices);
      }
    };

    const fetchAllRides = async () => {
      const res = await getAllRides();
      if(res.length > 0) {
        setRides(() => res);
      }
    };

    const fetchAllUsers = async () => {
      const res = await UserServices().getAllUsers({});
      if(res && res.length > 0) {
        setUsers(() => res);
      }
    };

    const fetchAllVehicleTypes = async () => {
      const res = await VehicleService().getAllVehicleType();
      if(res.length > 0) {
        setVehicleTypes(() => res);
      }
    }

    fetchAllPrices();
    fetchAllRides();
    fetchAllUsers();
    fetchAllVehicleTypes();
  },[]);

    const rideStatusData = {
      labels: ["Ongoing", "Completed", "Cancelled"],
      datasets: [
        {
          label: "Rides",
          data: [
            rides.filter(
              (r) =>
                r.status != RideStatusConstants().cancelled &&
                r.status != RideStatusConstants().completed
            ).length,
            rides.filter((r) => r.status == RideStatusConstants().completed).length,
            rides.filter((r) => r.status == RideStatusConstants().cancelled).length,
          ],
          backgroundColor: [
            "rgb(7, 117, 216, 0.8)",
            "rgba(39, 140, 92, 0.8)",
            "rgb(255, 82, 82, 0.8)",
          ],
        },
      ],
    };

    const ScheduledNormalRideStatusData = {
      labels: ["Normal", "Scheduled"],
      datasets: [
        {
          label: "Rides",
          data: [
            rides.filter((r) => !r.isScheduleRide).length,
            rides.filter((r) => r.isScheduleRide).length,
          ],
          backgroundColor: [
            "rgb(7, 117, 216, 0.8)",
            "rgb(255, 82, 82, 0.8)",
          ],
        },
      ],
    };

    const ongoingRideStatusData = {
      labels: ["Finding Driver", "Driver Accepted", "Awaiting Pickup", "In Progress"],
      datasets: [
        {
          label: "Ongoing Rides",
          data: [
            rides.filter((r) => r.status == RideStatusConstants().findingDriver).length,
            rides.filter((r) => r.status == RideStatusConstants().driverAccepted).length,
            rides.filter((r) => r.status == RideStatusConstants().awaitingPickup).length,
            rides.filter((r) => r.status == RideStatusConstants().inProgress).length,
          ],
          backgroundColor: [
            "rgb(255, 99, 71, 0.8)",
            "rgb(50, 205, 50, 0.8)",
            "rgb(255, 165, 0, 0.8)",
            "rgb(70, 130, 180, 0.8)",
          ],
        },
      ],
    };

    const revenueByMonthData = {
      labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
      datasets: 
        vehicleTypes.map(v => ({
          label: v.name,
          data: [
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 0 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 1 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 2 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 3 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 4 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 5 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 6 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 7 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 8 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 9 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 10 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
            prices
              .filter((p) =>
                rides.some((r) => {
                  const createdDate = new Date(r.createdDate);
                  return (
                    r.id == p.rideId &&
                    r.status == RideStatusConstants().completed &&
                    createdDate.getMonth() == 11 && r.vehicleTypeId == v.id
                  );
                })
              )
              .reduce((total, p) => total + p.totalPrice, 0),
          ],
        })),
    };

    const kmByMonthData = {
      labels: ["1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12"],
      datasets: vehicleTypes.map((v) => ({
        label: v.name,
        data: [
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 0 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 1 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 2 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 3 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 4 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 5 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 6 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 7 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 8 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 9 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 10 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
          prices
            .filter((p) =>
              rides.some((r) => {
                const createdDate = new Date(r.createdDate);
                return (
                  r.id == p.rideId &&
                  r.status == RideStatusConstants().completed &&
                  createdDate.getMonth() == 11 &&
                  r.vehicleTypeId == v.id
                );
              })
            )
            .reduce((total, p) => total + p.distanceInMeters / 1000, 0),
        ],
      })),
    };

  return (
    <>
      <div className="flex justify-between">
        <div className="w-1/3 h-5/12 px-3 flex items-center justify-center">
          <Card title="Ride Statuses:" className="m-3 h-full w-full">
            <Doughnut key={rides.toString()} data={rideStatusData} />
          </Card>
        </div>
        <div className="w-1/3 h-5/12 px-3 flex items-center justify-center">
          <Card title="Normal/Scheduled Ride:" className="m-3 h-full w-full">
            <Doughnut key={rides.toString()} data={ScheduledNormalRideStatusData} />
          </Card>
        </div>
        <div className="w-1/3 h-5/12 px-3 flex items-center justify-center">
          <Card title="Ongoing Ride:" className="m-3 h-full w-full">
            <Doughnut key={rides.toString()} data={ongoingRideStatusData} />
          </Card>
        </div>
      </div>
      <div className="flex justify-between mt-4">
        <div className="w-1/2 h-5/12 px-3 flex items-center justify-center">
          <Card key="Revenue by months:" title="Revenue by months:" className="m-3 h-full w-full">
            <Bar data={revenueByMonthData} />
          </Card>
        </div>
        <div className="w-1/2 h-5/12 px-3 flex items-center justify-center">
          <Card
            key="Total kilometres travelled by months:"
            title="Total kilometres travelled by months:"
            className="m-3 h-full w-full"
          >
            <Bar data={kmByMonthData} />
          </Card>
        </div>
      </div>

      {/* <div className="flex justify-around">
        <div className="flex flex-col justify-between">
          <div className="m-10 w-[400px] h-[450px]">
            <Card title="Ride Statuses:">
              <Doughnut key={rides.toString()} data={rideStatusData} />
            </Card>
          </div>
          <div className="m-10 w-[700px] h-[500px]">
            <Card title="Revenue by months:" className="w-[700px] h-[450px]">
              <Bar data={revenueByMonthData} />
            </Card>
          </div>
        </div>
        <div className="flex flex-col justify-between">
          <div className="m-10 w-[400px] h-[450px]">
            <Card title="Normal/Scheduled Ride:">
              <Doughnut key={rides.toString()} data={ScheduledNormalRideStatusData} />
            </Card>
          </div>
          <div className="">d</div>
        </div>
      </div> */}
    </>
  );
}
