import { Button, Form, FormInstance, Input, Modal, Typography } from "antd";
import UserDto from "../models/dto/UserDto";
import { useState } from "react";
import UserServices from "../services/UserServices";

const { Text } = Typography

export function FindRiderByPhoneForm({
  selectedUser,
  setSelectedUser,
  findUserByPhoneForm,
}: {
  selectedUser: UserDto | undefined;
  setSelectedUser: React.Dispatch<React.SetStateAction<UserDto | undefined>>;
  findUserByPhoneForm: FormInstance;
}): React.ReactElement {
  const [isLoading, setIsLoading] = useState(false);

  const onFinish = async (values: { phoneNumber: string }) => {
    try {
      setIsLoading(() => true);
      const userDtos = await UserServices().getAllUsers({ phoneNumber: values.phoneNumber });
      
      console.log(userDtos ?? "userDtos null");

      if (!userDtos || userDtos.length == 0) {
        setSelectedUser(() => undefined);
        setIsLoading(() => false);

        return;
      }

      if (!userDtos[0].roles.some((value) => value.toUpperCase() == "RIDER")) {
        setSelectedUser(() => undefined);
        setIsLoading(() => false);

        return;
      }

      setIsLoading(() => false);

      setSelectedUser(() => userDtos[0]);
    } catch (error) {
      console.error(error);
      Modal.error({
        title: "Login Failed",
        content: "An error occurred. Please try again later.",
      });
      setIsLoading(() => false);

      return;
    }
  };

  return (
    <>
      <div className="w-[470px]">
        <Form className="mb-4" layout="inline" onFinish={onFinish} form={findUserByPhoneForm}>
          <Form.Item
            label="Phone Number:"
            name="phoneNumber"
            rules={[{ required: true, message: "Please enter phone number." }]}
          >
            <Input type="tel" placeholder="Phone number" />
          </Form.Item>
          <Form.Item className="text-end">
            <Button
              loading={isLoading}
              htmlType="submit"
              className="login-form-button"
            >
              Find
            </Button>
          </Form.Item>
        </Form>
        {!selectedUser && (
          <div className="text-center">
            <Text type="danger">Rider is not found! Please provide the following information.</Text>
          </div>
        )}
      </div>
    </>
  );
}
