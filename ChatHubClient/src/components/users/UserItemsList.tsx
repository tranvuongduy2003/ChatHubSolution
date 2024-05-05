import { User } from "@/interfaces/user.interface";
import { UserItemBox } from "./UserItemBox";
import { SkeletonUserItemBox } from "./SkeletonUserItemBox";
import { useConversationStore } from "@/stores/useConversationStore";
import VirtualList from "react-tiny-virtual-list";

export interface UserItemsListProps {
  loading: boolean;
  onSelectUser: (receiver: User) => Promise<void>;
}

export function UserItemsList({ loading, onSelectUser }: UserItemsListProps) {
  const { users } = useConversationStore((state) => state);

  return loading ? (
    Array(5)
      .fill(0)
      .map((_, idx) => <SkeletonUserItemBox key={idx} />)
  ) : users && users.length > 0 ? (
    <VirtualList
      width="100%"
      height={516}
      itemSize={96}
      itemCount={users.length}
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      scrollDirection={"vertical" as any}
      className="!overflow-x-hidden"
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      scrollToAlignment={"end" as any}
      scrollToIndex={users.length - 1}
      renderItem={({ index, style }) => {
        const user = users[index];
        return (
          <div key={user.id} className="px-4 mt-2" style={style}>
            <UserItemBox
              onClick={() => onSelectUser(user)}
              name={user.name}
              lastMessage={""}
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
