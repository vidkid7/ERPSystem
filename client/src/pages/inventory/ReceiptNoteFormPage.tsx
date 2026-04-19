import React, { useState } from 'react';
import { Card, Form, Input, InputNumber, DatePicker, Button, Space, Select } from 'antd';
import { useNavigate } from 'react-router-dom';
import api from '../../services/api';

const ReceiptNoteFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const handleSubmit = async (values: any) => {
    setLoading(true);
    try { await api.post('/inventory/receipt-notes', values); navigate(-1); }
    catch (e) {} finally { setLoading(false); }
  };
  return (
    <Card title="New Receipt Note">
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item name="poRef" label="PO Reference" rules={[{ required: true, message: 'Required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="supplier" label="Supplier" rules={[{ required: true, message: 'Required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="date" label="Date" rules={[{ required: true, message: 'Required' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="godown" label="Godown" rules={[{ required: true, message: 'Required' }]}>
          <Input />
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
export default ReceiptNoteFormPage;
