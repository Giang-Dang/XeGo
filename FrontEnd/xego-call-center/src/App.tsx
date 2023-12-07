import "antd/dist/reset.css";
import './index.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProtectedRoute from "./pages/ProtectedRoute";
import { HomePage } from "./pages/Home/HomePage";
import { RideOrderPage } from "./pages/RideOrder/RideOrderPage";
import { StatisticsPage } from "./pages/Statistics/StatisticsPage";
import LoginPage from "./pages/Login/LoginPage";
import SharedLayout from "./components/SharedLayout";
import { VehiclePage } from "./pages/Vehicle/VehiclePage";
import { ConfigProvider } from "antd";

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
            <Route path="vehicles" element={<VehiclePage />} />
            <Route path="ride-order" element={<RideOrderPage />} />
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
