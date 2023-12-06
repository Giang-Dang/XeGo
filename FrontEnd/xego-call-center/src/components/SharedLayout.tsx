import { Layout } from "antd";
import { MainSidebar } from "./MainSidebar";
import { Footer, Header } from "antd/es/layout/layout";
import { Outlet } from "react-router-dom";
import { MenuUnfoldOutlined, MenuFoldOutlined } from "@ant-design/icons";
import { useState } from "react";

export default function SharedLayout() {
  const [collapsed, setCollapsed] = useState(false);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  console.log(collapsed);

  return (
    <Layout className="min-h-screen">
      <Header className="bg-green-500 px-6">
        {collapsed ? (
          <MenuUnfoldOutlined className="text-xl" onClick={toggleCollapsed} />
        ) : (
          <MenuFoldOutlined className="text-xl" onClick={toggleCollapsed} />
        )}
      </Header>
      <Layout>
        <MainSidebar
          collapsed={collapsed}
          onCollapse={toggleCollapsed}
          onBreakpoint={setCollapsed}
        />
        <Layout className="pl-6 pb-6">
          {/* <Breadcrumb className="my-4" items={[{ title: "Home" }, { title: "List" }]} /> */}
          <Outlet />
        </Layout>
      </Layout>
      <Footer className="bg-gray-200 text-center text-gray-700">
        ©{new Date().getFullYear()} XeGo - 21880213 - Đặng Vũ Ngọc Giang
      </Footer>
    </Layout>
  );
}