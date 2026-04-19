import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Radio, message } from 'antd';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Ledger } from '../../types';

const PaymentVoucherPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [ledgers, setLedgers] = useState<Ledger[]>([]);
  const paymentMode = Form.useWatch('paymentMode', form);

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
        paidTo: values.paidTo,
        bankLedgerId: values.bankLedgerId,
        amount: values.amount,
        paymentMode: values.paymentMode,
        chequeNo: values.chequeNo,
        voucherDate: values.voucherDate?.format('YYYY-MM-DD'),
        narration: values.narration,
      };
      await api.post('/account/voucher/payment', payload);
      message.success('Payment voucher created successfully');
      navigate('/account/vouchers');
    } catch (err: any) {
      if (err.errorFields) return;
      message.error(err.response?.data?.responseMSG || 'Failed to save payment voucher');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title="Payment Voucher" loading={loading} onSubmit={handleSubmit} backPath="/account/vouchers">
      <Form form={form} layout="vertical" initialValues={{ paymentMode: 'Cash' }}>
        <Form.Item name="paidTo" label="Paid To" rules={[{ required: true, message: 'Please select party' }]}>
          <Select showSearch optionFilterProp="label" placeholder="Select party" style={{ width: 300 }}
            options={ledgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          />
        </Form.Item>
        <Form.Item name="bankLedgerId" label="Bank Ledger" rules={[{ required: true, message: 'Please select bank ledger' }]}>
          <Select showSearch optionFilterProp="label" placeholder="Select bank ledger" style={{ width: 300 }}
            options={ledgers.filter((l) => l.groupName?.toLowerCase().includes('bank')).map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          />
        </Form.Item>
        <Form.Item name="amount" label="Amount" rules={[{ required: true, message: 'Please enter amount' }]}>
          <InputNumber style={{ width: 200 }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="paymentMode" label="Payment Mode" rules={[{ required: true }]}>
          <Radio.Group>
            <Radio value="Cash">Cash</Radio>
            <Radio value="Cheque">Cheque</Radio>
            <Radio value="Transfer">Transfer</Radio>
          </Radio.Group>
        </Form.Item>
        {paymentMode === 'Cheque' && (
          <Form.Item name="chequeNo" label="Cheque No" rules={[{ required: true, message: 'Please enter cheque number' }]}>
            <Input style={{ width: 200 }} />
          </Form.Item>
        )}
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

export default PaymentVoucherPage;
