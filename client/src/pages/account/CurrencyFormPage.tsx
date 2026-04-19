import React, { useState } from 'react';
import { Card, Form, Input, Button, Row, Col, message } from 'antd';
import api from '../../services/api';
const CurrencyFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/currency', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Currency Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="currencyName" label="Currency Name"><Input placeholder="Enter Currency Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="symbol" label="Symbol"><Input placeholder="Enter Symbol" /></Form.Item>
          <Form.Item name="rate" label="Rate"><Input placeholder="Enter Rate" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default CurrencyFormPage;
