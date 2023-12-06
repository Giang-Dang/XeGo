import { Button, Form, Input, Modal } from "antd";
import UserDto from "../models/dto/UserDto";
import { useState } from "react";
import UserServices from "../services/UserServices";

export function FindAllUsersByPhoneNumberForm({
  setUserDtos,
}: {
  setUserDtos: React.Dispatch<React.SetStateAction<UserDto | null>>;
}): React.ReactElement {
  const [isLoading, setIsLoading] = useState(false);

  const layout = {
    labelCol: { span: 8 },
    wrapperCol: { span: 16 },
  };

  const onFinish = async (values: { phoneNumber: string }) => {
    try {
      setIsLoading(() => true);
      const userDtos = await UserServices().getAllUsers({ phoneNumber: values.phoneNumber });

      if (userDtos) {
        Modal.error({
          title: "Finding User Failed",
          content: "Caught an unknown error!",
        });

        return;
      }

      setUserDtos(() => userDtos);
    } catch (error) {
      Modal.error({
        title: "Login Failed",
        content: "An error occurred. Please try again later.",
      });
      return;
    }
  };

  return (
    <>
      <Form {...layout} onFinish={onFinish}>
        <Form.Item
          label="Driver's Phone Number:"
          name="phoneNumber"
          rules={[{ required: true, message: "Please input driver's phone number" }]}
        >
          <Input type="tel" placeholder="Phone number" />
        </Form.Item>
        <Form.Item>
          <Button
            type="primary"
            loading={isLoading}
            htmlType="submit"
            className="login-form-button w-full"
          >
            Sign In
          </Button>
        </Form.Item>
      </Form>
    </>
  );
}