import { useEffect, useState } from "react";
import MapWithSearchBox from "../../components/MapWithSearchBox";
import ILatLng from "../../models/interfaces/ILatLng";
import { Button, Card, DatePicker, Form, Modal, Switch } from "antd";
import UserDto from "../../models/dto/UserDto";
import RiderInfoCard from "../../components/RiderInfoCard";
import IDirectionApi from "../../models/interfaces/IDirectionApi";
import RideInfoCard from "../../components/RideInfoCard";
import DropOffMarkerIcon from "../../assets/images/destination_location_icon.png";
import UserServices from "../../services/UserServices";
import { createRide } from "../../services/RideServices";
import { useNavigate } from "react-router-dom";
import dayjs from "dayjs";
import { sendFcmNotification, sendSms } from "../../services/NotificationServices";


export function OrderRidePage(): React.ReactElement {
  const navigate = useNavigate();
  const [selectedPickUpLocation, setSelectedPickUpLocation] = useState<ILatLng>({
    lat: 10.7628356,
    lng: 106.6824824,
  });
  const [selectedDropOffLocation, setSelectedDropOffLocation] = useState<ILatLng>({
    lat: 10.7628356,
    lng: 106.6824824,
  });
  const [selectedPickUpAddress, setSelectedPickUpAddress] = useState<string>("N/A");
  const [selectedDropOffAddress, setSelectedDropOffAddress] = useState<string>("N/A");

  const [rider, setRider] = useState<UserDto>();

  const [directionApi, setDirectionApi] = useState<IDirectionApi | null>(null);

  const [estimatedPrice, setEstimatedPrice] = useState<number | null>(null);

  const [isSubmitting, setIsSubmitting] = useState<boolean>();

  const [isVip, setIsVip] = useState<boolean>(false);
  const [scheduleAvailable, setScheduleAvailable] = useState<boolean>(false);
  const [selectedScheduleDateIsoString, setSelectedScheduleDateIsoString] = useState<string>(dayjs(new Date()).toISOString());
  const [riderInfoForm] = Form.useForm();
  const [vehicleTypeForm] = Form.useForm();
  const [findUserByPhoneForm] = Form.useForm();

  useEffect(() => {
    if (rider) {
      riderInfoForm.setFieldsValue({
        firstName: rider.firstName,
        lastName: rider.lastName,
        userName: rider.userName,
      });
    }
  }, [rider, riderInfoForm]);

  const onSubmitClick = async () => {
    setIsSubmitting(() => true);

    try {
      if(!directionApi) {
        return;
      }

      const vehicleTypeFormValues = await vehicleTypeForm.validateFields();
      const findUserByPhoneFormValues = await findUserByPhoneForm.validateFields();
      const riderInfoFormValues = await riderInfoForm.validateFields();

      console.log(riderInfoFormValues);
      console.log(vehicleTypeFormValues);
      console.log(findUserByPhoneFormValues);

      let riderId: string= rider?.userId ?? "riderId null";
      
      if(!rider) {
        const registerResponse = await UserServices().registerNewRider({
          userName: findUserByPhoneFormValues.phoneNumber as string,
          phoneNumber: findUserByPhoneFormValues.phoneNumber as string,
          firstName: riderInfoFormValues.firstName as string,
          lastName: riderInfoFormValues.lastName as string,
          address: selectedPickUpAddress,
        });

        console.log(registerResponse?.userId ?? "registerResponse?.userId: null");
        console.log(registerResponse ?? "registerResponse: null");

        if (!registerResponse) {
          throw new Error("Register failed!");
        }

        riderId = registerResponse.userId;
      }

      console.log(riderId);

      const now: string = (new Date()).toISOString();

      const createRideResponse = await createRide({
        riderId: riderId,
        vehicleTypeId: vehicleTypeFormValues.vehicleType,
        startLatitude: selectedPickUpLocation.lat,
        startLongitude: selectedPickUpLocation.lng,
        startAddress: selectedPickUpAddress,
        destinationLatitude: selectedDropOffLocation.lat,
        destinationLongitude: selectedDropOffLocation.lng,
        destinationAddress: selectedDropOffAddress,
        distanceInMeters: directionApi.distanceValue,
        pickupTime: scheduleAvailable ? selectedScheduleDateIsoString : now,
        isScheduleRide: scheduleAvailable,
        modifiedBy: UserServices().getLoginInfo()?.data.user?.userName ?? "N/A",
      });

      if (!createRideResponse) {
        throw new Error("Ride creation failed!");
      }
      
      const rideId = createRideResponse.id;
      if (scheduleAvailable) {
        if (rider) {
          sendFcmNotification({
            userId: riderId,
            title: `Ride ${rideId} Booked`,
            message: `Ride ${rideId} has been booked successfully!`,
          });
          const formattedRiderPhoneNumber = "+84" + findUserByPhoneFormValues.phoneNumber.substring(1);
          sendSms({
            phoneNumber: formattedRiderPhoneNumber,
            message: `Ride ${rideId} has been booked successfully!`,
          });
        } else {
          const formattedPhoneNumber = "+84" + findUserByPhoneFormValues.phoneNumber.substring(1);
          sendSms({
            phoneNumber: formattedPhoneNumber,
            message: `Ride ${rideId} has been booked successfully!`,
          });
        }
      } else {
        if (rider) {
          sendFcmNotification({
            userId: riderId,
            title: `Ride ${rideId} Ordered`,
            message: `Ride ${rideId} has been ordered successfully!`,
          });
          const formattedPhoneNumber = "+84" + findUserByPhoneFormValues.phoneNumber.substring(1);
          sendSms({
            phoneNumber: formattedPhoneNumber,
            message: `Ride ${rideId} has been ordered successfully!`,
          });
        } else {
          const formattedPhoneNumber = "+84" + findUserByPhoneFormValues.phoneNumber.substring(1);
          sendSms({
            phoneNumber: formattedPhoneNumber,
            message: `Ride ${rideId} has been ordered successfully!`,
          });
        }
      }

      Modal.success({
        title: 'Ride Created',
        content: 'Ride has been created successfully!',
        afterClose: () => {
          navigate("/get-driver", {
            state: {
              ride: createRideResponse,
              rider: rider,
              directionApi: directionApi,
              totalPrice: estimatedPrice,
            },
          });
        },
      });

    } catch (error) {
      console.error(error);
      Modal.error({
        title: 'Error',
        content: 'Something has gone wrong!'
      });
    }
    setIsSubmitting(() => false);
    return;
  }

  return (
    <>
      <div className="min-w-full min-h-full bg-white py-6 px-12">
        <h1 className="text-green-700">Order Ride</h1>
        <div className="min-w-full flex justify-center">
          <div>
            <Card className="mr-10 mb-5 w-[600px] h-[520px]" title="Select Pick Up Location:">
              <MapWithSearchBox
                selectedLocation={selectedPickUpLocation}
                setSelectedLocation={setSelectedPickUpLocation}
                setSelectedAddress={setSelectedPickUpAddress}
                mapHeight="280px"
                mapWidth="550px"
              />
            </Card>
            <Card className="mr-10 w-[600px] h-[520px]" title="Select Drop Off Location:">
              <MapWithSearchBox
                selectedLocation={selectedDropOffLocation}
                setSelectedLocation={setSelectedDropOffLocation}
                setSelectedAddress={setSelectedDropOffAddress}
                mapHeight="280px"
                mapWidth="550px"
                markerIconUrl={DropOffMarkerIcon}
              />
            </Card>
          </div>
          <div>
            <div className="mb-2 w-[670px] flex flex-col justify-start">
              <RiderInfoCard
                rider={rider}
                setRider={setRider}
                riderInfoForm={riderInfoForm}
                findUserByPhoneForm={findUserByPhoneForm}
                setIsVip={setIsVip}
              />
            </div>
            <div className="w-[670px] mb-2">
              <Card title="Scheduled ride:">
                <DatePicker
                  showTime
                  disabled={!scheduleAvailable}
                  onChange={(date) => {
                    if (date) {
                      setSelectedScheduleDateIsoString(() => dayjs(date).toISOString());
                      console.log(dayjs(date).toISOString());
                    }
                  }}
                  defaultValue={dayjs(new Date())}
                />
                <Switch
                  disabled={!isVip}
                  onChange={(checked: boolean) => {
                    setScheduleAvailable(() => checked);
                  }}
                  className="ml-4"
                />
              </Card>
            </div>
            <div className="mb-2">
              <RideInfoCard
                pickUpLocation={selectedPickUpLocation}
                pickUpAddress={selectedPickUpAddress}
                dropOffLocation={selectedDropOffLocation}
                dropOffAddress={selectedDropOffAddress}
                cardWidth="670px"
                cardHeight=""
                directionApi={directionApi}
                setDirectionApi={setDirectionApi}
                vehicleTypeForm={vehicleTypeForm}
                estimatedPrice={estimatedPrice}
                setEstimatedPrice={setEstimatedPrice}
              />
            </div>
            <div className="text-end">
              <Button type="primary" onClick={onSubmitClick} loading={isSubmitting}>
                Choose Driver
              </Button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
