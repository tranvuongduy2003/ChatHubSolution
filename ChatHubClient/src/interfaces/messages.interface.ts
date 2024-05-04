export interface Message {
  id: string;
  userId: string;
  conversationId: string;
  content: string;
  status: string;
  createdAt: Date;
  updatedAt: Date;
}
