import { Card, Form, FormInstance, Input, Select } from "antd";
import { FindRiderByPhoneForm } from "./FindRiderByPhoneForm";
import { useEffect } from "react";
import UserDto from "../models/dto/UserDto";
import RiderTypeConstants from "../constants/RiderTypeConstants";
import { getRiderType } from "../services/RiderServices";

interface RiderInfoCardProps {
  rider: UserDto | undefined;
  setRider: React.Dispatch<React.SetStateAction<UserDto | undefined>>;
  riderInfoForm: FormInstance;
  findUserByPhoneForm: FormInstance;
}

const RiderInfoCard: React.FC<RiderInfoCardProps> = ({
  rider,
  setRider,
  riderInfoForm,
  findUserByPhoneForm,
}) => {

  useEffect(() => {
    riderInfoForm.setFieldsValue(() => ({
      firstName: rider?.firstName ?? "",
      lastName: rider?.lastName ?? "",
    }));
  }, [rider, riderInfoForm]);

  useEffect(() => {
    if (rider) {
      const fetchRiderType = async () => {
        const response = await getRiderType(rider.userId);

        console.log(response);

        if (response) {
          riderInfoForm.setFieldsValue({
            userType: response,
          });
        }
      };

      fetchRiderType();
    }
  }, [rider, riderInfoForm]);

  return (
    <Card className="mb-5" title="Rider Infomation:">
      <FindRiderByPhoneForm
        selectedUser={rider}
        setSelectedUser={setRider}
        findUserByPhoneForm={findUserByPhoneForm}
      />
      <Form className="mt-5" form={riderInfoForm}>
        <Form.Item
          label="First Name:"
          name="firstName"
          rules={[{ required: true, message: "Please enter rider's First name." }]}
        >
          <Input disabled={rider != null} placeholder="Enter first name" />
        </Form.Item>
        <Form.Item
          label="Last Name:"
          name="lastName"
          rules={[{ required: true, message: "Please enter rider's Last name." }]}
        >
          <Input disabled={rider != null} placeholder="Enter last name" />
        </Form.Item>
        <Form.Item
          label="User Type:"
          name="userType"
          rules={[{ required: true, message: "Please select a type." }]}
        >
          <Select
            disabled={rider != null}
            placeholder="Select a type"
            options={[
              { value: RiderTypeConstants().normal, label: "Normal" },
              { value: RiderTypeConstants().vip, label: "VIP" },
            ]}
          />
        </Form.Item>
      </Form>
    </Card>
  );
};

export default RiderInfoCard;
