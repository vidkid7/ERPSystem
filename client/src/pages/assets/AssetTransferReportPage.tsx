import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface AssetTransferReportRow {
  id: number;
  asset: string;
  fromDept: string;
  toDept: string;
  transferDate: string;
  approvedBy: string;
}

const columns: ColumnsType<AssetTransferReportRow> = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'From Dept', dataIndex: 'fromDept', key: 'fromDept' },
  { title: 'To Dept', dataIndex: 'toDept', key: 'toDept' },
  { title: 'Transfer Date', dataIndex: 'transferDate', key: 'transferDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 120 },
  { title: 'Approved By', dataIndex: 'approvedBy', key: 'approvedBy' },
];

const AssetTransferReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetTransferReportRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/asset/transfer-report', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Asset Transfer Report">
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

export default AssetTransferReportPage;
