import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface LabBillingRow {
  id: number;
  patient: string;
  tests: string;
  amount: number;
  date: string;
  status: string;
}

const statusColor: Record<string, string> = { Paid: 'green', Unpaid: 'red', Partial: 'orange' };

const columns: ColumnsType<LabBillingRow> = [
  { title: 'Patient', dataIndex: 'patient', key: 'patient' },
  { title: 'Tests', dataIndex: 'tests', key: 'tests' },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 100, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const LabBillingPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<LabBillingRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/lab/billing', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Lab Billing">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 700 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default LabBillingPage;
