import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface ServiceRemainderRow {
  id: number;
  customer: string;
  product: string;
  lastService: string;
  nextDue: string;
  contact: string;
}

const columns: ColumnsType<ServiceRemainderRow> = [
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Last Service', dataIndex: 'lastService', key: 'lastService', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Next Due', dataIndex: 'nextDue', key: 'nextDue', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Contact', dataIndex: 'contact', key: 'contact', width: 140 },
];

const ServiceRemainderPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<ServiceRemainderRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/service/remainder', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Service Remainder">
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

export default ServiceRemainderPage;
