import { notification } from "antd";
import React, { PropsWithChildren } from "react";
import { useNavigate } from "react-router-dom";
import { LoginPayload, SignUpPayload } from "@/interfaces/auth.interface";
import { useAppStore } from "@/stores/useAppStore";
import { useAuthStore } from "@/stores/useAuthStore";
import { getProfile, login, signUp } from "@/apis/auth.api";
import { IResponse } from "@/interfaces/response.interface";

export interface AuthContextProps {
  logIn: (payload: LoginPayload) => Promise<void>;
  register: (payload: SignUpPayload) => Promise<void>;
  logOut: () => void;
}

export const AuthContext = React.createContext<Partial<AuthContextProps>>({});

const AuthProvider = ({ children }: PropsWithChildren) => {
  const navigate = useNavigate();

  const { setIsLoading } = useAppStore((state) => state);
  const { setToken, setLoggedIn, setProfile } = useAuthStore((state) => state);

  const logIn = async (payload: LoginPayload) => {
    setIsLoading(true);
    try {
      // LOGIN THEN GET AND SET TOKENS
      const { data } = await login(payload);

      // SAVE USER SESSION
      setToken(data.accessToken);
      setLoggedIn(true);

      const { data: profileData } = await getProfile();
      setProfile(profileData);

      // NAVIGATE TO HOME PAGE
      setIsLoading(false);
      notification.success({
        message: "Login successfully!",
        duration: 0.25,
        onClose: () => navigate("/"),
      });
    } catch (error: unknown) {
      setIsLoading(false);
      console.log("🚀 ~ logIn ~ error:", error);
      notification.error({
        message: (error as IResponse<unknown>).message,
      });
    }
  };

  const register = async (payload: SignUpPayload) => {
    setIsLoading(true);
    try {
      // LOGIN THEN GET AND SET TOKENS
      const { data: tokenData } = await signUp(payload);

      // SAVE USER SESSION
      setToken(tokenData.accessToken);
      setLoggedIn(true);

      const { data: profileData } = await getProfile();
      setProfile(profileData);

      // NAVIGATE TO HOME PAGE
      setIsLoading(false);
      notification.success({
        message: "Sign up successfully!",
        duration: 0.25,
        onClose: () => navigate("/"),
      });
    } catch (error) {
      setIsLoading(false);
      console.log(error);
      notification.error({
        message: (error as IResponse<unknown>).message,
      });
    }
  };

  const logOut = () => {
    setToken("");
    setProfile(null);
    setLoggedIn(false);
    localStorage.clear();
  };

  return (
    <AuthContext.Provider
      value={{
        logIn,
        register,
        logOut,
      }}
    >
      <>{children}</>
    </AuthContext.Provider>
  );
};
export default AuthProvider;
