import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const ProjectFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/project', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Project Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="name" label="Name"><Input placeholder="Enter Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="client" label="Client"><Select placeholder="Select Client" /></Form.Item>
          <Form.Item name="startDate" label="Start Date"><Input placeholder="Enter Start Date" /></Form.Item>
          <Form.Item name="endDate" label="End Date"><Input placeholder="Enter End Date" /></Form.Item>
          <Form.Item name="budget" label="Budget"><Input placeholder="Enter Budget" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default ProjectFormPage;
