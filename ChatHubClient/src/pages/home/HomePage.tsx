import { getConversationsByUserId } from "@/apis/conversations.api";
import { getUsers } from "@/apis/users.api";
import { ChatRoom } from "@/components/chat/ChatRoom";
import { EmptyChatRoom } from "@/components/chat/EmptyChatRoom";
import { ConversationItemsList } from "@/components/conversations/ConversationItemsList";
import { UserItemsList } from "@/components/users/UserItemsList";
import { AuthContext } from "@/context/AuthProvider";
import { IResponse } from "@/interfaces/response.interface";
import { User } from "@/interfaces/user.interface";
import { useAuthStore } from "@/stores/useAuthStore";
import { useConversationStore } from "@/stores/useConversationStore";
import { Button, Input, notification } from "antd";
import lodash from "lodash";
import { useCallback, useContext, useEffect, useRef, useState } from "react";

export function HomePage() {
  const { logOut } = useContext(AuthContext);
  const { profile } = useAuthStore((state) => state);
  const {
    selectedReceiver,
    setSelectedReceiver,
    users,
    setUsers,
    setConversations,
  } = useConversationStore((state) => state);

  const [loading, setLoading] = useState<boolean>(false);
  const [search, setSearch] = useState<string>("");

  const debounceFn = useCallback(
    lodash.debounce((value) => setSearch(value), 450),
    []
  );

  const handleFetchUsers = useRef<(() => Promise<void>) | null>(null);
  const handleFetchConversations = useRef<(() => Promise<void>) | null>(null);

  const onChatWithUser = async (receiver: User) => {
    setSearch("");
    setSelectedReceiver({ ...receiver });
  };

  useEffect(() => {
    handleFetchConversations.current = async () => {
      if (users && users.length > 0) return;
      setLoading(true);
      try {
        const { data } = await getConversationsByUserId(profile?.id as string);
        setConversations(data);
        setLoading(false);
      } catch (error: unknown) {
        console.log(error);
        notification.error({
          message: (error as IResponse<unknown>).message,
        });
        setLoading(false);
      }
    };
    handleFetchConversations.current();
    () => handleFetchConversations.current;
  }, [users]);

  useEffect(() => {
    handleFetchUsers.current = async () => {
      if (!search && search === "") {
        setUsers([]);
        return;
      }
      setLoading(true);
      try {
        const { data } = await getUsers(search);
        setUsers(data);
        setLoading(false);
      } catch (error: unknown) {
        console.log(error);
        notification.error({
          message: (error as IResponse<unknown>).message,
        });
        setLoading(false);
      }
    };
    handleFetchUsers.current();
    () => handleFetchUsers.current;
  }, [search]);

  return (
    <div className="w-screen h-screen p-10">
      <div className="flex items-center justify-center gap-8 mb-10">
        <h2 className="text-2xl leading-0">
          Welcome <span className="font-bold">{profile?.name}</span>
        </h2>
        <Button type="primary" className="font-semibold" onClick={logOut}>
          Log out
        </Button>
      </div>
      <div className="flex gap-8 h-[90%]">
        <div className="w-1/3 p-8 border-2 border-gray-200 border-solid rounded-lg">
          <Input
            placeholder="Find user"
            onChange={(e) => debounceFn(e.target.value)}
            size="large"
            className="mb-4"
            prefix={
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth="1.5"
                stroke="currentColor"
                className="w-4 h-4 text-gray-400"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M21 21l-5.197-5.197m0 0A7.5 7.5 0 105.196 5.196a7.5 7.5 0 0010.607 10.607z"
                />
              </svg>
            }
          />
          {users && users.length > 0 ? (
            <UserItemsList loading={loading} onSelectUser={onChatWithUser} />
          ) : (
            <ConversationItemsList loading={loading} />
          )}
        </div>
        <div className="flex flex-1">
          {!selectedReceiver ? <EmptyChatRoom /> : <ChatRoom />}
        </div>
      </div>
    </div>
  );
}
