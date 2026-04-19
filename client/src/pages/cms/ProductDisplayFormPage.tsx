import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const ProductDisplayFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/product-display/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/product-display/${id}`, values);
        message.success('Product display updated');
      } else {
        await api.post('/cms/product-display', values);
        message.success('Product display created');
      }
      navigate('/cms/product-displays');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Product Display' : 'New Product Display'} loading={loading} onSubmit={handleSubmit} backPath="/cms/product-displays">
      <Form form={form} layout="vertical" initialValues={{ showPrice: true, isActive: true }}>
        <Form.Item name="product" label="Product" rules={[{ required: true, message: 'Please enter product' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="displayPosition" label="Display Position">
          <InputNumber style={{ width: '100%' }} min={1} />
        </Form.Item>
        <Form.Item name="imageUrl" label="Image URL">
          <Input />
        </Form.Item>
        <Form.Item name="showPrice" label="Show Price" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default ProductDisplayFormPage;
