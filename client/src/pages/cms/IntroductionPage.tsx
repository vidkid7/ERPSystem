import React, { useEffect, useState } from 'react';
import { Card, Form, Input, Switch, Button, Typography, message } from 'antd';
import { SaveOutlined } from '@ant-design/icons';
import api from '../../services/api';

const { Title } = Typography;
const { TextArea } = Input;

interface Introduction {
  title: string;
  content: string;
  isPublished: boolean;
}

const IntroductionPage: React.FC = () => {
  const [form] = Form.useForm<Introduction>();
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    api.get('/introduction').then((res) => {
      if (res.data) form.setFieldsValue(res.data);
    }).catch(() => {});
  }, [form]);

  const handleSave = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      await api.post('/introduction', values);
      message.success('Introduction saved successfully');
    } catch {
      // validation or request error — no action needed
    } finally {
      setLoading(false);
    }
  };

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 24 }}>
        <Title level={4} style={{ margin: 0 }}>Introduction</Title>
        <Button type="primary" icon={<SaveOutlined />} loading={loading} onClick={handleSave}>
          Save
        </Button>
      </div>
      <Form form={form} layout="vertical">
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Title is required' }]}>
          <Input placeholder="Enter title" />
        </Form.Item>
        <Form.Item name="content" label="Content" rules={[{ required: true, message: 'Content is required' }]}>
          <TextArea rows={6} placeholder="Enter content" />
        </Form.Item>
        <Form.Item name="isPublished" label="Published" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </Card>
  );
};

export default IntroductionPage;
