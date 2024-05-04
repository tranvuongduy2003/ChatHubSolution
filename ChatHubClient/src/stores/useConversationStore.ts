import { Conversation } from "@/interfaces/conversations.interface";
import { User } from "@/interfaces/user.interface";
import { HubConnection } from "@microsoft/signalr";
import { create } from "zustand";

type State = {
  selectedReceiver: User | null;
  connection: HubConnection | null;
  users: User[];
  conversations: Conversation[];
};

type Action = {
  setSelectedReceiver: (receiver: User) => void;
  setConnection: (conn: HubConnection) => void;
  setUsers: (users: User[]) => void;
  setConversations: (conversations: Conversation[]) => void;
};

const initState: State = {
  selectedReceiver: null,
  connection: null,
  users: [],
  conversations: [],
};

export const useConversationStore = create<State & Action>((set) => ({
  ...initState,
  setSelectedReceiver: (receiver) =>
    set(() => ({ selectedReceiver: { ...receiver } })),
  setConnection: (conn) => set(() => ({ connection: conn })),
  setUsers: (users) => set(() => ({ users: users })),
  setConversations: (conversations) =>
    set(() => ({ conversations: conversations })),
}));
