import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, message } from 'antd';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Ledger } from '../../types';

const ContraVoucherPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [ledgers, setLedgers] = useState<Ledger[]>([]);

  useEffect(() => {
    api.get('/account/ledger', { params: { pageSize: 1000 } })
      .then((res) => setLedgers(res.data.data || []))
      .catch(() => message.error('Failed to load ledgers'));
  }, []);

  const bankLedgers = ledgers.filter((l) => l.groupName?.toLowerCase().includes('bank') || l.groupName?.toLowerCase().includes('cash'));

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      if (values.fromBankId === values.toBankId) {
        message.error('From and To bank must be different');
        return;
      }
      setLoading(true);
      const payload = {
        fromBankId: values.fromBankId,
        toBankId: values.toBankId,
        amount: values.amount,
        voucherDate: values.voucherDate?.format('YYYY-MM-DD'),
        narration: values.narration,
      };
      await api.post('/account/voucher/contra', payload);
      message.success('Contra voucher created successfully');
      navigate('/account/vouchers');
    } catch (err: any) {
      if (err.errorFields) return;
      message.error(err.response?.data?.responseMSG || 'Failed to save contra voucher');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title="Contra Voucher" loading={loading} onSubmit={handleSubmit} backPath="/account/vouchers">
      <Form form={form} layout="vertical">
        <Form.Item name="fromBankId" label="From Bank" rules={[{ required: true, message: 'Please select source bank' }]}>
          <Select showSearch optionFilterProp="label" placeholder="Select source bank" style={{ width: 300 }}
            options={bankLedgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          />
        </Form.Item>
        <Form.Item name="toBankId" label="To Bank" rules={[{ required: true, message: 'Please select destination bank' }]}>
          <Select showSearch optionFilterProp="label" placeholder="Select destination bank" style={{ width: 300 }}
            options={bankLedgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          />
        </Form.Item>
        <Form.Item name="amount" label="Amount" rules={[{ required: true, message: 'Please enter amount' }]}>
          <InputNumber style={{ width: 200 }} min={0.01} precision={2} />
        </Form.Item>
        <Form.Item name="voucherDate" label="Date" rules={[{ required: true, message: 'Please select date' }]}>
          <DatePicker style={{ width: 200 }} />
        </Form.Item>
        <Form.Item name="narration" label="Narration">
          <Input.TextArea rows={3} style={{ maxWidth: 500 }} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default ContraVoucherPage;
