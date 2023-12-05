import { ReactElement, useEffect } from "react";
import { Form, Input, Button, Modal } from 'antd';
import UserServices from "../../services/UserServices";
import "tailwindcss/tailwind.css";
import LoginRequestDto from "../../models/dto/LoginRequestDto";
import getAppConstants from "../../constants/AppConstants";
import { useUserStore } from "../../hooks/useUserStore";
import { useNavigate } from "react-router-dom";


export default function LoginPage(): ReactElement {
  const { user, setUser, setRoles }= useUserStore();
  const navigate = useNavigate();

  const onFinish = async (values: { phoneNumber: string, password: string} ) => {
    try {
      const loginRequestDto = new LoginRequestDto(values.phoneNumber, values.password, getAppConstants().fromAppValue);
      const loginResponse = await UserServices().login(loginRequestDto);
      
      if (!loginResponse) {
        Modal.error({
          title: "Login Failed",
          content: "Caught an unknown error!",
        });
        return;
      }

      if (!loginResponse.isSuccess) {
        Modal.error({
          title: "Login Failed",
          content: "Please check your phone number and password and try again.",
        });
        return;
      }

      console.log(loginResponse);

      console.log(loginResponse.user);

      setUser(loginResponse.user);
      setRoles(loginResponse.roles);

    } catch (error) {
      Modal.error({
        title: "Login Failed",
        content: "An error occurred. Please try again later.",
      });
      return;
    }
  };

  useEffect(() => {
    console.log(user);
    
    if (user) {
      console.log(user);
      navigate(-2);
    }
  }, [user, navigate]);

  return (
    <>
      <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
        <div className="w-[500px] h-[300px] rounded-lg bg-white shadow-lg p-[20px]">
          <div className="mt-6 text-center text-3xl font-extrabold text-gray-900">Sign In</div>
          <Form
            name="login_form"
            className="login-form mt-8 space-y-6"
            initialValues={{ remember: true }}
            onFinish={onFinish}
          >
            <div className="rounded-md shadow-sm -space-y-px">
              <Form.Item
                name="phoneNumber"
                rules={[
                  { required: true, message: "Please enter your phone number!" },
                  { pattern: new RegExp('^(0|\\+84)(3|5|7|8|9)[0-9]{8}$') , message: "Please enter a valid phone number!"},
                ]}
              >
                <Input placeholder="Phone number" />
              </Form.Item>

              <Form.Item
                name="password"
                rules={[{ required: true, message: "Please enter your password!" }]}
              >
                <Input type="password" placeholder="Password" />
              </Form.Item>
            </div>

            <div>
              <Form.Item>
                <Button type="primary" htmlType="submit" className="login-form-button w-full">
                  Sign In
                </Button>
              </Form.Item>
            </div>
          </Form>
        </div>
      </div>
    </>
  );
}
