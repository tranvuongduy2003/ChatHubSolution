import {
  LoginPayload,
  SignUpPayload,
  TokenPayload,
} from "@/interfaces/auth.interface";
import { User } from "@/interfaces/user.interface";
import httpRequest from "@/services/httpRequest";

export const login = (data: LoginPayload) => {
  return httpRequest.post<LoginPayload, TokenPayload>("/api/auth/signin", data);
};

export const signUp = (data: SignUpPayload) => {
  return httpRequest.post<SignUpPayload, TokenPayload>(
    "/api/auth/signup",
    data
  );
};

export const getProfile = () => {
  return httpRequest.get<User>("/api/auth/profile");
};
