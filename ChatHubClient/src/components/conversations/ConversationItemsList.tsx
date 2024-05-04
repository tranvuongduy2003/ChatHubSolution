import { User } from "@/interfaces/user.interface";
import { useConversationStore } from "@/stores/useConversationStore";
import { ConversationItemBox } from "./ConversationItemBox";
import { SkeletonConversationItemBox } from "./SkeletonConversationItemBox";

export interface ConversationItemsListProps {
  loading: boolean;
}

export function ConversationItemsList({ loading }: ConversationItemsListProps) {
  const { selectedReceiver, setSelectedReceiver, conversations } =
    useConversationStore((state) => state);

  return (
    <div className="flex flex-col gap-4 ">
      {loading ? (
        Array(5)
          .fill(0)
          .map((_, idx) => <SkeletonConversationItemBox key={idx} />)
      ) : conversations && conversations.length > 0 ? (
        conversations.map((conversation, idx) => (
          <ConversationItemBox
            key={idx}
            onClick={() =>
              setSelectedReceiver({
                id: conversation.receiverId,
                name: conversation.receiverName,
              } as User)
            }
            isActive={conversation.receiverId === selectedReceiver?.id}
            name={conversation.receiverName}
            lastMessage={conversation.lastMessage ?? ""}
            status={"ONLINE"}
          />
        ))
      ) : (
        <></>
      )}
    </div>
  );
}
