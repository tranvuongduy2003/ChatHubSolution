import { Button, Card, Form, Input } from "antd";
import { useForm } from "antd/es/form/Form";
import { useContext } from "react";
import { AuthContext } from "../../context/AuthProvider";
import { LoginPayload } from "../../interfaces/auth.interface";
import { useAppStore } from "../../stores/useAppStore";

export function LoginPage() {
  const [form] = useForm<LoginPayload>();

  const { logIn } = useContext(AuthContext);

  const { isLoading } = useAppStore((state) => state);

  return (
    <div className="relative w-screen">
      <Card
        title="Login"
        size="default"
        className="absolute top-1/2 left-1/2 -translate-x-1/2 translate-y-1/2"
      >
        <Form form={form} onFinish={logIn} className="w-[300px]">
          <Form.Item>
            <Input placeholder="Enter your email" />
          </Form.Item>
          <Form.Item>
            <Input type="password" placeholder="Enter your password" />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              loading={isLoading}
              htmlType="submit"
              className="w-full"
            >
              Log in
            </Button>
          </Form.Item>
          <a href="/auth/sign-up" className="hover:text-blue-300">
            Register now!
          </a>
        </Form>
      </Card>
    </div>
  );
}
