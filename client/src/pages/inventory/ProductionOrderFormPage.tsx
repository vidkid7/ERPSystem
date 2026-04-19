import React, { useState } from 'react';
import { Card, Form, Input, InputNumber, DatePicker, Button, Space, Select } from 'antd';
import { useNavigate } from 'react-router-dom';
import api from '../../services/api';

const ProductionOrderFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/inventory/production-orders', values); navigate(-1); }
    catch (e) {} finally { setLoading(false); }
  };
  return (
    <Card title="New Production Order">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item name="product" label="Product" rules={[{ required: true, message: 'Required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="quantity" label="Quantity" rules={[{ required: true, message: 'Required' }]}>
          <InputNumber style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="bomRef" label="BOM Reference" rules={[{ required: true, message: 'Required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="startDate" label="Start Date" rules={[{ required: true, message: 'Required' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="endDate" label="End Date" rules={[]}>
          <DatePicker style={{ width: '100%' }} />
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
export default ProductionOrderFormPage;
