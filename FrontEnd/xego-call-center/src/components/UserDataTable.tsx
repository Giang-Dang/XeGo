import React from "react"
import UserDto from "../models/dto/UserDto";
import { GridValueGetterParams } from "@mui/x-data-grid";

export default function UserDataTable({ userDtos } : { userDtos: UserDto[]}): React.FC {
  const columns: GridColDef[] = [
    {
      field: "phoneNumber",
      headerName: "Phone Number",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "roles",
      headerName: "Roles",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
      valueGetter: (params: GridValueGetterParams) =>
        params.row.roles.join(', '),
    },
    {
      field: "email",
      headerName: "Email",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "firstName",
      headerName: "First Name",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "lastName",
      headerName: "Last Name",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "userName",
      headerName: "Username",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
    {
      field: "userName",
      headerName: "Username",
      headerClassName: "bg-gray-300 text-[1.02rem]",
      flex: 1,
    },
  ];
}