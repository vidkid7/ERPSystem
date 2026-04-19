import React, { useEffect, useState } from 'react';
import { Form, Input, DatePicker, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const PatientFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/hms/patient/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue({
          ...d,
          dateOfBirth: d.dateOfBirth ? dayjs(d.dateOfBirth) : undefined,
        });
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      const payload = {
        ...values,
        dateOfBirth: values.dateOfBirth?.toISOString(),
      };
      if (isEdit) {
        await api.post(`/hms/patient/${id}`, payload);
        message.success('Patient updated successfully');
      } else {
        await api.post('/hms/patient', payload);
        message.success('Patient registered successfully');
      }
      navigate('/hms/patients');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Patient' : 'Register Patient'} loading={loading} onSubmit={handleSubmit} backPath="/hms/patients">
      <Form form={form} layout="vertical">
        <Form.Item name="firstName" label="First Name" rules={[{ required: true, message: 'Please enter first name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="lastName" label="Last Name" rules={[{ required: true, message: 'Please enter last name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="gender" label="Gender" rules={[{ required: true, message: 'Please select gender' }]}>
          <Select options={[{ value: 'Male', label: 'Male' }, { value: 'Female', label: 'Female' }, { value: 'Other', label: 'Other' }]} />
        </Form.Item>
        <Form.Item name="dateOfBirth" label="Date of Birth">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="phone" label="Phone" rules={[{ required: true, message: 'Please enter phone number' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="address" label="Address">
          <TextArea rows={3} />
        </Form.Item>
        <Form.Item name="bloodGroup" label="Blood Group">
          <Select options={['A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-'].map((v) => ({ value: v, label: v }))} />
        </Form.Item>
        <Form.Item name="emergencyContactName" label="Emergency Contact Name">
          <Input />
        </Form.Item>
        <Form.Item name="emergencyContactPhone" label="Emergency Contact Phone">
          <Input />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default PatientFormPage;
