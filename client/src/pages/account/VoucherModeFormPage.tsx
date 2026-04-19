import React, { useState } from 'react';
import { Card, Form, Input, Button, Row, Col, message } from 'antd';
import api from '../../services/api';
const VoucherModeFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/voucher-mode', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Voucher Mode Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="modeName" label="Mode Name"><Input placeholder="Enter Mode Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="type" label="Type"><Input placeholder="Enter Type" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default VoucherModeFormPage;
