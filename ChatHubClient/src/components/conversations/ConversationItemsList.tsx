import { User } from "@/interfaces/user.interface";
import { useConversationStore } from "@/stores/useConversationStore";
import { ConversationItemBox } from "./ConversationItemBox";
import { SkeletonConversationItemBox } from "./SkeletonConversationItemBox";
import VirtualList from "react-tiny-virtual-list";

export interface ConversationItemsListProps {
  loading: boolean;
}

export function ConversationItemsList({ loading }: ConversationItemsListProps) {
  const { selectedReceiver, setSelectedReceiver, conversations } =
    useConversationStore((state) => state);

  return loading ? (
    Array(5)
      .fill(0)
      .map((_, idx) => <SkeletonConversationItemBox key={idx} />)
  ) : conversations && conversations.length > 0 ? (
    <VirtualList
      width="100%"
      height={516}
      itemSize={96}
      itemCount={conversations.length}
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      scrollDirection={"vertical" as any}
      className="!overflow-x-hidden"
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      scrollToAlignment={"end" as any}
      scrollToIndex={conversations.length - 1}
      renderItem={({ index, style }) => {
        const conversation = conversations[index];
        return (
          <div key={conversation.id} className="px-4 mt-2" style={style}>
            <ConversationItemBox
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
          </div>
        );
      }}
    />
  ) : (
    <></>
  );
}
