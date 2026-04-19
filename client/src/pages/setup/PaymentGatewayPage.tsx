import React from 'react';
import { Card, Form, Input, Select, Switch, Button, message } from 'antd';

const { Option } = Select;

const PaymentGatewayPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save payment gateway:', values);
    message.success('Payment gateway saved successfully');
  };

  return (
    <Card title="Payment Gateway">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="gatewayName" label="Gateway Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="apiKey" label="API Key" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="secretKey" label="Secret Key" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="mode" label="Mode" rules={[{ required: true }]}>
          <Select placeholder="Select mode">
            <Option value="test">Test</Option>
            <Option value="live">Live</Option>
          </Select>
        </Form.Item>
        <Form.Item name="isEnabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default PaymentGatewayPage;
