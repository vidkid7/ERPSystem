import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const LabTestFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/lab/test/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/lab/test/${id}`, values);
        message.success('Lab test updated');
      } else {
        await api.post('/lab/test', values);
        message.success('Lab test created');
      }
      navigate('/lab/tests');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Lab Test' : 'New Lab Test'} loading={loading} onSubmit={handleSubmit} backPath="/lab/tests">
      <Form form={form} layout="vertical">
        <Form.Item name="testName" label="Test Name" rules={[{ required: true, message: 'Please enter test name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="category" label="Category">
          <Input />
        </Form.Item>
        <Form.Item name="price" label="Price">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="method" label="Method">
          <Input />
        </Form.Item>
        <Form.Item name="normalRange" label="Normal Range">
          <Input />
        </Form.Item>
        <Form.Item name="unit" label="Unit">
          <Input />
        </Form.Item>
        <Form.Item name="turnaroundTime" label="Turnaround Time (hrs)">
          <InputNumber style={{ width: '100%' }} min={0} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default LabTestFormPage;
