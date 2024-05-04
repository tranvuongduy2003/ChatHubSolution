import { MouseEventHandler } from "react";

export interface UserItemBoxProps {
  name: string;
  lastMessage: string;
  status: string;
  isActive?: boolean;
  onClick?: MouseEventHandler<HTMLDivElement>;
}

export function UserItemBox({
  name,
  lastMessage,
  status,
  isActive = false,
  onClick,
}: UserItemBoxProps) {
  return (
    <div
      onClick={onClick}
      className={
        "flex items-center w-full gap-4 p-4 rounded-lg cursor-pointer hover:bg-slate-100"
      }
      style={{
        backgroundColor: isActive ? "rgb(241 245 249)" : "",
      }}
    >
      <img
        src="https://i.pravatar.cc/300"
        alt="avatar"
        className="w-16 h-16 rounded-full"
      />
      <div className="flex flex-col items-start justify-between gap-2 w-[calc(100%-104px)]">
        <h3 className="text-lg font-semibold text-gray-800">{name}</h3>
        <h4 className="w-full text-base text-gray-400 truncate line-clamp-1 text-ellipsis">
          {lastMessage}
        </h4>
      </div>
      {status === "ONLINE" && (
        <div className="w-2 h-2 bg-green-400 rounded-full"></div>
      )}
    </div>
  );
}
