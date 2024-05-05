import { getMessagesByConversationId } from "@/apis/messages.api";
import { Conversation } from "@/interfaces/conversations.interface";
import { Message } from "@/interfaces/messages.interface";
import { IResponse } from "@/interfaces/response.interface";
import { useAuthStore } from "@/stores/useAuthStore";
import { useConversationStore } from "@/stores/useConversationStore";
import {
  HttpTransportType,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";
import { Button, Form, Input, List, notification } from "antd";
import { useForm } from "antd/es/form/Form";
import { useEffect, useRef, useState } from "react";
import VirtualList from "react-tiny-virtual-list";
import { MessageItem } from "./MessageItem";

export interface ChatRoomProps {}

const ContainerHeight = 454;

export function ChatRoom() {
  const { profile } = useAuthStore((state) => state);
  const { selectedReceiver, setConnection, connection } = useConversationStore(
    (state) => state
  );

  const [form] = useForm<{ message: string }>();

  const [conversation, setConversation] = useState<Conversation>();
  const [messages, setMessages] = useState<Message[]>([]);
  const [loading, setLoading] = useState<boolean>();

  const joinChatRoom = useRef<(() => Promise<void>) | null>(null);
  const handleFetchMessages = useRef<(() => Promise<void>) | null>(null);

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
          setConversation(conversation);
          console.log("🚀 ~ conn.on ~ message:", message);
          console.log("🚀 ~ conn.on ~ senderId:", conversation);
        });

        conn.on("ReceiveSpecificMessage", (message: Message) => {
          console.log("🚀 ~ conn.on ~ message:", message);
          setMessages((prevMessages) => {
            const cloneMessages = JSON.parse(
              JSON.stringify(prevMessages)
            ) as Message[];
            cloneMessages.push(message);
            return cloneMessages;
          });
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

  useEffect(() => {
    handleFetchMessages.current = async () => {
      if (!conversation) return;
      setLoading(true);
      try {
        const { data } = await getMessagesByConversationId(conversation?.id);
        setMessages((messages) => messages.concat(data));
        setLoading(false);
      } catch (error) {
        console.log(error);
        notification.error({
          message: (error as IResponse<unknown>).message,
        });
        setLoading(false);
      }
    };
    handleFetchMessages.current();
    () => handleFetchMessages.current;
  }, [conversation]);

  const sendMessage = async (message: string) => {
    try {
      await connection?.invoke("SendMessage", {
        conversationId: conversation?.id,
        senderId: profile?.id,
        content: message,
      });
      form.setFieldValue("message", "");
    } catch (error) {
      console.log(error);
      notification.error({
        message: (error as IResponse<unknown>).message,
      });
    }
  };

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
        <div className="flex flex-col flex-1 gap-2 max-h-[454px] mb-4">
          <List loading={loading}>
            {messages && messages.length > 0 && (
              <VirtualList
                width="100%"
                height={ContainerHeight}
                itemCount={messages.length}
                itemSize={46}
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                scrollDirection={"vertical" as any}
                className="!overflow-x-hidden"
                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                scrollToAlignment={"end" as any}
                scrollToIndex={messages.length - 1}
                renderItem={({ index, style }) => {
                  const message = messages[index];
                  return (
                    <div key={message.id} className="px-4 mt-2" style={style}>
                      <MessageItem isReceiver={message.userId !== profile?.id}>
                        {message.content}
                      </MessageItem>
                    </div>
                  );
                }}
              ></VirtualList>
            )}
          </List>
        </div>
        <Form
          form={form}
          onFinish={({ message }) => sendMessage(message)}
          className="!w-full"
        >
          <div className="flex gap-4">
            <Form.Item name="message" className="flex-1 chat-form">
              <Input placeholder="Enter message" size="large" />
            </Form.Item>
            <Form.Item className="chat-form">
              <Button size="large" type="primary" htmlType="submit">
                Send
              </Button>
            </Form.Item>
          </div>
        </Form>
      </div>
    </div>
  );
}
