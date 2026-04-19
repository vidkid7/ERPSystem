import React, { useEffect, useState } from 'react';
import { Form, Select, DatePicker, Input, message } from 'antd';
import { useNavigate } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const AssetReturnPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [assets, setAssets] = useState<any[]>([]);
  const [employees, setEmployees] = useState<any[]>([]);

  useEffect(() => {
    api.get('/asset', { params: { pageSize: 200, status: 'Issued' } }).then(r => setAssets(r.data.data || []));
    api.get('/hr/employee', { params: { pageSize: 200 } }).then(r => setEmployees(r.data.data || [])).catch(() => {});
    form.setFieldsValue({ transactionDate: dayjs(), transactionType: 3 });
  }, []);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      values.transactionDate = values.transactionDate.toISOString();
      values.transactionType = 3; // Return
      values.amount = 0;
      setLoading(true);
      await api.post('/assettransaction', values);
      message.success('Asset returned successfully');
      navigate('/assets/asset');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally { setLoading(false); }
  };

  return (
    <FormPage title="Asset Return" loading={loading} onSubmit={handleSubmit} backPath="/assets/asset">
      <Form form={form} layout="vertical">
        <Form.Item name="assetId" label="Asset" rules={[{ required: true, message: 'Asset is required' }]}>
          <Select placeholder="Select asset" showSearch optionFilterProp="children"
            options={assets.map(a => ({ label: `${a.assetCode} - ${a.name}`, value: a.id }))} />
        </Form.Item>
        <Form.Item name="fromEmployeeId" label="Returned By (Employee)" rules={[{ required: true, message: 'Employee is required' }]}>
          <Select placeholder="Select employee" showSearch optionFilterProp="children"
            options={employees.map(e => ({ label: e.name, value: e.id }))} />
        </Form.Item>
        <Form.Item name="transactionDate" label="Return Date" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} />
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

export default AssetReturnPage;
