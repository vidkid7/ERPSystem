import React, { useEffect, useState } from 'react';
import { Form, Input, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const VideoFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/video/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/video/${id}`, values);
        message.success('Video updated');
      } else {
        await api.post('/cms/video', values);
        message.success('Video created');
      }
      navigate('/cms/videos');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Video' : 'New Video'} loading={loading} onSubmit={handleSubmit} backPath="/cms/videos">
      <Form form={form} layout="vertical">
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="videoUrl" label="Video URL" rules={[{ required: true, message: 'Please enter video URL' }]}>
          <Input placeholder="YouTube, Vimeo or direct URL" />
        </Form.Item>
        <Form.Item name="thumbnailUrl" label="Thumbnail URL">
          <Input />
        </Form.Item>
        <Form.Item name="category" label="Category">
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default VideoFormPage;
