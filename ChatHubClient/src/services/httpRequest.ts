import { AxiosInstance } from "axios";
import { axiosInstance, IConfig } from "./initRequest";

class HttpRequest {
  api: AxiosInstance;

  constructor() {
    this.api = axiosInstance;
  }

  async get(url: string, config?: IConfig) {
    return this.api.get(url, config);
  }

  async post<TPayload, TData>(url: string, data: TPayload, config?: IConfig) {
    return this.api.post<TData>(url, data, config);
  }

  async put<TPayload, TData>(url: string, data: TPayload, config?: IConfig) {
    return this.api.put<TData>(url, data, config);
  }

  async patch<TPayload, TData>(url: string, data?: TPayload, config?: IConfig) {
    return this.api.patch<TData>(url, data, config);
  }

  async delete(url: string, config?: IConfig) {
    return this.api.delete(url, config);
  }
}

const httpRequest = new HttpRequest();

export default httpRequest;
