import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { ProductGroup } from '../../types';

const ProductFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  const [loading, setLoading] = useState(false);
  const [groups, setGroups] = useState<ProductGroup[]>([]);

  useEffect(() => {
    api.get('/inventory/productgroup').then((res) => {
      setGroups(res.data.data || []);
    });
    if (isEdit) {
      api.get(`/inventory/product/${id}`).then((res) => {
        form.setFieldsValue(res.data.data || res.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.put(`/inventory/product/${id}`, { ...values, id: Number(id) });
        message.success('Product updated successfully');
      } else {
        await api.post('/inventory/product', values);
        message.success('Product created successfully');
      }
      navigate('/inventory/products');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage
      title={isEdit ? 'Edit Product' : 'New Product'}
      loading={loading}
      onSubmit={handleSubmit}
      backPath="/inventory/products"
    >
      <Form form={form} layout="vertical" initialValues={{ isActive: true, taxRate: 0, reorderLevel: 0 }}>
        <Form.Item name="name" label="Name" rules={[{ required: true, message: 'Name is required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Code is required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="alias" label="Alias">
          <Input />
        </Form.Item>
        <Form.Item name="productGroupId" label="Product Group" rules={[{ required: true, message: 'Group is required' }]}>
          <Select
            placeholder="Select group"
            showSearch
            optionFilterProp="children"
            options={groups.map((g) => ({ label: g.name, value: g.id }))}
          />
        </Form.Item>
        <Form.Item name="hsnCode" label="HSN Code">
          <Input />
        </Form.Item>
        <Form.Item name="purchaseRate" label="Purchase Rate">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="salesRate" label="Sales Rate">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="mrp" label="MRP">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="unit" label="Unit" rules={[{ required: true, message: 'Unit is required' }]}>
          <Input placeholder="e.g. Pcs, Kg, Ltr" />
        </Form.Item>
        <Form.Item name="taxRate" label="Tax Rate (%)">
          <InputNumber style={{ width: '100%' }} min={0} max={100} precision={2} />
        </Form.Item>
        <Form.Item name="reorderLevel" label="Reorder Level">
          <InputNumber style={{ width: '100%' }} min={0} />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default ProductFormPage;
