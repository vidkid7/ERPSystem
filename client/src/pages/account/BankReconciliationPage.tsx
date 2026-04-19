import React, { useEffect, useState } from 'react';
import { Card, Select, DatePicker, Table, Checkbox, Button, message, Typography, Space } from 'antd';
import api from '../../services/api';
import type { Ledger } from '../../types';

const { Title } = Typography;
const { RangePicker } = DatePicker;

interface ReconciliationRow {
  id: number;
  voucherNumber: string;
  voucherDate: string;
  narration: string;
  debitAmount: number;
  creditAmount: number;
  isReconciled: boolean;
  bankDate: string | null;
}

const BankReconciliationPage: React.FC = () => {
  const [ledgers, setLedgers] = useState<Ledger[]>([]);
  const [bankLedgerId, setBankLedgerId] = useState<number | undefined>();
  const [dateRange, setDateRange] = useState<[string, string] | null>(null);
  const [data, setData] = useState<ReconciliationRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    api.get('/account/ledger', { params: { pageSize: 1000 } })
      .then((res) => setLedgers((res.data.data || []).filter((l: Ledger) => l.groupName?.toLowerCase().includes('bank'))))
      .catch(() => message.error('Failed to load ledgers'));
  }, []);

  const fetchData = async () => {
    if (!bankLedgerId) return;
    setLoading(true);
    try {
      const params: Record<string, any> = { bankLedgerId };
      if (dateRange) {
        params.fromDate = dateRange[0];
        params.toDate = dateRange[1];
      }
      const res = await api.get('/account/bank-reconciliation', { params });
      setData(res.data.data || []);
    } catch {
      message.error('Failed to load reconciliation data');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (bankLedgerId) fetchData();
  }, [bankLedgerId, dateRange]);

  const toggleReconciled = (id: number, checked: boolean) => {
    setData((prev) => prev.map((r) => (r.id === id ? { ...r, isReconciled: checked } : r)));
  };

  const updateBankDate = (id: number, date: string | null) => {
    setData((prev) => prev.map((r) => (r.id === id ? { ...r, bankDate: date } : r)));
  };

  const handleSave = async () => {
    setSaving(true);
    try {
      const reconciled = data.filter((r) => r.isReconciled).map((r) => ({
        id: r.id,
        bankDate: r.bankDate,
      }));
      await api.post('/account/bank-reconciliation', { bankLedgerId, items: reconciled });
      message.success('Reconciliation saved successfully');
      fetchData();
    } catch {
      message.error('Failed to save reconciliation');
    } finally {
      setSaving(false);
    }
  };

  const columns = [
    {
      title: 'Reconciled', dataIndex: 'isReconciled', key: 'isReconciled', width: 90,
      render: (val: boolean, record: ReconciliationRow) => (
        <Checkbox checked={val} onChange={(e) => toggleReconciled(record.id, e.target.checked)} />
      ),
    },
    { title: 'Voucher No', dataIndex: 'voucherNumber', key: 'voucherNumber', width: 140 },
    {
      title: 'Voucher Date', dataIndex: 'voucherDate', key: 'voucherDate', width: 120,
      render: (v: string) => v ? new Date(v).toLocaleDateString() : '',
    },
    { title: 'Narration', dataIndex: 'narration', key: 'narration' },
    {
      title: 'Debit', dataIndex: 'debitAmount', key: 'debitAmount', width: 120,
      render: (v: number) => v?.toFixed(2), align: 'right' as const,
    },
    {
      title: 'Credit', dataIndex: 'creditAmount', key: 'creditAmount', width: 120,
      render: (v: number) => v?.toFixed(2), align: 'right' as const,
    },
    {
      title: 'Bank Date', dataIndex: 'bankDate', key: 'bankDate', width: 160,
      render: (_: any, record: ReconciliationRow) => (
        <DatePicker
          size="small"
          style={{ width: '100%' }}
          disabled={!record.isReconciled}
          onChange={(d) => updateBankDate(record.id, d?.format('YYYY-MM-DD') || null)}
        />
      ),
    },
  ];

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Bank Reconciliation</Title>
        <Button type="primary" loading={saving} onClick={handleSave}>Save Reconciliation</Button>
      </div>
      <Space style={{ marginBottom: 16 }} size="large" wrap>
        <Select
          showSearch optionFilterProp="label" placeholder="Select bank ledger" style={{ width: 300 }}
          value={bankLedgerId}
          onChange={setBankLedgerId}
          options={ledgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
        />
        <RangePicker onChange={(_: any, ds: [string, string]) => setDateRange(ds[0] ? ds : null)} />
      </Space>
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        size="middle"
        pagination={false}
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
};

export default BankReconciliationPage;
