import React, { useEffect, useState } from 'react';
import { Form, Input, DatePicker, InputNumber, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const EmployeeFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/hr/employee/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue({
          ...d,
          joinDate: d.joinDate ? dayjs(d.joinDate) : undefined,
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
        joinDate: values.joinDate?.toISOString(),
      };
      if (isEdit) {
        await api.post(`/hr/employee/${id}`, payload);
        message.success('Employee updated successfully');
      } else {
        await api.post('/hr/employee', payload);
        message.success('Employee created successfully');
      }
      navigate('/hr/employees');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Employee' : 'Add Employee'} loading={loading} onSubmit={handleSubmit} backPath="/hr/employees">
      <Form form={form} layout="vertical" initialValues={{ isActive: true }}>
        <Form.Item name="firstName" label="First Name" rules={[{ required: true, message: 'Please enter first name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="lastName" label="Last Name" rules={[{ required: true, message: 'Please enter last name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="email" label="Email" rules={[{ type: 'email', message: 'Please enter a valid email' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="phone" label="Phone">
          <Input />
        </Form.Item>
        <Form.Item name="department" label="Department">
          <Input />
        </Form.Item>
        <Form.Item name="designation" label="Designation">
          <Input />
        </Form.Item>
        <Form.Item name="joinDate" label="Join Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="salary" label="Salary">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default EmployeeFormPage;
