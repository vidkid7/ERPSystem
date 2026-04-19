import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AssetCategoryFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/asset/category/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/asset/category/${id}`, values);
        message.success('Asset category updated');
      } else {
        await api.post('/asset/category', values);
        message.success('Asset category created');
      }
      navigate('/assets/categories');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Asset Category' : 'New Asset Category'} loading={loading} onSubmit={handleSubmit} backPath="/assets/categories">
      <Form form={form} layout="vertical">
        <Form.Item name="categoryName" label="Category Name" rules={[{ required: true, message: 'Please enter category name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="group" label="Group">
          <Input />
        </Form.Item>
        <Form.Item name="usefulLife" label="Useful Life (years)">
          <InputNumber style={{ width: '100%' }} min={1} max={99} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AssetCategoryFormPage;
