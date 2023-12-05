interface AppConstants {
  fromAppValue: string;
  ApiUrl: string;
  JsonHeader: { [key: string]: string };
}

export default function getAppConstants(): AppConstants {
  return {
    fromAppValue: "ADMIN",
    ApiUrl: "192.168.10.32:6100",
    JsonHeader: { "Content-Type": "application/json" },
  };
}
