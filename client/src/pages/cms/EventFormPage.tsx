import React, { useEffect, useState } from 'react';
import { Form, Input, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const EventFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/cms/event/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/cms/event/${id}`, values);
        message.success('Event updated');
      } else {
        await api.post('/cms/event', values);
        message.success('Event created');
      }
      navigate('/cms/events');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Event' : 'New Event'} loading={loading} onSubmit={handleSubmit} backPath="/cms/events">
      <Form form={form} layout="vertical">
        <Form.Item name="eventName" label="Event Name" rules={[{ required: true, message: 'Please enter event name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="eventType" label="Event Type">
          <Input />
        </Form.Item>
        <Form.Item name="eventDate" label="Date" rules={[{ required: true, message: 'Please select date' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="venue" label="Venue">
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={4} />
        </Form.Item>
        <Form.Item name="imageUrl" label="Image URL">
          <Input />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default EventFormPage;
