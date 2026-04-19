import React from 'react';
import { Card, Form, Input, Switch, Button, message } from 'antd';

const IRDDetailsPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save IRD details:', values);
    message.success('IRD details saved successfully');
  };

  return (
    <Card title="IRD Details">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="taxpayerName" label="Taxpayer Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="panNo" label="PAN No" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="irdUsername" label="IRD Username" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="irdPassword" label="IRD Password" rules={[{ required: true }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="fiscalYear" label="Fiscal Year">
          <Input placeholder="e.g. 2081/82" />
        </Form.Item>
        <Form.Item name="isEnabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default IRDDetailsPage;
