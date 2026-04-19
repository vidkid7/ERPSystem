import React, { useEffect, useState } from 'react';
import { Form, Input, Select, Upload, Button, message } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const ComplaintTicketFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/service/complaint-ticket/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/service/complaint-ticket/${id}`, values);
        message.success('Complaint ticket updated');
      } else {
        await api.post('/service/complaint-ticket', values);
        message.success('Complaint ticket created');
      }
      navigate('/service/complaint-tickets');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Complaint Ticket' : 'New Complaint Ticket'} loading={loading} onSubmit={handleSubmit} backPath="/service/complaint-tickets">
      <Form form={form} layout="vertical" initialValues={{ priority: 'Medium', status: 'Open' }}>
        <Form.Item name="customerName" label="Customer" rules={[{ required: true, message: 'Please enter customer name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="category" label="Category" rules={[{ required: true, message: 'Please select category' }]}>
          <Select options={[
            { value: 'Hardware', label: 'Hardware' },
            { value: 'Software', label: 'Software' },
            { value: 'Service', label: 'Service' },
            { value: 'Billing', label: 'Billing' },
            { value: 'Other', label: 'Other' },
          ]} />
        </Form.Item>
        <Form.Item name="priority" label="Priority" rules={[{ required: true }]}>
          <Select options={[
            { value: 'Low', label: 'Low' },
            { value: 'Medium', label: 'Medium' },
            { value: 'High', label: 'High' },
            { value: 'Critical', label: 'Critical' },
          ]} />
        </Form.Item>
        <Form.Item name="description" label="Description" rules={[{ required: true, message: 'Please enter description' }]}>
          <TextArea rows={4} />
        </Form.Item>
        <Form.Item name="attachments" label="Attachments">
          <Upload beforeUpload={() => false} multiple>
            <Button icon={<UploadOutlined />}>Select Files</Button>
          </Upload>
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

export default ComplaintTicketFormPage;
