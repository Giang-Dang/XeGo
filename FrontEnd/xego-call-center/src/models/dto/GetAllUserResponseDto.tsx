import UserDto from "./UserDto";

export default interface GetAllUsersResponseDto {
  data: UserDto[];
  isSuccess: boolean;
  message: string;
}
