import React, { useEffect, useState } from 'react';
import { Form, Input, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const SpecimenFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/lab/specimen/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/lab/specimen/${id}`, values);
        message.success('Specimen updated');
      } else {
        await api.post('/lab/specimen', values);
        message.success('Specimen created');
      }
      navigate('/lab/specimens');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Specimen' : 'New Specimen'} loading={loading} onSubmit={handleSubmit} backPath="/lab/specimens">
      <Form form={form} layout="vertical">
        <Form.Item name="specimenType" label="Specimen Type" rules={[{ required: true, message: 'Please enter specimen type' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="container" label="Container">
          <Input />
        </Form.Item>
        <Form.Item name="stability" label="Stability">
          <Input placeholder="e.g. 7 days at 2-8°C" />
        </Form.Item>
        <Form.Item name="collectionInstructions" label="Collection Instructions">
          <Input.TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default SpecimenFormPage;
