import "antd/dist/reset.css";
import './index.css'
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ProtectedRoute from "./pages/ProtectedRoute";
import { HomePage } from "./pages/Home/HomePage";
import { RideOrderPage } from "./pages/RideOrder/RideOrderPage";
import { StatisticsPage } from "./pages/Statistics/StatisticsPage";
import { MainSidebar } from "./components/MainSidebar";
import LoginPage from "./pages/Login/LoginPage";

function App() {
  return (
    <Router>
      <Routes>
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <MainSidebar />
            </ProtectedRoute>
          }
        >
          <Route index element={<HomePage />} />
          <Route path="ride-order" element={<RideOrderPage />} />
          <Route path="statistics" element={<StatisticsPage />} />
        </Route>
        <Route path="/login" element={<LoginPage />} />
      </Routes>
    </Router>
  );
}

export default App
