import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Select } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import api from '../../services/api';

interface AssetDepreciationRow {
  id: number;
  asset: string;
  purchaseValue: number;
  rate: number;
  depreciation: number;
  bookValue: number;
}

const columns: ColumnsType<AssetDepreciationRow> = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Purchase Value', dataIndex: 'purchaseValue', key: 'purchaseValue', align: 'right', width: 140, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Rate (%)', dataIndex: 'rate', key: 'rate', align: 'right', width: 100, render: (v: number) => v?.toFixed(2) },
  { title: 'Depreciation', dataIndex: 'depreciation', key: 'depreciation', align: 'right', width: 130, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Book Value', dataIndex: 'bookValue', key: 'bookValue', align: 'right', width: 130, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const AssetDepreciationPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetDepreciationRow[]>([]);
  const [year, setYear] = useState<number>(new Date().getFullYear());

  const handleSearch = async () => {
    setLoading(true);
    try {
      const res = await api.get('/asset/depreciation', { params: { year } });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  const years = Array.from({ length: 10 }, (_, i) => new Date().getFullYear() - i);

  return (
    <Card title="Asset Depreciation">
      <Space style={{ marginBottom: 16 }} wrap>
        <Select value={year} onChange={setYear} style={{ width: 120 }}
          options={years.map(y => ({ value: y, label: y.toString() }))} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 700 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default AssetDepreciationPage;
