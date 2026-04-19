import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const NarrationFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/narration', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Narration Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="narrationText" label="Narration Text"><Input placeholder="Enter Narration Text" /></Form.Item>
          <Form.Item name="type" label="Type"><Select placeholder="Select Type" /></Form.Item>
          <Form.Item name="default" label="Default"><Select placeholder="Select Default" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default NarrationFormPage;
