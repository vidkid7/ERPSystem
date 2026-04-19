import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AppointmentFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/service/appointment/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/service/appointment/${id}`, values);
        message.success('Appointment updated successfully');
      } else {
        await api.post('/service/appointment', values);
        message.success('Appointment created successfully');
      }
      navigate('/service/appointments');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Appointment' : 'New Appointment'} loading={loading} onSubmit={handleSubmit} backPath="/service/appointments">
      <Form form={form} layout="vertical">
        <Form.Item name="customerName" label="Customer" rules={[{ required: true, message: 'Please enter customer name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="product" label="Product" rules={[{ required: true, message: 'Please enter product' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="issue" label="Issue">
          <Input.TextArea rows={3} />
        </Form.Item>
        <Form.Item name="preferredDate" label="Preferred Date" rules={[{ required: true, message: 'Please select date' }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="technician" label="Technician">
          <Input />
        </Form.Item>
        <Form.Item name="status" label="Status" initialValue="Scheduled">
          <Select options={[
            { value: 'Scheduled', label: 'Scheduled' },
            { value: 'Confirmed', label: 'Confirmed' },
            { value: 'Completed', label: 'Completed' },
            { value: 'Cancelled', label: 'Cancelled' },
          ]} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AppointmentFormPage;
