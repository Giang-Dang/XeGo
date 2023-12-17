import { Button, Col, DatePicker, Form, Row, Select } from "antd";
import dayjs from "dayjs";
import { useState } from "react";
import { generateReport, getReportUri } from "../../services/ReportServices";
import UserServices from "../../services/UserServices";
import IReportInfo from "../../models/interfaces/IReportInfo";

export function RunMyReport(): React.ReactElement {
  const [startDateDisabled, setStartDateDisabled] = useState<boolean>(false);
  const [endDateDisabled, setEndDateDisabled] = useState<boolean>(false);
  const [isGenerating, setIsGenerating] = useState<boolean>(false);
  const [reportInfo, setReportInfo] = useState<IReportInfo | null>(null);

  const onRunMyReportFormFinish = async (values: {
    reportType: string;
    startDate: string;
    endDate: string;
  }) => {
    setIsGenerating(() => true);

    try {
      const startDate = dayjs(values.startDate).toISOString();
      const endDate = dayjs(values.endDate).toISOString();
      const year = "";
      const userId = UserServices().getLoginInfo()?.data.user?.userId ?? "N/A";

      const reportResponse = await generateReport({
        reportType: values.reportType,
        startDate: startDate,
        endDate: endDate,
        year: year,
        createdBy: userId,
      });
      console.log(reportResponse);
      setReportInfo(() => reportResponse);
    } catch (error) {
      console.error(error);
      
    } finally {
      setIsGenerating(() => false);
    }
  };

  const onDownloadReportClick = async () => {
    if(!reportInfo) {
      return;
    }
    const response = await getReportUri({
      userId: reportInfo.CreatedBy,
      fileName: reportInfo.ReportName,
      type: "REPORT"
    });
    if (response) {
      openInNewTab(response);
    }
  }

  const openInNewTab = (url: string) => {
    window.open(url, "_blank");
  };

  const onSelectChange = (value: string) => {
    switch (value) {
      case "RIDE_REPORT":
        setStartDateDisabled(() => false);
        setEndDateDisabled(() => false);
        break;
      default:
        setStartDateDisabled(() => true);
        setEndDateDisabled(() => true);
        break;
    }
  }

  return (
    <>
      <div className="min-h-screen flex items-center justify-center bg-white px-12 py-5">
        <div className="w-[350px] h-[350px] rounded-lg bg-white shadow-lg p-[20px]">
          <div className="mt-6 text-center text-xl font-bold text-green-600">Run My Report</div>
          <Form
            name="runMyReport"
            className="login-form mt-8 space-y-6"
            onFinish={onRunMyReportFormFinish}
          >
            <Form.Item
              name="reportType"
              rules={[{ required: true, message: "Please select a report type" }]}
            >
              <Select
                options={[
                  { value: "RIDE_REPORT", label: "Ride report" },
                  {
                    value: "REVENUE_BY_MONTH_AND_YEAR_REPORT",
                    label: "Revenue report by month and year",
                  },
                ]}
                onChange={onSelectChange}
              ></Select>
            </Form.Item>
            <Row gutter={16}>
              <Col>
                <Form.Item name="startDate">
                  <DatePicker disabled={startDateDisabled} />
                </Form.Item>
              </Col>
              <Col>
                <Form.Item name="endDate">
                  <DatePicker disabled={endDateDisabled} />
                </Form.Item>
              </Col>
            </Row>
            <Form.Item>
              <Button
                loading={isGenerating}
                type="primary"
                htmlType="submit"
                className="login-form-button w-full"
              >
                Generate
              </Button>
            </Form.Item>
            <Button className="w-full" onClick={onDownloadReportClick} disabled={reportInfo == null}>
              Download Report
            </Button>
          </Form>
        </div>
      </div>
    </>
  );
}
