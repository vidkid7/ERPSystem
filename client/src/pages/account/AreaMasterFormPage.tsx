import React, { useState } from 'react';
import { Card, Form, Input, Button, Select, Row, Col, message } from 'antd';
import api from '../../services/api';
const AreaMasterFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/account/area-master', values); message.success('Saved successfully'); form.resetFields(); }
    catch { message.error('Save failed'); } finally { setLoading(false); }
  };
  return (
    <Card title="Area Master Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Row gutter={16}>
          <Form.Item name="areaName" label="Area Name"><Input placeholder="Enter Area Name" /></Form.Item>
          <Form.Item name="code" label="Code"><Input placeholder="Enter Code" /></Form.Item>
          <Form.Item name="salesman" label="Salesman"><Select placeholder="Select Salesman" /></Form.Item>
          <Form.Item name="region" label="Region"><Select placeholder="Select Region" /></Form.Item>
        </Row>
        <Form.Item><Button type="primary" htmlType="submit" loading={loading}>Save</Button></Form.Item>
      </Form>
    </Card>
  );
};
export default AreaMasterFormPage;
