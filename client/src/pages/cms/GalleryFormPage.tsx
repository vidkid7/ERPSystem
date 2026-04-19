import React, { useEffect, useState } from 'react';
import { Form, Input, Switch, Upload, Button, message } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const GalleryFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/gallery/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/gallery/${id}`, values);
        message.success('Gallery updated');
      } else {
        await api.post('/cms/gallery', values);
        message.success('Gallery created');
      }
      navigate('/cms/galleries');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Gallery' : 'New Gallery'} loading={loading} onSubmit={handleSubmit} backPath="/cms/galleries">
      <Form form={form} layout="vertical" initialValues={{ isActive: true }}>
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="images" label="Images">
          <Upload beforeUpload={() => false} multiple accept="image/*" listType="picture">
            <Button icon={<UploadOutlined />}>Upload Images</Button>
          </Upload>
        </Form.Item>
        <Form.Item name="category" label="Category">
          <Input />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default GalleryFormPage;
