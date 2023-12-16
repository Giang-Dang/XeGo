import axios from "axios";
import getAppConstants from "../constants/AppConstants";
import IPrice from "../models/interfaces/IPrice";

export async function getAllPrices() {
  try {
    console.log("getAllPrices:");
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/price`;

    const response = await axios.get(url);
    console.log(response);

    if (response.data.isSuccess) {
      console.log(response.data.data);
      return response.data.data as IPrice[];
    } else {
      console.error(response.data.message);
      return [];
    }
  } catch (error) {
    console.error(error);
    return [];
  }
}