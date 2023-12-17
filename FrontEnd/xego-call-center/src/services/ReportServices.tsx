import axios from "axios";
import IReportInfo from "../models/interfaces/IReportInfo";
import getAppConstants from "../constants/AppConstants";

export async function generateReport(requestDto : {
  reportType: string;
  startDate?: string;
  endDate?: string;
  year?: string;
  createdBy: string;
}) : Promise<IReportInfo | null> {
  try {
    const { ApiUrl, JsonHeader } = getAppConstants();
    const url = `http://${ApiUrl}/api/reports/generate-report`;
    
    const response = await axios.post(url, requestDto, {
      headers: JsonHeader,
    });
    console.log(response.data);

    return response.data.Value as IReportInfo | null;
  } catch (error) {
    console.error(error);
    return null;
  }
}

export async function getReportUri(requestDto: {
  userId: string;
  fileName: string;
  type: string;
}) : Promise<string | null> {
  try {
    const { ApiUrl } = getAppConstants();
    const url = `http://${ApiUrl}/api/files/uri?userId=${requestDto.userId}&fileName=${requestDto.fileName}.xlsx&type=REPORT`;

    const response = await axios.get(url);
    console.log(response.data);

    return response.data.data as string | null;
  } catch (error) {
    console.error(error);
    return null;
  }
}