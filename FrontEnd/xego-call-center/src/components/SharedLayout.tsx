import { Layout } from "antd";
import { MainSidebar } from "./MainSidebar";
import { Footer, Header } from "antd/es/layout/layout";
import { Outlet } from "react-router-dom";
// import { MenuUnfoldOutlined, MenuFoldOutlined } from "@ant-design/icons";
import { useState } from "react";

export default function SharedLayout() {
  const [collapsed, setCollapsed] = useState(false);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  return (
    <Layout className="min-h-screen">
      <MainSidebar collapsed={collapsed} onCollapse={toggleCollapsed} onBreakpoint={setCollapsed} />
      <Layout>
        <Header className="bg-green-500 px-6 text-center">
        <span className="text-white text-bold text-2xl">XeGo - Call Center</span>
        </Header>
        <Layout className="">
          <Outlet />
        </Layout>
        <Footer className="bg-gray-200 text-center text-gray-700">
          ©{new Date().getFullYear()} XeGo - 21880213 - Đặng Vũ Ngọc Giang
        </Footer>
      </Layout>
    </Layout>
  );
}