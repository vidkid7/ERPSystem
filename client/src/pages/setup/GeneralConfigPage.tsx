import React from 'react';
import { Card, Form, Input, Select, Switch, Button, message } from 'antd';

const { Option } = Select;

const GeneralConfigPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save config:', values);
    message.success('Configuration saved successfully');
  };

  return (
    <Card title="General Configuration">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="companyName" label="Company Name">
          <Input />
        </Form.Item>
        <Form.Item name="fiscalYearStart" label="Fiscal Year Start">
          <Input placeholder="e.g. 2081-04-01" />
        </Form.Item>
        <Form.Item name="dateFormat" label="Date Format">
          <Select placeholder="Select date format">
            <Option value="YYYY-MM-DD">YYYY-MM-DD</Option>
            <Option value="DD/MM/YYYY">DD/MM/YYYY</Option>
            <Option value="MM/DD/YYYY">MM/DD/YYYY</Option>
          </Select>
        </Form.Item>
        <Form.Item name="timeZone" label="Time Zone">
          <Select placeholder="Select time zone">
            <Option value="Asia/Kathmandu">Asia/Kathmandu (NPT)</Option>
            <Option value="UTC">UTC</Option>
            <Option value="Asia/Kolkata">Asia/Kolkata (IST)</Option>
          </Select>
        </Form.Item>
        <Form.Item name="enableAuditLog" label="Enable Audit Log" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Form.Item name="enableRateLimit" label="Enable Rate Limiting" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default GeneralConfigPage;
