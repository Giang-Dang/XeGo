import { Form, Input } from "antd";
import IVehicle from "../models/interfaces/IVehicle";
import IUser from "../models/interfaces/IUser";
import UserDto from "../models/dto/UserDto";

export function AssignDriverForm({
  setDriver,
}: {
  setDriver: React.Dispatch<React.SetStateAction<UserDto | null>>;
}): React.ReactElement {
  const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
  };

  const onFinish = () => {
    
  }

  return (
    <>
      <Form {...layout} onFinish={onFinish}>
        <Form.Item
          label="Driver's Phone Number:"
          name="phoneNumber"
          rules={[
            { required: true, message: "Please input driver's phone number" },
            {
              pattern: new RegExp("^(0|\\+84)(3|5|7|8|9)[0-9]{8}$"),
              message: "Please enter a valid phone number!",
            },
          ]}
        >
          <Input type="tel" placeholder="Phone number" />
        </Form.Item>
      </Form>
    </>
  );
}