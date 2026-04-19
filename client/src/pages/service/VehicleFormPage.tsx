import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const VehicleFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/service/vehicle/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/service/vehicle/${id}`, values);
        message.success('Vehicle updated successfully');
      } else {
        await api.post('/service/vehicle', values);
        message.success('Vehicle created successfully');
      }
      navigate('/service/vehicles');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Vehicle' : 'New Vehicle'} loading={loading} onSubmit={handleSubmit} backPath="/service/vehicles">
      <Form form={form} layout="vertical">
        <Form.Item name="registrationNo" label="Registration No" rules={[{ required: true, message: 'Please enter registration number' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="make" label="Make" rules={[{ required: true, message: 'Please enter make' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="model" label="Model" rules={[{ required: true, message: 'Please enter model' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="year" label="Year">
          <InputNumber style={{ width: '100%' }} min={1900} max={2100} />
        </Form.Item>
        <Form.Item name="customerName" label="Customer">
          <Input />
        </Form.Item>
        <Form.Item name="engineNo" label="Engine No">
          <Input />
        </Form.Item>
        <Form.Item name="chassisNo" label="Chassis No">
          <Input />
        </Form.Item>
        <Form.Item name="color" label="Color">
          <Input />
        </Form.Item>
        <Form.Item name="purchaseDate" label="Purchase Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default VehicleFormPage;
