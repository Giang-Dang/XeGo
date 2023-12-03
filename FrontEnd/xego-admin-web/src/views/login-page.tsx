import { useState } from "react";
import InputField from "../components/InputField";
import UserServices from "../services/user-services";
import LoginRequestDto from "../models/dto/LoginRequestDto";
import { Secrets } from "../constants/secrets";

const Login = () => {
  const [phone, setPhone] = useState("");
  const [password, setPassword] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    // Handle your form submission here
    console.log(`Phone: ${phone}, Password: ${password}`);
    //
    const requestDto = new LoginRequestDto(phone, password, Secrets.FromAppValue);
    const loginResponse = await UserServices.login(requestDto);
    console.log(loginResponse);
    
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 py-12 px-4 sm:px-6 lg:px-8">
      <div className="max-w-md w-full space-y-8">
        <div>
          <h2 className="mt-6 text-center text-3xl font-extrabold text-gray-900">
            Sign in to your account
          </h2>
        </div>
        <form className="mt-8 space-y-6" onSubmit={handleSubmit}>
          <input type="hidden" name="remember" value="true" />
          <div className="rounded-md shadow-sm -space-y-px">
            <InputField
              id="phone-number"
              name="phone"
              type="tel"
              autoComplete="tel"
              required
              placeholder="Phone Number"
              value={phone}
              onChange={(e) => setPhone(e.target.value)}
            />
            <InputField
              id="password"
              name="password"
              type="password"
              autoComplete="current-password"
              required
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>

          <div>
            <button
              type="submit"
              className="group relative w-full flex justify-center py-2 px-4 border 
              border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 
              hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
            >
              Sign in
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default Login;
