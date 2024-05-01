import {
  LoginPayload,
  SignUpPayload,
  TokenPayload,
} from "../interfaces/auth.interface";
import httpRequest from "../services/httpRequest";

export const login = (data: LoginPayload) => {
  return httpRequest.post<LoginPayload, TokenPayload>("/auth/login", data);
};

export const signUp = (data: SignUpPayload) => {
  return httpRequest.post("/auth/signup", data);
};

export const refresh = (token: string) => {
  return httpRequest.post("/auth/refresh-token", { refreshToken: token });
};
