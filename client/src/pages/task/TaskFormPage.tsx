import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const TaskFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/task/task/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue({
          ...d,
          dueDate: d.dueDate ? dayjs(d.dueDate) : undefined,
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
        dueDate: values.dueDate?.toISOString(),
      };
      if (isEdit) {
        await api.post(`/task/task/${id}`, payload);
        message.success('Task updated successfully');
      } else {
        await api.post('/task/task', payload);
        message.success('Task created successfully');
      }
      navigate('/task/tasks');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Task' : 'New Task'} loading={loading} onSubmit={handleSubmit} backPath="/task/tasks">
      <Form form={form} layout="vertical" initialValues={{ priority: 'Medium', status: 'Todo' }}>
        <Form.Item name="title" label="Title" rules={[{ required: true, message: 'Please enter task title' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <TextArea rows={4} />
        </Form.Item>
        <Form.Item name="assignedToId" label="Assigned To (ID)">
          <InputNumber style={{ width: '100%' }} min={1} />
        </Form.Item>
        <Form.Item name="priority" label="Priority" rules={[{ required: true }]}>
          <Select options={[
            { value: 'Low', label: 'Low' },
            { value: 'Medium', label: 'Medium' },
            { value: 'High', label: 'High' },
            { value: 'Critical', label: 'Critical' },
          ]} />
        </Form.Item>
        <Form.Item name="dueDate" label="Due Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="status" label="Status">
          <Select options={[
            { value: 'Todo', label: 'Todo' },
            { value: 'InProgress', label: 'In Progress' },
            { value: 'Completed', label: 'Completed' },
            { value: 'OnHold', label: 'On Hold' },
          ]} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default TaskFormPage;
