import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AssetGroupFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/asset/group/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/asset/group/${id}`, values);
        message.success('Asset group updated');
      } else {
        await api.post('/asset/group', values);
        message.success('Asset group created');
      }
      navigate('/assets/groups');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Asset Group' : 'New Asset Group'} loading={loading} onSubmit={handleSubmit} backPath="/assets/groups">
      <Form form={form} layout="vertical">
        <Form.Item name="groupName" label="Group Name" rules={[{ required: true, message: 'Please enter group name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="depreciationMethod" label="Depreciation Method">
          <Select options={[
            { value: 'StraightLine', label: 'Straight Line' },
            { value: 'DecliningBalance', label: 'Declining Balance' },
            { value: 'DoubleDeclining', label: 'Double Declining' },
          ]} />
        </Form.Item>
        <Form.Item name="depreciationRate" label="Depreciation Rate (%)">
          <InputNumber style={{ width: '100%' }} min={0} max={100} precision={2} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AssetGroupFormPage;
