import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const LabPackageFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/lab/package/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/lab/package/${id}`, values);
        message.success('Lab package updated');
      } else {
        await api.post('/lab/package', values);
        message.success('Lab package created');
      }
      navigate('/lab/packages');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Lab Package' : 'New Lab Package'} loading={loading} onSubmit={handleSubmit} backPath="/lab/packages">
      <Form form={form} layout="vertical">
        <Form.Item name="packageName" label="Package Name" rules={[{ required: true, message: 'Please enter package name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="tests" label="Tests">
          <Select mode="multiple" placeholder="Select tests" options={[]} />
        </Form.Item>
        <Form.Item name="discount" label="Discount (%)">
          <InputNumber style={{ width: '100%' }} min={0} max={100} precision={2} />
        </Form.Item>
        <Form.Item name="price" label="Package Price">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default LabPackageFormPage;
