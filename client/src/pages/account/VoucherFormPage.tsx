import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Table, Button, Space, message, Typography } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Ledger } from '../../types';

const { Text } = Typography;

interface VoucherLine {
  key: number;
  ledgerId: number | undefined;
  debitAmount: number;
  creditAmount: number;
}

let lineKey = 0;

const VoucherFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [ledgers, setLedgers] = useState<Ledger[]>([]);
  const [lines, setLines] = useState<VoucherLine[]>([
    { key: ++lineKey, ledgerId: undefined, debitAmount: 0, creditAmount: 0 },
    { key: ++lineKey, ledgerId: undefined, debitAmount: 0, creditAmount: 0 },
  ]);

  useEffect(() => { fetchLedgers(); }, []);

  const fetchLedgers = async () => {
    try {
      const res = await api.get('/account/ledger', { params: { pageSize: 1000 } });
      setLedgers(res.data.data || []);
    } catch {
      message.error('Failed to load ledgers');
    }
  };

  const totalDebit = lines.reduce((sum, l) => sum + (l.debitAmount || 0), 0);
  const totalCredit = lines.reduce((sum, l) => sum + (l.creditAmount || 0), 0);

  const updateLine = (key: number, field: keyof VoucherLine, value: any) => {
    setLines((prev) => prev.map((l) => (l.key === key ? { ...l, [field]: value } : l)));
  };

  const addLine = () => {
    setLines((prev) => [...prev, { key: ++lineKey, ledgerId: undefined, debitAmount: 0, creditAmount: 0 }]);
  };

  const removeLine = (key: number) => {
    setLines((prev) => (prev.length <= 1 ? prev : prev.filter((l) => l.key !== key)));
  };

  const lineColumns = [
    {
      title: 'Ledger', dataIndex: 'ledgerId', key: 'ledgerId', width: 300,
      render: (_: any, record: VoucherLine) => (
        <Select placeholder="Select ledger" showSearch optionFilterProp="label" style={{ width: '100%' }}
          value={record.ledgerId}
          onChange={(v) => updateLine(record.key, 'ledgerId', v)}
          options={ledgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
        />
      ),
    },
    {
      title: 'Debit Amount', dataIndex: 'debitAmount', key: 'debitAmount', width: 180,
      render: (_: any, record: VoucherLine) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2}
          value={record.debitAmount}
          onChange={(v) => updateLine(record.key, 'debitAmount', v || 0)}
        />
      ),
    },
    {
      title: 'Credit Amount', dataIndex: 'creditAmount', key: 'creditAmount', width: 180,
      render: (_: any, record: VoucherLine) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2}
          value={record.creditAmount}
          onChange={(v) => updateLine(record.key, 'creditAmount', v || 0)}
        />
      ),
    },
    {
      title: '', key: 'action', width: 60,
      render: (_: any, record: VoucherLine) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeLine(record.key)} />
      ),
    },
  ];

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();

      if (lines.some((l) => !l.ledgerId)) {
        message.error('Please select a ledger for each line');
        return;
      }
      if (lines.every((l) => l.debitAmount === 0 && l.creditAmount === 0)) {
        message.error('Please enter at least one debit or credit amount');
        return;
      }
      if (Math.abs(totalDebit - totalCredit) > 0.001) {
        message.error('Total debit must equal total credit');
        return;
      }

      setLoading(true);
      const payload = {
        voucherDate: values.voucherDate?.format('YYYY-MM-DD'),
        narration: values.narration,
        documentTypeId: values.documentTypeId,
        details: lines.map((l) => ({
          ledgerId: l.ledgerId,
          debitAmount: l.debitAmount,
          creditAmount: l.creditAmount,
        })),
      };
      await api.post('/account/voucher', payload);
      message.success('Voucher created successfully');
      navigate('/account/vouchers');
    } catch (err: any) {
      if (err.errorFields) return;
      message.error('Failed to save voucher');
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title="New Voucher" loading={loading} onSubmit={handleSubmit} backPath="/account/vouchers">
      <Form form={form} layout="vertical">
        <Space size="large" wrap>
          <Form.Item name="documentTypeId" label="Document Type" rules={[{ required: true, message: 'Please select type' }]}>
            <Select placeholder="Select type" style={{ width: 200 }}
              options={[
                { value: 1, label: 'Journal' },
                { value: 2, label: 'Payment' },
                { value: 3, label: 'Receipt' },
                { value: 4, label: 'Contra' },
              ]}
            />
          </Form.Item>
          <Form.Item name="voucherDate" label="Date" rules={[{ required: true, message: 'Please select date' }]}>
            <DatePicker style={{ width: 200 }} />
          </Form.Item>
          <Form.Item name="narration" label="Narration" style={{ minWidth: 300 }}>
            <Input.TextArea rows={1} />
          </Form.Item>
        </Space>
      </Form>

      <Table
        dataSource={lines}
        columns={lineColumns}
        rowKey="key"
        pagination={false}
        size="middle"
        style={{ marginTop: 16 }}
        footer={() => (
          <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Button type="dashed" icon={<PlusOutlined />} onClick={addLine}>Add Row</Button>
            <Space size="large">
              <Text strong>Total Debit: {totalDebit.toFixed(2)}</Text>
              <Text strong>Total Credit: {totalCredit.toFixed(2)}</Text>
              {Math.abs(totalDebit - totalCredit) > 0.001 && (
                <Text type="danger">Difference: {Math.abs(totalDebit - totalCredit).toFixed(2)}</Text>
              )}
            </Space>
          </div>
        )}
      />
    </FormPage>
  );
};

export default VoucherFormPage;
