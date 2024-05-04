import { Message } from "@/interfaces/messages.interface";
import httpRequest from "@/services/httpRequest";

export const getMessagesByConversationId = (conversationId: string) => {
  return httpRequest.get<Message[]>(`/api/messages/${conversationId}`);
};
