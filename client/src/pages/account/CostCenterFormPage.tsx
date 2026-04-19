import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const CostCenterFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/cost-center', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Cost Center Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="name" label="Name"><Input placeholder="Enter Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="type" label="Type"><Select placeholder="Select Type" /></Form.Item>
          <Form.Item name="parent" label="Parent"><Select placeholder="Select Parent" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default CostCenterFormPage;
