import axios from 'axios';
import getAppConstants from '../constants/AppConstants';
import LoginRequestDto from '../models/dto/LoginRequestDto';
import LoginResponseDto from '../models/dto/LoginResponseDto';



const UserServices = () =>
{
  const login = async function (requestDto: LoginRequestDto): Promise<LoginResponseDto | null> {
      
    const { ApiUrl, JsonHeader} = getAppConstants();
    const url = `http://${ApiUrl}/api/auth/user/login`;

    try {
      const response = await axios.post(url, requestDto, {
          headers: JsonHeader,
      });

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
  };

    // register: async (requestDto: RegistrationRequestDto): Promise<any | null> => {
    //     const url = `http://${Secrets.ApiUrl}/api/auth/user/register`;

    //     try {
    //         const response = await axios.post(url, requestDto, {
    //             headers: Secrets.JsonHeader,
    //         });

    //         if (!response.data.isSuccess) {
    //             return response.data;
    //         }

    //         UserServices.userDto = {
    //             userId: response.data.data.id,
    //             userName: response.data.data.userName,
    //             email: response.data.data.email,
    //             phoneNumber: response.data.data.phoneNumber,
    //             firstName: response.data.data.firstName,
    //             lastName: response.data.data.lastName,
    //             address: response.data.data.address,
    //         };

    //         return response.data;
    //     } catch (error) {
    //         console.error(error);
    //         return null;
    //     }
    // },

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
    };
};

export default UserServices;
