import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface AssetDisposalReportRow {
  id: number;
  asset: string;
  disposalDate: string;
  method: string;
  amount: number;
  approvedBy: string;
}

const columns: ColumnsType<AssetDisposalReportRow> = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Disposal Date', dataIndex: 'disposalDate', key: 'disposalDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 120 },
  { title: 'Method', dataIndex: 'method', key: 'method', width: 110 },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Approved By', dataIndex: 'approvedBy', key: 'approvedBy' },
];

const AssetDisposalReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetDisposalReportRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/asset/disposal-report', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Asset Disposal Report">
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

export default AssetDisposalReportPage;
