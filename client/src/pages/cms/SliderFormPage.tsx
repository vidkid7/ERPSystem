import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const SliderFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/slider/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/slider/${id}`, values);
        message.success('Slider updated');
      } else {
        await api.post('/cms/slider', values);
        message.success('Slider created');
      }
      navigate('/cms/sliders');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Slider' : 'New Slider'} loading={loading} onSubmit={handleSubmit} backPath="/cms/sliders">
      <Form form={form} layout="vertical" initialValues={{ isActive: true, duration: 5000 }}>
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="imageUrl" label="Image URL" rules={[{ required: true, message: 'Please enter image URL' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="linkUrl" label="Link URL">
          <Input />
        </Form.Item>
        <Form.Item name="order" label="Order">
          <InputNumber style={{ width: '100%' }} min={1} />
        </Form.Item>
        <Form.Item name="duration" label="Duration (ms)">
          <InputNumber style={{ width: '100%' }} min={1000} step={500} />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default SliderFormPage;
