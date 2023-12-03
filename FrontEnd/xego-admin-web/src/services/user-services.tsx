import axios from 'axios';
import Cookies from 'js-cookie';
import {Secrets} from '../constants/secrets';
import LoginRequestDto from '../models/dto/LoginRequestDto';
import LoginResponseDto from '../models/dto/LoginResponseDto';
import TokensDto from '../models/dto/TokensDto';
import UserDto from '../models/dto/UserDto';



const UserServices = {
    userDto: null as UserDto | null,
    accessToken: null as string | null,
    refreshToken: null as string | null,
    
    login: async function (requestDto: LoginRequestDto): Promise<boolean> {
        const url = `http://${Secrets.ApiUrl}/api/auth/user/login`;

        try {
            const response = await axios.post(url, requestDto, {
                headers: Secrets.JsonHeader,
            });
            
            console.log(response.data);
            
            if (!response.data.isSuccess) {
                return false;
            }

            const loginResponseDto: LoginResponseDto = response.data.data;

            console.log(loginResponseDto);
            

            // UserServices.userDto = loginResponseDto.user;
            // UserServices.accessToken = loginResponseDto.tokens.accessToken;
            // UserServices.refreshToken = loginResponseDto.tokens.refreshToken;
            

            // await this.saveTokensDto(loginResponseDto.tokens);
            
            return true;
        } catch (error) {
            console.error(error);
            return false;
        }
    },

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

    saveTokensDto: async (tokensDto: TokensDto) => {
        Cookies.set(Secrets.AccessTokenKeyName, tokensDto.accessToken);
        Cookies.set(Secrets.RefreshTokenKeyName, tokensDto.refreshToken);
    },
};

export default UserServices;
