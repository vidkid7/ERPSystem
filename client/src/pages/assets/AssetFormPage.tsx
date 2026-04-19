import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const AssetFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id && id !== 'new';

  const [loading, setLoading] = useState(false);
  const [models, setModels] = useState<any[]>([]);
  const [categories, setCategories] = useState<any[]>([]);
  const [employees, setEmployees] = useState<any[]>([]);

  useEffect(() => {
    api.get('/assetmodel', { params: { pageSize: 200 } }).then(r => setModels(r.data.data || []));
    api.get('/assetcategory', { params: { pageSize: 200 } }).then(r => setCategories(r.data.data || []));
    api.get('/hr/employee', { params: { pageSize: 200 } }).then(r => setEmployees(r.data.data || [])).catch(() => {});
    if (isEdit) {
      api.get(`/asset/${id}`).then(r => {
        const d = r.data.data || r.data;
        if (d.purchaseDate) d.purchaseDate = dayjs(d.purchaseDate);
        form.setFieldsValue(d);
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      if (values.purchaseDate) values.purchaseDate = values.purchaseDate.toISOString();
      setLoading(true);
      if (isEdit) {
        await api.put(`/asset/${id}`, { ...values, id: Number(id) });
        message.success('Asset updated successfully');
      } else {
        await api.post('/asset', values);
        message.success('Asset created successfully');
      }
      navigate('/assets/asset');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage
      title={isEdit ? 'Edit Asset' : 'New Asset'}
      loading={loading}
      onSubmit={handleSubmit}
      backPath="/assets/asset"
    >
      <Form form={form} layout="vertical">
        <Form.Item name="assetCode" label="Asset Code" rules={[{ required: true, message: 'Asset code is required' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="name" label="Name">
          <Input />
        </Form.Item>
        <Form.Item name="assetModelId" label="Asset Model">
          <Select placeholder="Select model" showSearch optionFilterProp="children"
            options={models.map(m => ({ label: m.name, value: m.id }))} allowClear />
        </Form.Item>
        <Form.Item name="assetCategoryId" label="Asset Category">
          <Select placeholder="Select category" showSearch optionFilterProp="children"
            options={categories.map(c => ({ label: c.name, value: c.id }))} allowClear />
        </Form.Item>
        <Form.Item name="purchaseDate" label="Purchase Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="purchaseCost" label="Purchase Cost" rules={[{ required: true, message: 'Purchase cost is required' }]}>
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="currentValue" label="Current Value">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="location" label="Location">
          <Input />
        </Form.Item>
        <Form.Item name="serialNumber" label="Serial Number">
          <Input />
        </Form.Item>
        <Form.Item name="assignedToEmployeeId" label="Assigned To">
          <Select placeholder="Select employee" showSearch optionFilterProp="children"
            options={employees.map(e => ({ label: e.name, value: e.id }))} allowClear />
        </Form.Item>
        <Form.Item name="notes" label="Notes">
          <TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AssetFormPage;
