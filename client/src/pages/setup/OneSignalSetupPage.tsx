import React from 'react';
import { Card, Form, Input, Switch, Button, message } from 'antd';

const OneSignalSetupPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save OneSignal setup:', values);
    message.success('OneSignal setup saved successfully');
  };

  return (
    <Card title="OneSignal Push Notification Setup">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="appId" label="App ID" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="apiKey" label="API Key" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="isEnabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default OneSignalSetupPage;
