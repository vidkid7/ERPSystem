import React from 'react';
import { Card, Form, Input, Switch, Button, message, InputNumber } from 'antd';

const EmailSetupPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save email setup:', values);
    message.success('Email setup saved successfully');
  };

  return (
    <Card title="Email Setup">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="smtpHost" label="SMTP Host" rules={[{ required: true }]}>
          <Input placeholder="e.g. smtp.gmail.com" />
        </Form.Item>
        <Form.Item name="smtpPort" label="SMTP Port" rules={[{ required: true }]}>
          <InputNumber min={1} max={65535} style={{ width: '100%' }} placeholder="e.g. 587" />
        </Form.Item>
        <Form.Item name="email" label="Email Address" rules={[{ required: true, type: 'email' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="password" label="Password" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="displayName" label="Display Name">
          <Input placeholder="e.g. ERP System" />
        </Form.Item>
        <Form.Item name="isEnabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default EmailSetupPage;
