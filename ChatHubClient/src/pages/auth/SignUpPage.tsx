import { Button, Card, Form, Input } from "antd";
import { useContext } from "react";
import { AuthContext } from "../../context/AuthProvider";
import { LoginPayload } from "../../interfaces/auth.interface";
import { useForm } from "antd/es/form/Form";
import { useAppStore } from "../../stores/useAppStore";

export function SignUpPage() {
  const [form] = useForm<LoginPayload>();

  const { logIn } = useContext(AuthContext);

  const { isLoading } = useAppStore((state) => state);

  return (
    <div className="relative w-screen">
      <Card
        title="Sign up"
        size="default"
        className="absolute top-1/2 left-1/2 -translate-x-1/2 translate-y-1/2"
      >
        <Form form={form} onFinish={logIn} className="w-[300px]">
          <Form.Item name="fullName">
            <Input placeholder="Enter your fullname" />
          </Form.Item>
          <Form.Item name="email">
            <Input placeholder="Enter your email" />
          </Form.Item>
          <Form.Item name="password">
            <Input type="password" placeholder="Enter your password" />
          </Form.Item>
          <Form.Item>
            <Button
              type="primary"
              loading={isLoading}
              htmlType="submit"
              className="w-full"
            >
              Sign up
            </Button>
          </Form.Item>
          <a href="/auth/login" className="hover:text-blue-300">
            Login now!
          </a>
        </Form>
      </Card>
    </div>
  );
}
