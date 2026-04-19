import React, { useEffect, useState } from 'react';
import { Form, Select, DatePicker, InputNumber, Input, message } from 'antd';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AssetDamagePage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [assets, setAssets] = useState<any[]>([]);

  useEffect(() => {
    api.get('/asset', { params: { pageSize: 200 } }).then(r => setAssets(r.data.data || []));
    form.setFieldsValue({ transactionDate: dayjs(), transactionType: 4 });
  }, []);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      values.transactionDate = values.transactionDate.toISOString();
      values.transactionType = 4; // Damage
      setLoading(true);
      await api.post('/assettransaction', values);
      message.success('Asset damage recorded successfully');
      navigate('/assets/asset');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally { setLoading(false); }
  };

  return (
    <FormPage title="Record Asset Damage" loading={loading} onSubmit={handleSubmit} backPath="/assets/asset">
      <Form form={form} layout="vertical">
        <Form.Item name="assetId" label="Asset" rules={[{ required: true, message: 'Asset is required' }]}>
          <Select placeholder="Select asset" showSearch optionFilterProp="children"
            options={assets.map(a => ({ label: `${a.assetCode} - ${a.name}`, value: a.id }))} />
        </Form.Item>
        <Form.Item name="transactionDate" label="Date" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="amount" label="Damage Value Reduction">
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="documentNo" label="Document No">
          <Input />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks">
          <Input.TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default AssetDamagePage;
