import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "@/stores/useAuthStore";

const HomeLayout: React.FunctionComponent = () => {
  const loggedIn = useAuthStore((state) => state.loggedIn);

  return !loggedIn ? <Navigate to="/auth/login" replace /> : <Outlet />;
};

export default HomeLayout;
