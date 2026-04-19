import React from 'react';
import { Card, Form, Input, InputNumber, Button, Space } from 'antd';
import api from '../../services/api';

const DocumentTypeFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/setup/document-type', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Document Type Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Document Type" name="documentType" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Code" name="code" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Prefix" name="prefix">
          <Input />
        </Form.Item>
        <Form.Item label="Starting Number" name="startingNumber">
          <InputNumber min={1} style={{ width: '100%' }} />
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
export default DocumentTypeFormPage;
