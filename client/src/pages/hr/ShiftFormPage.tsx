import React from 'react';
import { Card, Form, Input, InputNumber, Checkbox, Button, Space, TimePicker } from 'antd';
import api from '../../services/api';

const WEEKDAYS = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];

const ShiftFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/hr/shift', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Shift Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Shift Name" name="shiftName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Start Time" name="startTime" rules={[{ required: true }]}>
          <TimePicker format="HH:mm" style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="End Time" name="endTime" rules={[{ required: true }]}>
          <TimePicker format="HH:mm" style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="Break Minutes" name="breakMinutes">
          <InputNumber min={0} style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="Working Days" name="workingDays">
          <Checkbox.Group options={WEEKDAYS} />
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
export default ShiftFormPage;
