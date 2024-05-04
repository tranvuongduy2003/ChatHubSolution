import { Button, Card, Form, Input } from "antd";
import { useForm } from "antd/es/form/Form";
import { useContext } from "react";
import { AuthContext } from "@/context/AuthProvider";
import { LoginPayload } from "@/interfaces/auth.interface";
import { useAppStore } from "@/stores/useAppStore";

export function LoginPage() {
  const [form] = useForm<LoginPayload>();

  const { logIn } = useContext(AuthContext);

  const { isLoading } = useAppStore((state) => state);

  return (
    <div className="relative w-screen">
      <Card
        title={<h2 className="text-2xl font-bold text-center">Login</h2>}
        size="default"
        className="absolute -translate-x-1/2 translate-y-1/2 border-0 top-1/2 left-1/2 bg-slate-100"
      >
        <Form size="large" form={form} onFinish={logIn} className="w-[300px]">
          <Form.Item name="email">
            <Input placeholder="Enter your email" className="border-0" />
          </Form.Item>
          <Form.Item name="password">
            <Input
              type="password"
              placeholder="Enter your password"
              className="border-0"
            />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              loading={isLoading}
              htmlType="submit"
              className="w-full border-0"
            >
              Log in
            </Button>
          </Form.Item>
          <div className="flex items-center justify-center">
            <a
              href="/auth/sign-up"
              className="text-lg text-blue-400 hover:text-blue-300"
            >
              Register now!
            </a>
          </div>
        </Form>
      </Card>
    </div>
  );
}
