import React, { useState } from 'react';
import { Card, Form, Input, InputNumber, DatePicker, Button, Space, Select } from 'antd';
import { useNavigate } from 'react-router-dom';
import api from '../../services/api';

const BOMFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/inventory/boms', values); navigate(-1); }
    catch (e) {} finally { setLoading(false); }
  };
  return (
    <Card title="New Bill of Materials">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item name="finishedProduct" label="Finished Product" rules={[{ required: true, message: 'Required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="version" label="Version" rules={[]}>
          <Input />
        </Form.Item>
        <Form.Item name="yieldQty" label="Yield Quantity" rules={[]}>
          <InputNumber style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks" rules={[]}>
          <Input.TextArea rows={4} />
        </Form.Item>
        <Form.Item>
          <Space>
            <Button type="primary" htmlType="submit" loading={loading}>Save</Button>
            <Button onClick={() => navigate(-1)}>Cancel</Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  );
};
export default BOMFormPage;
