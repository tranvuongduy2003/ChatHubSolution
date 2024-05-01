import { notification } from "antd";
import React, { PropsWithChildren } from "react";
import { useNavigate } from "react-router-dom";
import { LoginPayload } from "../interfaces/auth.interface";
import { useAppStore } from "../stores/useAppStore";
import { useAuthStore } from "../stores/useAuthStore";
import { login } from "../apis/auth.api";

export interface AuthContextProps {
  logIn: (payload: LoginPayload) => Promise<void>;
}

export const AuthContext = React.createContext<Partial<AuthContextProps>>({});

const AuthProvider = ({ children }: PropsWithChildren) => {
  const navigate = useNavigate();

  const { setIsLoading } = useAppStore((state) => state);
  const { setToken, setLoggedIn } = useAuthStore((state) => state);

  const logIn = async (payload: LoginPayload) => {
    setIsLoading(true);
    try {
      // LOGIN THEN GET AND SET TOKENS
      const { data } = await login(payload);

      // SAVE USER SESSION
      setToken(data);
      setLoggedIn(true);

      // NAVIGATE TO HOME PAGE
      setIsLoading(false);
      notification.success({
        message: "Login successfully!",
        duration: 0.25,
        onClose: () => navigate("/"),
      });
    } catch (error) {
      setIsLoading(false);
      console.log(error);
      notification.error({
        message: JSON.stringify(error),
      });
    }
  };

  return (
    <AuthContext.Provider
      value={{
        logIn,
      }}
    >
      <>{children}</>
    </AuthContext.Provider>
  );
};
export default AuthProvider;
