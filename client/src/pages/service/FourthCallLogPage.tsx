import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface FourthCallLogRow {
  id: number;
  jobcardNo: string;
  customer: string;
  callDate: string;
  status: string;
  technician: string;
}

const columns: ColumnsType<FourthCallLogRow> = [
  { title: 'Jobcard No', dataIndex: 'jobcardNo', key: 'jobcardNo', width: 130 },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Call Date', dataIndex: 'callDate', key: 'callDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110 },
  { title: 'Technician', dataIndex: 'technician', key: 'technician' },
];

const FourthCallLogPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<FourthCallLogRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/service/fourth-call-log', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Fourth Call Log">
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

export default FourthCallLogPage;
