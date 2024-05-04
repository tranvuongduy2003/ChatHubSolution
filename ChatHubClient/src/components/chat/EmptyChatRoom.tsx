import { Button, Input } from "antd";

export function EmptyChatRoom() {
  return (
    <div className="flex flex-col w-full border-2 border-gray-200 border-solid rounded-lg">
      <div className="w-full h-10 py-4 border-b-2 border-gray-200 border-solid"></div>
      <div className="flex flex-col flex-1 p-4">
        <div className="flex flex-col flex-1 gap-2"></div>
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
