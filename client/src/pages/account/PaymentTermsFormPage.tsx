import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const PaymentTermsFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/payment-terms', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Payment Terms Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="termsName" label="Terms Name"><Input placeholder="Enter Terms Name" /></Form.Item>
          <Form.Item name="days" label="Days"><Input placeholder="Enter Days" /></Form.Item>
          <Form.Item name="discountPercent" label="Discount %"><Input placeholder="Enter Discount %" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default PaymentTermsFormPage;
