import React, { useEffect, useState } from 'react';
import { Form, Input, DatePicker, Table, Button, Typography, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { Title } = Typography;

interface RequisitionItem {
  key: string;
  itemName: string;
  quantity: number;
  unit: string;
  remarks: string;
}

const MaterialRequisitionFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [items, setItems] = useState<RequisitionItem[]>([]);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/inventory/material-requisition/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue(d);
        setItems(d?.items || []);
      });
    }
  }, [id]);

  const addItem = () => {
    setItems(prev => [...prev, { key: Date.now().toString(), itemName: '', quantity: 0, unit: '', remarks: '' }]);
  };

  const removeItem = (key: string) => {
    setItems(prev => prev.filter(i => i.key !== key));
  };

  const itemColumns = [
    { title: 'Item Name', dataIndex: 'itemName', key: 'itemName' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', width: 100 },
    { title: 'Unit', dataIndex: 'unit', key: 'unit', width: 90 },
    { title: 'Remarks', dataIndex: 'remarks', key: 'remarks' },
    {
      title: '', key: 'action', width: 50,
      render: (_: unknown, record: RequisitionItem) => (
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
        await api.post(`/inventory/material-requisition/${id}`, payload);
        message.success('Material requisition updated');
      } else {
        await api.post('/inventory/material-requisition', payload);
        message.success('Material requisition created');
      }
      navigate('/inventory/material-requisitions');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Material Requisition' : 'New Material Requisition'} loading={loading} onSubmit={handleSubmit} backPath="/inventory/material-requisitions">
      <Form form={form} layout="vertical">
        <Form.Item name="department" label="Department" rules={[{ required: true, message: 'Please enter department' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="requisitionDate" label="Date" rules={[{ required: true, message: 'Please select date' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="requiredByDate" label="Required By Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks">
          <Input.TextArea rows={2} />
        </Form.Item>
      </Form>
      <Title level={5} style={{ marginTop: 16 }}>Items</Title>
      <Table columns={itemColumns} dataSource={items} rowKey="key" size="small" pagination={false} scroll={{ x: 550 }} />
      <Button type="dashed" icon={<PlusOutlined />} onClick={addItem} style={{ marginTop: 8 }}>Add Item</Button>
    </FormPage>
  );
};

export default MaterialRequisitionFormPage;
