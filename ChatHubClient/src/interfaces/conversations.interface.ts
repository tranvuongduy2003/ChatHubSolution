export interface Conversation {
  id: string;
  receiverId: string;
  receiverName: string;
  connectionId: string;
  status: string;
  lastMessage?: string;
  createdAt: Date;
  updatedAt: Date;
}
