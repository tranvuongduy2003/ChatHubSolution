import { IResponse } from "@/interfaces/response.interface";
import { useAuthStore } from "@/stores/useAuthStore";
import { useConversationStore } from "@/stores/useConversationStore";
import {
  HttpTransportType,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { Button, Input, notification } from "antd";
import { useEffect, useRef } from "react";
import { MessageItem } from "./MessageItem";

export interface ChatRoomProps {}

export function ChatRoom() {
  const { profile } = useAuthStore((state) => state);
  const { selectedReceiver, setConnection } = useConversationStore(
    (state) => state
  );

  const joinChatRoom = useRef<(() => Promise<void>) | null>(null);

  useEffect(() => {
    joinChatRoom.current = async () => {
      if (!selectedReceiver) return;
      try {
        // initial a connection
        const conn = new HubConnectionBuilder()
          .withUrl(`${import.meta.env.VITE_SERVER_URL}/chat`, {
            skipNegotiation: true,
            transport: HttpTransportType.WebSockets,
          })
          .configureLogging(LogLevel.Debug)
          .build();

        // setup handler
        conn.on("JoinSpecificChatRoom", (conversation, message) => {
          console.log("🚀 ~ conn.on ~ message:", message);
          console.log("🚀 ~ conn.on ~ senderId:", conversation);
        });

        await conn.start();
        await conn.invoke("JoinSpecificChatRoom", {
          senderName: profile?.name,
          senderId: profile?.id,
          receiverId: selectedReceiver.id,
          receiverName: selectedReceiver.name,
        });

        setConnection(conn);
      } catch (error) {
        console.log(error);
        notification.error({
          message: (error as IResponse<unknown>).message,
        });
      }
    };
    joinChatRoom.current();
    () => joinChatRoom.current;
  }, [selectedReceiver?.id]);

  return (
    <div className="flex flex-col w-full border-2 border-gray-200 border-solid rounded-lg">
      <div className="flex items-center justify-center w-full gap-4 py-4 border-b-2 border-gray-200 border-solid">
        <img
          src="https://i.pravatar.cc/300"
          alt="avatar"
          className="w-10 h-10 rounded-full"
        />
        <div className="flex flex-col items-start justify-between">
          <h3 className="font-semibold text-gray-800">
            {selectedReceiver?.name}
          </h3>
          <div className="flex items-center gap-2">
            <div className="w-2 h-2 bg-green-400 rounded-full"></div>
            <span className="text-sm font-medium text-gray-400">Online</span>
          </div>
        </div>
      </div>
      <div className="flex flex-col flex-1 p-4">
        <div className="flex flex-col flex-1 gap-2">
          <MessageItem isReceiver={true}>Message</MessageItem>
          <MessageItem>Message</MessageItem>
          <MessageItem>Message</MessageItem>
          <MessageItem>Message</MessageItem>
          <MessageItem>Message</MessageItem>
          <MessageItem>Message</MessageItem>
          <MessageItem>Message</MessageItem>
        </div>
        <div className="flex gap-4">
          <Input placeholder="Enter message" size="large" />
          <Button size="large" type="primary">
            Send
          </Button>
        </div>
      </div>
    </div>
  );
}
