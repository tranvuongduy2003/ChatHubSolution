import { User } from "@/interfaces/user.interface";
import httpRequest from "@/services/httpRequest";

export const getUsers = (search?: string) => {
  return httpRequest.get<User[]>("/api/users", {
    params: { search },
  });
};
