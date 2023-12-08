import axios from 'axios';
import getAppConstants from '../constants/AppConstants';
import LoginRequestDto from '../models/dto/LoginRequestDto';
import LoginResponseDto from '../models/dto/LoginResponseDto';
import UserDto from '../models/dto/UserDto';
import GetAllUsersResponseDto from '../models/dto/GetAllUserResponseDto';
import RoleConstants from '../constants/RoleConstans';
import ICreateUserResponseDto from '../models/interfaces/ICreateUserResponseDto';



const UserServices = () =>
{
  const login = async function (requestDto: LoginRequestDto): Promise<LoginResponseDto | null> {
      
    const { ApiUrl, JsonHeader} = getAppConstants();
    const url = `http://${ApiUrl}/api/auth/user/login`;

    try {
      const response = await axios.post(url, requestDto, {
          headers: JsonHeader,
      });

      console.log(response.data);

      const loginResponseDto: LoginResponseDto | null = LoginResponseDto.fromJson(response.data);
      
      if(loginResponseDto) {
        await saveLoginInfo(loginResponseDto);
      }

      console.log(loginResponseDto);
      
      return loginResponseDto;
    } catch (error) {
      let loginResponseDto: LoginResponseDto | null;
      if (error instanceof Error) {
        loginResponseDto = new LoginResponseDto(null, null, false, error.message);
      } else {
        loginResponseDto = new LoginResponseDto(null, null, false, "Caught an unknown error!");
      }
      return loginResponseDto;
    }
  }

  const registerNewRider = async function (requestDto: {
    userName: string;
    phoneNumber: string;
    firstName: string;
    lastName: string;
    address: string;
    password?: string | null | undefined;
    role?: string | null | undefined;
  }): Promise<UserDto | null> {
    try {
      const { ApiUrl, JsonHeader, defaultRiderPassword } = getAppConstants();
      const url = `http://${ApiUrl}/api/auth/user/register`;

      if (!requestDto.password) {
        requestDto.password = defaultRiderPassword;
      }
      requestDto.role = RoleConstants().rider;

      const response = await axios.post(url, requestDto, {
        headers: JsonHeader,
      });

      const createdUser = response.data.data as ICreateUserResponseDto;
      const returnedUser: UserDto = {
        userId: createdUser.id,
        userName: createdUser.userName,
        email: createdUser.email,
        phoneNumber: createdUser.phoneNumber,
        firstName: createdUser.firstName,
        lastName: createdUser.lastName,
        address: createdUser.address,
        roles: [ RoleConstants().rider ],
      };
      return returnedUser;
    } catch (error) {
      console.error(error);
      return null;
    }
  };

  
  async function getAllUsers(params: {
    userName?: string;
    email?: string;
    phoneNumber?: string;
    firstName?: string;
    lastName?: string;
    address?: string;
    role?: string;
    pageNumber?: number;
    pageSize?: number;
  }): Promise<UserDto[] | null> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/auth/user`;
    let userDtos: UserDto[] | null = null;

    try {
      const response = await axios.get<GetAllUsersResponseDto>(url, { params });
      userDtos = response.data.data;
    } catch (error) {
      console.error(error);
    }

    return userDtos;
  }

  async function getUserAvatar(params: {userId: string, imageSize: string}) : Promise<string | null> {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/images/avatar`;
    let res: string | null = null;

    try {
      const response = await axios.get(url, {params});
      res = response.data.data;
    } catch (error) {
      console.error(error);
    }

    return res;
  }

  const saveLoginInfo = (loginInfo: LoginResponseDto) => {
      localStorage.setItem("loginInfo", JSON.stringify(loginInfo));
  };

  const getLoginInfo = () : LoginResponseDto | null => {
      const loginInfoResponse = localStorage.getItem("loginInfo");
      if(loginInfoResponse) {
          const loginResponse = LoginResponseDto
              .fromJson(JSON.parse(loginInfoResponse));
          return loginResponse;
      }
      return null;
  };

  return {
    login: login,
    saveLoginInfo: saveLoginInfo,
    getLoginInfo: getLoginInfo,
    getAllUsers: getAllUsers,
    getUserAvatar: getUserAvatar,
    registerNewRider: registerNewRider,
  };
};

export default UserServices;
