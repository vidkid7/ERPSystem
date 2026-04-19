import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface TechnicianPerformanceRow {
  id: number;
  technician: string;
  jobcards: number;
  completed: number;
  avgTime: string;
  revenue: number;
}

const columns: ColumnsType<TechnicianPerformanceRow> = [
  { title: 'Technician', dataIndex: 'technician', key: 'technician' },
  { title: 'Jobcards', dataIndex: 'jobcards', key: 'jobcards', align: 'right', width: 110 },
  { title: 'Completed', dataIndex: 'completed', key: 'completed', align: 'right', width: 110 },
  { title: 'Avg Time', dataIndex: 'avgTime', key: 'avgTime', width: 110 },
  { title: 'Revenue', dataIndex: 'revenue', key: 'revenue', align: 'right', width: 130, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const TechnicianPerformancePage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<TechnicianPerformanceRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/service/technician-performance', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Technician Performance">
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

export default TechnicianPerformancePage;
