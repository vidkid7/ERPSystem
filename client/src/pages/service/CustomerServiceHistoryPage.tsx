import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Input } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;
const { Search } = Input;

interface CustomerServiceHistoryRow {
  id: number;
  customer: string;
  product: string;
  date: string;
  issue: string;
  resolution: string;
}

const columns: ColumnsType<CustomerServiceHistoryRow> = [
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Issue', dataIndex: 'issue', key: 'issue' },
  { title: 'Resolution', dataIndex: 'resolution', key: 'resolution' },
];

const CustomerServiceHistoryPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<CustomerServiceHistoryRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);
  const [search, setSearch] = useState('');

  const handleSearch = async (s = search) => {
    setLoading(true);
    try {
      const params: Record<string, string> = { search: s };
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/service/customer-history', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Customer Service History">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Search placeholder="Customer name..." onSearch={(v) => { setSearch(v); handleSearch(v); }} allowClear style={{ width: 220 }} />
        <Button type="primary" onClick={() => handleSearch()} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 800 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default CustomerServiceHistoryPage;
