import React from 'react';
import { Card, Form, Input, Select, DatePicker, InputNumber, Button, Space } from 'antd';
import api from '../../services/api';

const EmergencyPatientPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/hms/emergency-patient', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Emergency Patient">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Patient Name" name="patientName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Emergency Type" name="emergencyType" rules={[{ required: true }]}>
          <Select>
            <Select.Option value="Trauma">Trauma</Select.Option>
            <Select.Option value="Cardiac">Cardiac</Select.Option>
            <Select.Option value="Respiratory">Respiratory</Select.Option>
            <Select.Option value="Other">Other</Select.Option>
          </Select>
        </Form.Item>
        <Form.Item label="Doctor" name="doctor">
          <Select placeholder="Select doctor" />
        </Form.Item>
        <Form.Item label="Vitals" name="vitals">
          <Input.TextArea rows={2} placeholder="BP, Pulse, Temperature, SpO2..." />
        </Form.Item>
        <Form.Item label="Admission Time" name="admissionTime" rules={[{ required: true }]}>
          <DatePicker showTime style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item>
          <Space>
            <Button type="primary" htmlType="submit" danger>Admit Emergency</Button>
            <Button onClick={() => form.resetFields()}>Reset</Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  );
};
export default EmergencyPatientPage;
