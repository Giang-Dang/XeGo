import "antd/dist/reset.css";
import './index.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProtectedRoute from "./pages/ProtectedRoute";
import { StatisticsPage } from "./pages/Statistics/StatisticsPage";
import LoginPage from "./pages/Login/LoginPage";
import SharedLayout from "./components/SharedLayout";
import { VehiclePage } from "./pages/Vehicle/VehiclePage";
import { ConfigProvider } from "antd";
import { OrderRidePage } from "./pages/OrderRide/OrderRidePage";
import GetDriverPage from "./pages/GetDriver/GetDriverPage";
import { RunMyReport } from "./pages/RunMyReport/RunMyReport";

function App() {
  return (
    <ConfigProvider
      theme={{
        token: {
          // Seed Token
          colorPrimary: "#00b96b",

          // Alias Token
          colorBgContainer: "#f6ffed",
        },
      }}
    >
      <Router>
        <Routes>
          <Route
            path="/"
            element={
              <ProtectedRoute>
                <SharedLayout />
              </ProtectedRoute>
            }
          >
            <Route index element={<StatisticsPage />} />
            <Route path="order-ride" element={<OrderRidePage />} />
            <Route path="vehicles" element={<VehiclePage />} />
            <Route path="get-driver" element={<GetDriverPage />} />
            <Route path="statistics" element={<StatisticsPage />} />
            <Route path="run-my-report" element={<RunMyReport />} />
            <Route path="view-my-report" element={<RunMyReport />} />
          </Route>
          <Route path="/login" element={<LoginPage />} />
        </Routes>
      </Router>
    </ConfigProvider>
  );
}

export default App
