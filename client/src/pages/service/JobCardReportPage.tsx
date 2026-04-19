import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface JobCardReportRow {
  id: number;
  date: string;
  jobcardNo: string;
  customer: string;
  technician: string;
  partsCost: number;
  labor: number;
  total: number;
}

const columns: ColumnsType<JobCardReportRow> = [
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Jobcard No', dataIndex: 'jobcardNo', key: 'jobcardNo', width: 130 },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Technician', dataIndex: 'technician', key: 'technician' },
  { title: 'Parts Cost', dataIndex: 'partsCost', key: 'partsCost', align: 'right', width: 110, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Labor', dataIndex: 'labor', key: 'labor', align: 'right', width: 100, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Total', dataIndex: 'total', key: 'total', align: 'right', width: 110, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const JobCardReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<JobCardReportRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/service/jobcard-report', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Jobcard Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 800 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default JobCardReportPage;
