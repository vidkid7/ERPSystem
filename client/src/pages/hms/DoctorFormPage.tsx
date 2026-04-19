import React from 'react';
import { Card, Form, Input, Select, Button, Space, TimePicker } from 'antd';
import api from '../../services/api';

const DoctorFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/hms/doctor', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Doctor Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Name" name="name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Specialization" name="specialization" rules={[{ required: true }]}>
          <Select placeholder="Select specialization" />
        </Form.Item>
        <Form.Item label="Qualification" name="qualification">
          <Input />
        </Form.Item>
        <Form.Item label="Contact" name="contact">
          <Input />
        </Form.Item>
        <Form.Item label="Schedule Start" name="scheduleStart">
          <TimePicker format="HH:mm" style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="Schedule End" name="scheduleEnd">
          <TimePicker format="HH:mm" style={{ width: '100%' }} />
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
export default DoctorFormPage;
