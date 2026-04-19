import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface LabReportRow {
  id: number;
  date: string;
  patient: string;
  tests: string;
  status: string;
  amount: number;
}

const statusColor: Record<string, string> = { Pending: 'orange', Completed: 'green', Verified: 'blue', Delivered: 'default' };

const columns: ColumnsType<LabReportRow> = [
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Patient', dataIndex: 'patient', key: 'patient' },
  { title: 'Tests', dataIndex: 'tests', key: 'tests' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const LabReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<LabReportRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/lab/report-summary', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Lab Report Summary">
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

export default LabReportPage;
