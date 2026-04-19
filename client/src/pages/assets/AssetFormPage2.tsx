import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, DatePicker, Select, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AssetFormPage2: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/asset/${id}`).then((res) => {
        form.setFieldsValue(res.data.data);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.post(`/asset/${id}`, values);
        message.success('Asset updated');
      } else {
        await api.post('/asset', values);
        message.success('Asset created');
      }
      navigate('/assets/list');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Asset' : 'New Asset'} loading={loading} onSubmit={handleSubmit} backPath="/assets/list">
      <Form form={form} layout="vertical">
        <Form.Item name="assetName" label="Asset Name" rules={[{ required: true, message: 'Please enter asset name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="assetCode" label="Asset Code" rules={[{ required: true, message: 'Please enter asset code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="categoryId" label="Category">
          <Input />
        </Form.Item>
        <Form.Item name="purchaseDate" label="Purchase Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="purchaseValue" label="Purchase Value">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="location" label="Location">
          <Input />
        </Form.Item>
        <Form.Item name="serialNo" label="Serial No">
          <Input />
        </Form.Item>
        <Form.Item name="warrantyExpiry" label="Warranty Expiry">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="status" label="Status" initialValue="Available">
          <Select options={[
            { value: 'Available', label: 'Available' },
            { value: 'Issued', label: 'Issued' },
            { value: 'Repair', label: 'Repair' },
            { value: 'Damaged', label: 'Damaged' },
            { value: 'Disposed', label: 'Disposed' },
          ]} />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AssetFormPage2;
