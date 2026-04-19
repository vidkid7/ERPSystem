import React, { useState } from 'react';
import { Card, Form, Input, Button, Row, Col, message } from 'antd';
import api from '../../services/api';
const BankFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/bank', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Bank Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="bankName" label="Bank Name"><Input placeholder="Enter Bank Name" /></Form.Item>
          <Form.Item name="accountNo" label="Account No"><Input placeholder="Enter Account No" /></Form.Item>
          <Form.Item name="branch" label="Branch"><Input placeholder="Enter Branch" /></Form.Item>
          <Form.Item name="openingBalance" label="Opening Balance"><Input placeholder="Enter Opening Balance" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default BankFormPage;
