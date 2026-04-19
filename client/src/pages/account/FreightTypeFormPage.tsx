import React, { useState } from 'react';
import { Card, Form, Input, Button, Row, Col, message } from 'antd';
import api from '../../services/api';
const FreightTypeFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/freight-type', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Freight Type Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="freightTypeName" label="Freight Type Name"><Input placeholder="Enter Freight Type Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="calculationType" label="Calculation Type"><Input placeholder="Enter Calculation Type" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default FreightTypeFormPage;
