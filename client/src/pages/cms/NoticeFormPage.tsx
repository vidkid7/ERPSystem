import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const NoticeFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/notice/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/notice/${id}`, values);
        message.success('Notice updated');
      } else {
        await api.post('/cms/notice', values);
        message.success('Notice created');
      }
      navigate('/cms/notices');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Notice' : 'New Notice'} loading={loading} onSubmit={handleSubmit} backPath="/cms/notices">
      <Form form={form} layout="vertical" initialValues={{ isPinned: false }}>
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="content" label="Content" rules={[{ required: true, message: 'Please enter content' }]}>
          <TextArea rows={5} />
        </Form.Item>
        <Form.Item name="type" label="Type">
          <Select options={[
            { value: 'General', label: 'General' },
            { value: 'Urgent', label: 'Urgent' },
            { value: 'Event', label: 'Event' },
            { value: 'Holiday', label: 'Holiday' },
          ]} />
        </Form.Item>
        <Form.Item name="publishDate" label="Publish Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="expiryDate" label="Expiry Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="isPinned" label="Pinned" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default NoticeFormPage;
