import React from "react";
import { Layout, Menu, MenuProps } from "antd";
import { BarChartOutlined, CustomerServiceOutlined, CarOutlined, TeamOutlined, ApiOutlined} from "@ant-design/icons";

const { Sider } = Layout;

type MenuItem = Required<MenuProps>['items'][number];

function getItem(
  label: React.ReactNode,
  key: React.Key,
  icon?: React.ReactNode,
  children?: MenuItem[],
  type?: 'group',
): MenuItem {
  return {
    key,
    icon,
    children,
    label,
    type,
  } as MenuItem;
}

const items: MenuProps["items"] = [
  getItem("Call Center", "callCenter", <CustomerServiceOutlined />),
  getItem("Vehicle", "vehicle", <CarOutlined />),
  getItem("Drivers Management", "driverManagement", <TeamOutlined />),
  getItem("Statistics", "statistics", <BarChartOutlined />, [
    getItem("By Week", "byWeek"),
    getItem("By Month", "byMonth"),
    getItem("By Year", "byYear"),
  ]),
  { type: 'divider'},
  getItem('System Settings', 'systemSettings', <ApiOutlined />),
];

export function MainSidebar({
  collapsed,
  onCollapse,
  onBreakpoint,
}: {
  collapsed: boolean;
  onCollapse: () => void;
  onBreakpoint: (broken: boolean) => void;
}): React.ReactElement {
  const onClick: MenuProps["onClick"] = (e) => {
    console.log(e);
  };

  return (
    <Sider
      theme="light"
      breakpoint="lg"
      collapsedWidth="0"
      onBreakpoint={onBreakpoint}
      trigger={null}
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
