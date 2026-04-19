import React from 'react';
import { Card, Form, Input, Button, Space } from 'antd';
import api from '../../services/api';

const BranchFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/setup/branch', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Branch Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Branch Name" name="branchName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Code" name="code" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Address" name="address">
          <Input.TextArea rows={2} />
        </Form.Item>
        <Form.Item label="Contact" name="contact">
          <Input />
        </Form.Item>
        <Form.Item label="Email" name="email">
          <Input type="email" />
        </Form.Item>
        <Form.Item>
          <Space>
            <Button type="primary" htmlType="submit">Save</Button>
            <Button onClick={() => form.resetFields()}>Reset</Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  );
};
export default BranchFormPage;
