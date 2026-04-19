import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Select } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const CashBankBookPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);
  const [accountType, setAccountType] = useState<string>('cash');

  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Narration', dataIndex: 'narration', key: 'narration' },
    { title: 'Voucher No', dataIndex: 'voucherNo', key: 'voucherNo' },
    { title: 'Debit', dataIndex: 'debit', key: 'debit', align: 'right' as const },
    { title: 'Credit', dataIndex: 'credit', key: 'credit', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = { accountType };
      if (dateRange) {
        params.fromDate = dateRange[0].format('YYYY-MM-DD');
        params.toDate = dateRange[1].format('YYYY-MM-DD');
      }
      const res = await api.get('/reporting/cash-bank-book', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Cash & Bank Book">
      <Space style={{ marginBottom: 16 }} wrap>
        <Select value={accountType} onChange={setAccountType} style={{ width: 120 }}
          options={[{ value: 'cash', label: 'Cash' }, { value: 'bank', label: 'Bank' }]} />
        <RangePicker onChange={(dates) => setDateRange(dates as any)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default CashBankBookPage;
