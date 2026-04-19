import React from 'react';
import { Card, Form, Input, DatePicker, Select, Button, Space } from 'antd';
import api from '../../services/api';

const HolidayFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/hr/holiday', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Holiday Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Holiday Name" name="holidayName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Date" name="date" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="Type" name="type" rules={[{ required: true }]}>
          <Select>
            <Select.Option value="Public">Public</Select.Option>
            <Select.Option value="Company">Company</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item label="Description" name="description">
          <Input.TextArea rows={2} />
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
export default HolidayFormPage;
