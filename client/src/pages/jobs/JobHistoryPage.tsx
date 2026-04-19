import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface JobHistoryRow {
  id: number;
  jobName: string;
  startTime: string;
  duration: string;
  status: string;
  records: number;
}

const statusColor: Record<string, string> = { Completed: 'green', Failed: 'red', Cancelled: 'orange' };

const columns: ColumnsType<JobHistoryRow> = [
  { title: 'Job Name', dataIndex: 'jobName', key: 'jobName' },
  { title: 'Start Time', dataIndex: 'startTime', key: 'startTime', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 160 },
  { title: 'Duration', dataIndex: 'duration', key: 'duration', width: 110 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Records', dataIndex: 'records', key: 'records', align: 'right', width: 100 },
];

const JobHistoryPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<JobHistoryRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/jobs/history', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Job History">
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

export default JobHistoryPage;
