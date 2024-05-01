export interface LoginPayload {
  email: string;
  password: string;
}

export interface SignUpPayload {
  email: string;
  password: string;
  fullName: string;
}

export interface TokenPayload {
  accessToken: string;
  refreshToken: string;
}
