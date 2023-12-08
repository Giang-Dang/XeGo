import axios from "axios";
import getAppConstants from "../constants/AppConstants";

export async function getRiderType (riderId: string) : Promise<string | null>{
  const subApiUrl = "api/auth/user/rider-type";
  const { ApiUrl } = getAppConstants()
  const url = `http://${ApiUrl}/${subApiUrl}?id=${riderId}`;

  try {
    const response = await axios.get(url);

    if (response.data["isSuccess"]) {
      const riderType = response.data["data"];

      return riderType;
    }
  } catch (error) {
    console.error(error);
  }
  return null;

}

