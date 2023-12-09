import "antd/dist/reset.css";
import './index.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProtectedRoute from "./pages/ProtectedRoute";
import { HomePage } from "./pages/Home/HomePage";
import { GetRiderPage } from "./pages/GetRider/GetRiderPage";
import { StatisticsPage } from "./pages/Statistics/StatisticsPage";
import LoginPage from "./pages/Login/LoginPage";
import SharedLayout from "./components/SharedLayout";
import { VehiclePage } from "./pages/Vehicle/VehiclePage";
import { ConfigProvider } from "antd";
import { OrderRidePage } from "./pages/OrderRide/OrderRidePage";

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
            <Route index element={<HomePage />} />
            <Route path="order-ride" element={<OrderRidePage />} />
            <Route path="vehicles" element={<VehiclePage />} />
            <Route path="get-rider" element={<GetRiderPage />} />
            <Route path="statistics" element={<StatisticsPage />} />
            <Route path="statistics" element={<StatisticsPage />} />
            <Route path="statistics/by-week" element={<StatisticsPage />} />
            <Route path="statistics/by-month" element={<StatisticsPage />} />
            <Route path="statistics/by-year" element={<StatisticsPage />} />
          </Route>
          <Route path="/login" element={<LoginPage />} />
        </Routes>
      </Router>
    </ConfigProvider>
  );
}

export default App
