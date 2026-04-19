import React from 'react';
import { Form, Input, Button, Card, Typography, message } from 'antd';
import { UserOutlined, LockOutlined } from '@ant-design/icons';
import { useDispatch, useSelector } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { login } from '../../store/slices/authSlice';
import type { RootState, AppDispatch } from '../../store';
import type { LoginRequest } from '../../types';

const { Title, Text } = Typography;

const LoginPage: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const navigate = useNavigate();
  const { loading, error } = useSelector((s: RootState) => s.auth);

  const onFinish = async (values: LoginRequest) => {
    const result = await dispatch(login(values));
    if (login.fulfilled.match(result)) {
      message.success('Login successful');
      navigate('/dashboard');
    }
  };

  return (
    <div style={{ minHeight: '100vh', display: 'flex', alignItems: 'center', justifyContent: 'center', background: '#f0f2f5' }}>
      <Card style={{ width: 400, boxShadow: '0 4px 12px rgba(0,0,0,0.1)' }}>
        <div style={{ textAlign: 'center', marginBottom: 24 }}>
          <Title level={2}>Ultimate ERP</Title>
          <Text type="secondary">Sign in to your account</Text>
        </div>
        {error && <Text type="danger" style={{ display: 'block', marginBottom: 16, textAlign: 'center' }}>{error}</Text>}
        <Form name="login" onFinish={onFinish} size="large">
          <Form.Item name="username" rules={[{ required: true, message: 'Please enter username' }]}>
            <Input prefix={<UserOutlined />} placeholder="Username" />
          </Form.Item>
          <Form.Item name="password" rules={[{ required: true, message: 'Please enter password' }]}>
            <Input.Password prefix={<LockOutlined />} placeholder="Password" />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" loading={loading} block>Sign In</Button>
          </Form.Item>
        </Form>
      </Card>
    </div>
  );
};

export default LoginPage;
