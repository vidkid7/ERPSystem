import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const VendorFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/vendor', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Vendor Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="name" label="Name"><Input placeholder="Enter Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="group" label="Group"><Select placeholder="Select Group" /></Form.Item>
          <Form.Item name="contact" label="Contact"><Input placeholder="Enter Contact" /></Form.Item>
          <Form.Item name="email" label="Email"><Input placeholder="Enter Email" /></Form.Item>
          <Form.Item name="pan" label="PAN"><Input placeholder="Enter PAN" /></Form.Item>
          <Form.Item name="openingBalance" label="Opening Balance"><Input placeholder="Enter Opening Balance" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default VendorFormPage;
