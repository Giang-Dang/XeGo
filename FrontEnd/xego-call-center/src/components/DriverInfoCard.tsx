import { Card, Image } from "antd";
import { useEffect, useState } from "react";
import UserServices from "../services/UserServices";
import ImageSizeConstants from "../constants/ImageSizeConstants";
import IDriver from "../models/interfaces/IDriver";
import ProfileImage from "../assets/images/unisex-profile-image.png";

export default function DriverInfoCard({ driver }: { driver: IDriver | null | undefined}): React.ReactElement {
  
  const [driverAvatarUrl, setDriverAvatarUrl] = useState<string | null>(null);

  useEffect(() => {
    if(!driver) {
      return;
    }
    const fetchDriverAvatarUrl = async () => {
      const avatarUrl = await UserServices().getUserAvatar({
        userId: driver.userId,
        imageSize: ImageSizeConstants().origin,
      });
      
      if(!avatarUrl) {
        setDriverAvatarUrl(() => null);
        return;
      }
      setDriverAvatarUrl(() => avatarUrl);
    };
    fetchDriverAvatarUrl();
  }, [driver]);
  
  return (
    <>
      <Card className="w-[440px]" title="Driver Info:" loading={!driver}>
        <div className="flex justify-start">
          <div className="m-4">
            {driver && <Image
              key={driver.userId}
              width={90}
              src={driverAvatarUrl ?? ProfileImage}
            />}
            
          </div>
          <div>
            <p>
              <strong>Name:</strong>{" "}
              {driver ? `${driver?.firstName}, ${driver?.lastName}` : "........, .........."}
            </p>
            <p>
              <strong>Phone Number:</strong> {driver ? `${driver?.phoneNumber}` : "........."}
            </p>
            <p>
              <strong>Address:</strong> {driver ? `${driver?.address}` : "........."}
            </p>
          </div>
        </div>
      </Card>
    </>
  );
}
