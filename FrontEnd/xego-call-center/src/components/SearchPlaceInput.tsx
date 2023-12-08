import { useState } from "react";
import ILatLng from "../models/interfaces/ILatLng";
import { GOOGLE_MAP_API_KEY } from "../secret/SecretKeys";
import { Button, Form, Input } from "antd";

export default function SearchPlaceInput({
  setSelectedLocation,
  setSelectedBySearchBox,
}: {
  setSelectedLocation: React.Dispatch<
    React.SetStateAction<ILatLng>
  >;
  setSelectedBySearchBox: React.Dispatch<React.SetStateAction<boolean>>;
}) {
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const [form] = Form.useForm();

  const onFinish = async (values: { searchPlace: string }) => {
    setIsLoading(() => true);
    const encodedPlace = encodeURIComponent(values.searchPlace);
    const response = await fetch(
      `https://maps.googleapis.com/maps/api/geocode/json?address=${encodedPlace}&key=${GOOGLE_MAP_API_KEY}`
    );
    const data = await response.json();
    if (data.results && data.results.length > 0) {
      const location = data.results[0].geometry.location;
      setSelectedLocation(() => ({ lat: location.lat, lng: location.lng }));
      setSelectedBySearchBox(() => true);
    } else {
      console.error("No results found");
    }

    setIsLoading(() => false);
  };

  return (
    <div className="flex items-center space-x-2 m-4">
      <Form 
        onFinish={onFinish} 
        layout="inline" 
        form={form}
      >
        <Form.Item
          label="Enter Place:"
          name="searchPlace"
          rules={[{ required: true, message: "Please input place for searching." }]}
        >
          <Input placeholder="Enter place to search"/>
        </Form.Item>
        <Form.Item>
          <Button htmlType="submit" loading={isLoading}>
            Search
          </Button>
        </Form.Item>
      </Form>
    </div>
  );
}