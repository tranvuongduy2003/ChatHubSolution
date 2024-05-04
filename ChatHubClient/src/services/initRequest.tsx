import axios, {
  AxiosError,
  AxiosRequestConfig,
  InternalAxiosRequestConfig,
} from "axios";
import { getAccessToken } from "@/utils/auth";

const requestConfig: AxiosRequestConfig = {
  baseURL: import.meta.env.VITE_SERVER_URL,
  timeout: 20000,
  headers: {
    "Content-Type": "application/json",
  },
};

export type IConfig = AxiosRequestConfig;

export const axiosInstance = axios.create(requestConfig);

export default function initRequest() {
  axiosInstance.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
      const token = getAccessToken();
      if (token && config.headers) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error: AxiosError) => {
      return Promise.reject(error.response?.data);
    }
  );

  axiosInstance.interceptors.response.use(
    (res) => {
      return res.data;
    },
    async (error) => {
      const statusCode = error.response?.status;

      switch (statusCode) {
        case 401: {
          break;
        }
        case 403: {
          break;
        }
        case 500: {
          break;
        }
        default:
          break;
      }
      return Promise.reject(error.response?.data);
    }
  );
}

initRequest();
