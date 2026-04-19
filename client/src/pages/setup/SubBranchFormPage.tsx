import React from 'react';
import { Card, Form, Input, Select, Button, Space } from 'antd';
import api from '../../services/api';

const SubBranchFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/setup/sub-branch', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Sub-Branch Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Sub-Branch Name" name="subBranchName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Code" name="code" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Parent Branch" name="parentBranch" rules={[{ required: true }]}>
          <Select placeholder="Select parent branch" />
        </Form.Item>
        <Form.Item label="Contact" name="contact">
          <Input />
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
export default SubBranchFormPage;
