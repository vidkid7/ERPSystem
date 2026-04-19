import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Tag } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface AssetAuditRow {
  id: number;
  asset: string;
  expectedLocation: string;
  actualLocation: string;
  status: string;
  auditedBy: string;
}

const statusColor: Record<string, string> = { Matched: 'green', Mismatched: 'red', Missing: 'orange' };

const columns: ColumnsType<AssetAuditRow> = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Expected Location', dataIndex: 'expectedLocation', key: 'expectedLocation' },
  { title: 'Actual Location', dataIndex: 'actualLocation', key: 'actualLocation' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Audited By', dataIndex: 'auditedBy', key: 'auditedBy' },
];

const AssetAuditPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetAuditRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/asset/audit', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Asset Audit Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 750 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default AssetAuditPage;
