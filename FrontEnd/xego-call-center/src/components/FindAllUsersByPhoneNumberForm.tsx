import { Button, Card, Form, Image, Input, Modal } from "antd";
import UserDto from "../models/dto/UserDto";
import { useState } from "react";
import UserServices from "../services/UserServices";
import ImageSizeConstants from "../constants/ImageSizeConstants";

export function FindAllUsersByPhoneNumberForm({
  setSelectedDriver,
}: {
  setSelectedDriver: React.Dispatch<React.SetStateAction<UserDto | undefined>>;
}): React.ReactElement {
  const [isLoading, setIsLoading] = useState(false);
  const [driver, setDriver] = useState<UserDto | null>(null);
  const [driverAvatarUrl, setDriverAvatarUrl] = useState<string | null>(null);

  const [form] = Form.useForm();

  const onFinish = async (values: { phoneNumber: string }) => {
    try {
      setIsLoading(() => true);
      const userDtos = await UserServices().getAllUsers({ phoneNumber: values.phoneNumber });
      if (!userDtos || userDtos.length == 0) {
        Modal.error({
          title: "Driver Not Found",
          content: "Driver Not Found!",
        });
        setIsLoading(() => false);

        return;
      }

      console.log(userDtos);

      const avatarUrl = await UserServices().getUserAvatar({
        userId: userDtos[0].userId,
        imageSize: ImageSizeConstants().origin,
      });

      setIsLoading(() => false);

      setSelectedDriver(() => userDtos[0]);
      setDriver(() => userDtos[0]);
      setDriverAvatarUrl(() => avatarUrl);
    } catch (error) {
      console.log(error);
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
      <Card className="w-[440px]" title="Find Driver">
        <Form onFinish={onFinish} form={form}>
          <Form.Item
            label="Phone Number:"
            name="phoneNumber"
            rules={[{ required: true, message: "Please input driver's phone number" }]}
          >
            <Input type="tel" placeholder="Phone number" />
          </Form.Item>
          <Form.Item className="text-end">
            <Button
              type="primary"
              loading={isLoading}
              htmlType="submit"
              className="login-form-button"
            >
              Find
            </Button>
          </Form.Item>
        </Form>
        <Card title="Driver Info:" loading={isLoading}>
          <div className="flex justify-start">
            <div className="m-4">
              <Image
                width={90}
                src={driverAvatarUrl ?? "https://img.icons8.com/parakeet/96/person-male.png"}
              />
            </div>
            <div>
              <p>
                <strong>Name:</strong>{" "}
                {driver ? `${driver?.firstName}, ${driver?.lastName}` : "........, .........."}
              </p>
              <p>
                <strong>Phone Number:</strong> {driver ? `${driver?.phoneNumber}` : "........."}
              </p>
              <p>
                <strong>Address:</strong> {driver ? `${driver?.address}` : "........."}
              </p>
            </div>
          </div>
        </Card>
      </Card>
    </>
  );
}