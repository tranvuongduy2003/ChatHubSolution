import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "@/stores/useAuthStore";

const AuthProtectedRoute: React.FunctionComponent = () => {
  const loggedIn = useAuthStore((state) => state.loggedIn);

  return loggedIn ? <Navigate to="/" replace /> : <Outlet />;
};

export default AuthProtectedRoute;
