import React, { useEffect, useState } from 'react';
import { Form, Input, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const LabCategoryFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/lab/category/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/lab/category/${id}`, values);
        message.success('Lab category updated');
      } else {
        await api.post('/lab/category', values);
        message.success('Lab category created');
      }
      navigate('/lab/categories');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Lab Category' : 'New Lab Category'} loading={loading} onSubmit={handleSubmit} backPath="/lab/categories">
      <Form form={form} layout="vertical">
        <Form.Item name="categoryName" label="Category Name" rules={[{ required: true, message: 'Please enter category name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="department" label="Department">
          <Input />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default LabCategoryFormPage;
