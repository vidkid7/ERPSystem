import React, { useEffect, useState } from 'react';
import { Form, Input, DatePicker, Table, Button, Typography, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { Title } = Typography;

interface DispatchItem {
  key: string;
  itemName: string;
  quantity: number;
  unit: string;
}

const DispatchOrderFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [items, setItems] = useState<DispatchItem[]>([]);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/inventory/dispatch-order/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue(d);
        setItems(d?.items || []);
      });
    }
  }, [id]);

  const addItem = () => {
    setItems(prev => [...prev, { key: Date.now().toString(), itemName: '', quantity: 0, unit: '' }]);
  };

  const removeItem = (key: string) => {
    setItems(prev => prev.filter(i => i.key !== key));
  };

  const itemColumns = [
    { title: 'Item Name', dataIndex: 'itemName', key: 'itemName' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', width: 100 },
    { title: 'Unit', dataIndex: 'unit', key: 'unit', width: 90 },
    {
      title: '', key: 'action', width: 50,
      render: (_: unknown, record: DispatchItem) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeItem(record.key)} />
      ),
    },
  ];

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      const payload = { ...values, items };
      if (isEdit) {
        await api.post(`/inventory/dispatch-order/${id}`, payload);
        message.success('Dispatch order updated');
      } else {
        await api.post('/inventory/dispatch-order', payload);
        message.success('Dispatch order created');
      }
      navigate('/inventory/dispatch-orders');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Dispatch Order' : 'New Dispatch Order'} loading={loading} onSubmit={handleSubmit} backPath="/inventory/dispatch-orders">
      <Form form={form} layout="vertical">
        <Form.Item name="customerName" label="Customer" rules={[{ required: true, message: 'Please enter customer' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="orderRef" label="Order Reference">
          <Input />
        </Form.Item>
        <Form.Item name="vehicle" label="Vehicle">
          <Input />
        </Form.Item>
        <Form.Item name="driver" label="Driver">
          <Input />
        </Form.Item>
        <Form.Item name="dispatchDate" label="Dispatch Date" rules={[{ required: true, message: 'Please select dispatch date' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks">
          <Input.TextArea rows={2} />
        </Form.Item>
      </Form>
      <Title level={5} style={{ marginTop: 16 }}>Items</Title>
      <Table columns={itemColumns} dataSource={items} rowKey="key" size="small" pagination={false} scroll={{ x: 400 }} />
      <Button type="dashed" icon={<PlusOutlined />} onClick={addItem} style={{ marginTop: 8 }}>Add Item</Button>
    </FormPage>
  );
};

export default DispatchOrderFormPage;
