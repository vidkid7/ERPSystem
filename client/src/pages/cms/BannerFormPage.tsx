import React, { useEffect, useState } from 'react';
import { Form, Input, Select, Switch, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const BannerFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/banner/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/banner/${id}`, values);
        message.success('Banner updated');
      } else {
        await api.post('/cms/banner', values);
        message.success('Banner created');
      }
      navigate('/cms/banners');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Banner' : 'New Banner'} loading={loading} onSubmit={handleSubmit} backPath="/cms/banners">
      <Form form={form} layout="vertical" initialValues={{ isActive: true }}>
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="imageUrl" label="Image URL">
          <Input />
        </Form.Item>
        <Form.Item name="linkUrl" label="Link URL">
          <Input />
        </Form.Item>
        <Form.Item name="position" label="Position">
          <Select options={[
            { value: 'Top', label: 'Top' },
            { value: 'Bottom', label: 'Bottom' },
            { value: 'Sidebar', label: 'Sidebar' },
            { value: 'Popup', label: 'Popup' },
          ]} />
        </Form.Item>
        <Form.Item name="startDate" label="Schedule Start">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="endDate" label="Schedule End">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default BannerFormPage;
