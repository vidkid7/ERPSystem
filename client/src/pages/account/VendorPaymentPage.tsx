import React from 'react';
import { Card, Form, Input, Button, Select, Space } from 'antd';

const VendorPaymentPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = (values: Record<string, unknown>) => {
    console.log(values);
  };

  return (
    <Card title="Vendor Payment">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item name="name" label="Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} />
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

export default VendorPaymentPage;
