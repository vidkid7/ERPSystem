import React, { useEffect, useState } from 'react';
import { Form, Input, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const EventTypeFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/event-type/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/event-type/${id}`, values);
        message.success('Event type updated');
      } else {
        await api.post('/cms/event-type', values);
        message.success('Event type created');
      }
      navigate('/cms/event-types');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Event Type' : 'New Event Type'} loading={loading} onSubmit={handleSubmit} backPath="/cms/event-types">
      <Form form={form} layout="vertical">
        <Form.Item name="eventTypeName" label="Event Type Name" rules={[{ required: true, message: 'Please enter event type name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="color" label="Color">
          <Input placeholder="#1677ff" />
        </Form.Item>
        <Form.Item name="icon" label="Icon">
          <Input placeholder="e.g. CalendarOutlined" />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default EventTypeFormPage;
