import axios from "axios";
import getAppConstants from "../constants/AppConstants";

export async function sendScheduledSms(requestDto : {
  phoneNumber: string;
  message: string; 
  sendTime: string;
}) : Promise<boolean>{
  console.log("sendScheduledSms:");
  try {
    const { ApiUrl, JsonHeader } = getAppConstants();
    const url = `http://${ApiUrl}/api/notifications/store-scheduled-sms-sending-task`;

    const response = await axios.post(url, requestDto, {
      headers: JsonHeader,
    });

    if (response.status != 200) {
      console.log(response);
      return false;
    }
    console.log("sendScheduledSms: Done!");
    return true;
  } catch (error) {
    console.error(error);
    return false;
  }
}

export async function sendSms(requestDto: { 
  phoneNumber: string; 
  message: string;
}) : Promise<boolean> {
  console.log("sendSms:");

  try {
    const { ApiUrl, JsonHeader } = getAppConstants();
    const url = `http://${ApiUrl}/api/notifications/send-sms`;

    const response = await axios.post(url, requestDto, {
      headers: JsonHeader,
    });

    if (response.status != 200) {
      console.log(response);
      return false;
    }
    console.log("sendSms: Done!");
    return true;
  } catch (error) {
    console.error(error);
    return false;
  }
}

export async function sendFcmNotification(requestDto: {
  userId: string;
  title: string;
  message: string;
}) : Promise<boolean> {
  console.log("sendFcmNotification:");
  try {
    const { ApiUrl, JsonHeader } = getAppConstants();
    const url = `http://${ApiUrl}/api/notifications/send-fcm-notification`;

    const response = await axios.post(url, requestDto, {
      headers: JsonHeader,
    });

    if (response.status != 200) {
      console.log(response);
      return false;
    }
    console.log("sendFcmNotification: Done!");
    return true;
  } catch (error) {
    console.error(error);
    return false;
  }
}