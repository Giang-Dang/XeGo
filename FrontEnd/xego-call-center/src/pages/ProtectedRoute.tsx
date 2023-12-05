import { Navigate, useLocation } from "react-router-dom";
import { useUserStore } from "../hooks/useUserStore";

export default function ProtectedRoute({ children } : { children: React.ReactNode }) {
  const user = useUserStore((state) => state.user);
  const location = useLocation();

  if (user === null) {
    return <Navigate to="/login" state={{ from: location }} />;
  }

  return(
    <>{children}</>
  );
}