import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, Select, Radio, Switch, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { LedgerGroup } from '../../types';

const LedgerFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const [loading, setLoading] = useState(false);
  const [groups, setGroups] = useState<LedgerGroup[]>([]);
  const isEdit = !!id;

  useEffect(() => {
    fetchGroups();
    if (isEdit) fetchLedger();
  }, [id]);

  const fetchGroups = async () => {
    try {
      const res = await api.get('/account/ledger-group');
      setGroups(res.data.data || []);
    } catch {
      message.error('Failed to load ledger groups');
    }
  };

  const fetchLedger = async () => {
    setLoading(true);
    try {
      const res = await api.get(`/account/ledger/${id}`);
      form.setFieldsValue(res.data.data);
    } catch {
      message.error('Failed to load ledger');
    } finally {
      setLoading(false);
    }
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      if (isEdit) {
        await api.put(`/account/ledger/${id}`, { ...values, id: Number(id) });
        message.success('Ledger updated successfully');
      } else {
        await api.post('/account/ledger', values);
        message.success('Ledger created successfully');
      }
      navigate('/account/ledgers');
    } catch (err: any) {
      if (err.errorFields) return;
      message.error('Failed to save ledger');
    } finally {
      setLoading(false);
    }
  };

  const flattenGroups = (items: LedgerGroup[]): { id: number; name: string }[] => {
    const result: { id: number; name: string }[] = [];
    const walk = (list: LedgerGroup[]) => {
      for (const g of list) {
        result.push({ id: g.id, name: g.name });
        if (g.children?.length) walk(g.children);
      }
    };
    walk(items);
    return result;
  };

  return (
    <FormPage
      title={isEdit ? 'Edit Ledger' : 'New Ledger'}
      loading={loading}
      onSubmit={handleSubmit}
      backPath="/account/ledgers"
    >
      <Form form={form} layout="vertical" initialValues={{ isActive: true, openingBalanceType: 'Dr' }}>
        <Form.Item name="name" label="Name" rules={[{ required: true, message: 'Please enter name' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="code" label="Code" rules={[{ required: true, message: 'Please enter code' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="alias" label="Alias">
          <Input />
        </Form.Item>
        <Form.Item name="ledgerGroupId" label="Ledger Group" rules={[{ required: true, message: 'Please select a group' }]}>
          <Select placeholder="Select ledger group" showSearch optionFilterProp="label"
            options={flattenGroups(groups).map((g) => ({ value: g.id, label: g.name }))}
          />
        </Form.Item>
        <Form.Item name="openingBalance" label="Opening Balance">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="openingBalanceType" label="Opening Balance Type">
          <Radio.Group>
            <Radio value="Dr">Dr</Radio>
            <Radio value="Cr">Cr</Radio>
          </Radio.Group>
        </Form.Item>
        <Form.Item name="panNo" label="PAN No">
          <Input />
        </Form.Item>
        <Form.Item name="isActive" label="Is Active" valuePropName="checked">
          <Switch />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default LedgerFormPage;
