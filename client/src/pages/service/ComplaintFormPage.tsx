import React, { useEffect, useState } from 'react';
import { Form, Input, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const ComplaintFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/service/complaintticket/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/service/complaintticket/${id}`, values);
        message.success('Complaint updated successfully');
      } else {
        await api.post('/service/complaintticket', values);
        message.success('Complaint created successfully');
      }
      navigate('/service/complaints');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Complaint' : 'New Complaint'} loading={loading} onSubmit={handleSubmit} backPath="/service/complaints">
      <Form form={form} layout="vertical" initialValues={{ priority: 'Medium', status: 'Open' }}>
        <Form.Item name="customerName" label="Customer Name" rules={[{ required: true, message: 'Please enter customer name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="deviceType" label="Device Type">
          <Input />
        </Form.Item>
        <Form.Item name="deviceName" label="Device Name">
          <Input />
        </Form.Item>
        <Form.Item name="serialNo" label="Serial No">
          <Input />
        </Form.Item>
        <Form.Item name="complaintDescription" label="Complaint Description" rules={[{ required: true, message: 'Please enter complaint description' }]}>
          <TextArea rows={4} />
        </Form.Item>
        <Form.Item name="priority" label="Priority" rules={[{ required: true }]}>
          <Select options={[
            { value: 'Low', label: 'Low' },
            { value: 'Medium', label: 'Medium' },
            { value: 'High', label: 'High' },
            { value: 'Critical', label: 'Critical' },
          ]} />
        </Form.Item>
        <Form.Item name="status" label="Status">
          <Select options={[
            { value: 'Open', label: 'Open' },
            { value: 'InProgress', label: 'In Progress' },
            { value: 'Resolved', label: 'Resolved' },
            { value: 'Closed', label: 'Closed' },
          ]} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default ComplaintFormPage;
