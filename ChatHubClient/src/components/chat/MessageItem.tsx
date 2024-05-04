import { ReactNode } from "react";

export interface MessageItemProps {
  isReceiver?: boolean;
  children: ReactNode;
}

export function MessageItem({
  isReceiver = false,
  children,
}: MessageItemProps) {
  return (
    <div
      className="flex items-center gap-4"
      style={{
        justifyContent: isReceiver ? "flex-start" : "flex-end",
        paddingRight: isReceiver ? 200 : 0,
        paddingLeft: isReceiver ? 0 : 200,
      }}
    >
      {isReceiver && (
        <img
          src="https://i.pravatar.cc/300"
          alt="avatar"
          className="w-8 h-8 rounded-full"
        />
      )}
      <span
        className="p-2 text-gray-800 rounded-md"
        style={{
          backgroundColor: isReceiver ? "rgb(203 213 225)" : "rgb(37 99 235)",
          color: isReceiver ? "rgb(31 41 55)" : "white",
        }}
      >
        {children}
      </span>
    </div>
  );
}
