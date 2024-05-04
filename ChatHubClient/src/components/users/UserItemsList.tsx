import { User } from "@/interfaces/user.interface";
import { UserItemBox } from "./UserItemBox";
import { SkeletonUserItemBox } from "./SkeletonUserItemBox";
import { useConversationStore } from "@/stores/useConversationStore";

export interface UserItemsListProps {
  loading: boolean;
  onSelectUser: (receiver: User) => Promise<void>;
}

export function UserItemsList({ loading, onSelectUser }: UserItemsListProps) {
  const { users } = useConversationStore((state) => state);

  return (
    <div className="flex flex-col gap-4 ">
      {loading ? (
        Array(5)
          .fill(0)
          .map((_, idx) => <SkeletonUserItemBox key={idx} />)
      ) : users && users.length > 0 ? (
        users.map((user, idx) => (
          <UserItemBox
            key={idx}
            onClick={() => onSelectUser(user)}
            name={user.name}
            lastMessage={""}
            status={"ONLINE"}
          />
        ))
      ) : (
        <></>
      )}
    </div>
  );
}
