import { Conversation } from "@/interfaces/conversations.interface";
import httpRequest from "@/services/httpRequest";

export const getConversationsByUserId = (userId: string) => {
  return httpRequest.get<Conversation[]>(`/api/conversations/${userId}`);
};
