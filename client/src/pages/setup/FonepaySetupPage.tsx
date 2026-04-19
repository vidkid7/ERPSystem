import React from 'react';
import { Card, Form, Input, Switch, Button, message } from 'antd';

const FonepaySetupPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save Fonepay setup:', values);
    message.success('Fonepay setup saved successfully');
  };

  return (
    <Card title="Fonepay Setup">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="merchantId" label="Merchant ID" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="secretKey" label="Secret Key" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="apiUrl" label="API URL" rules={[{ required: true }]}>
          <Input placeholder="e.g. https://dev.fonepay.com/api" />
        </Form.Item>
        <Form.Item name="isEnabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default FonepaySetupPage;
