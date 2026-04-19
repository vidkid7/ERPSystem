import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, message } from 'antd';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Ledger } from '../../types';

const DebitNotePage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [ledgers, setLedgers] = useState<Ledger[]>([]);

  useEffect(() => {
    api.get('/account/ledger', { params: { pageSize: 1000 } })
      .then((res) => setLedgers(res.data.data || []))
      .catch(() => message.error('Failed to load ledgers'));
  }, []);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      const payload = {
        partyLedgerId: values.partyLedgerId,
        amount: values.amount,
        reason: values.reason,
        referenceInvoiceNo: values.referenceInvoiceNo,
        noteDate: values.noteDate?.format('YYYY-MM-DD'),
      };
      await api.post('/account/debit-note', payload);
      message.success('Debit note created successfully');
      navigate('/account/vouchers');
    } catch (err: any) {
      if (err.errorFields) return;
      message.error(err.response?.data?.responseMSG || 'Failed to save debit note');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title="Debit Note" loading={loading} onSubmit={handleSubmit} backPath="/account/vouchers">
      <Form form={form} layout="vertical">
        <Form.Item name="partyLedgerId" label="Party Ledger" rules={[{ required: true, message: 'Please select party' }]}>
          <Select showSearch optionFilterProp="label" placeholder="Select party" style={{ width: 300 }}
            options={ledgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          />
        </Form.Item>
        <Form.Item name="amount" label="Amount" rules={[{ required: true, message: 'Please enter amount' }]}>
          <InputNumber style={{ width: 200 }} min={0.01} precision={2} />
        </Form.Item>
        <Form.Item name="reason" label="Reason" rules={[{ required: true, message: 'Please enter reason' }]}>
          <Input.TextArea rows={3} style={{ maxWidth: 500 }} />
        </Form.Item>
        <Form.Item name="referenceInvoiceNo" label="Reference Invoice No">
          <Input style={{ width: 200 }} />
        </Form.Item>
        <Form.Item name="noteDate" label="Date" rules={[{ required: true, message: 'Please select date' }]}>
          <DatePicker style={{ width: 200 }} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default DebitNotePage;
