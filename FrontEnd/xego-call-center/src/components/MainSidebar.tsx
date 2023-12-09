import React from "react";
import { Layout, Menu, MenuProps } from "antd";
import { BarChartOutlined, CustomerServiceOutlined, CarOutlined, TeamOutlined, ApiOutlined, ClusterOutlined} from "@ant-design/icons";
import { NavigateFunction, useNavigate } from "react-router-dom";

const { Sider } = Layout;

type MenuItem = Required<MenuProps>['items'][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[],
  type?: "group",
  navigate?: NavigateFunction,
  path?: string
): MenuItem {
  const handleClick = () => {
    if (path && navigate) {
      navigate(path);
    }
  };

  return {
    key,
    icon,
    children,
    label,
    type,
    onClick: handleClick,
  } as MenuItem;
}



export function MainSidebar({
  collapsed,
  onCollapse,
  onBreakpoint,
} : {
  collapsed: boolean;
  onCollapse: () => void;
  onBreakpoint: (broken: boolean) => void;
}): React.ReactElement {
  const navigate = useNavigate();

  const items: MenuProps["items"] = [
    getItem("Call Center", "callCenter", <CustomerServiceOutlined />, [
      getItem("Order Ride", "orderRide", undefined, undefined, undefined, navigate, "/order-ride"),
      getItem("Rides", "rides", undefined, undefined, undefined, navigate, "/rides"),
    ]),
    getItem("Management", "management", <ClusterOutlined />, [
      getItem("Vehicles", "vehicle", <CarOutlined />, undefined, undefined, navigate, "/vehicles"),
      getItem("Drivers", "driver", <TeamOutlined />),
    ]),
    getItem("Statistics", "statistics", <BarChartOutlined />),
    { type: "divider" },
    getItem("System Settings", "systemSettings", <ApiOutlined />),
  ];

  const onClick: MenuProps["onClick"] = (e) => {
    console.log(e);
  };

  return (
    <Sider
      theme="light"
      breakpoint="lg"
      collapsedWidth="0"
      onBreakpoint={onBreakpoint}
      collapsible
      // trigger={null}
      collapsed={collapsed}
      onCollapse={onCollapse}
    >
      <Menu
        className="pt-4"
        onClick={onClick}
        theme="light"
        mode="inline"
        defaultSelectedKeys={["1"]}
        items={items}
      />
    </Sider>
  );
}
