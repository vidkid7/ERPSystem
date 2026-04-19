import React, { useState } from 'react';
import { Card, Button, Table, Space, Select, Input } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import api from '../../services/api';

interface AssetScheduleRow {
  id: number;
  year: number | string;
  openingValue: number;
  depreciation: number;
  closingValue: number;
}

const columns: ColumnsType<AssetScheduleRow> = [
  { title: 'Year', dataIndex: 'year', key: 'year', width: 100 },
  { title: 'Opening Value', dataIndex: 'openingValue', key: 'openingValue', align: 'right', width: 140, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Depreciation', dataIndex: 'depreciation', key: 'depreciation', align: 'right', width: 130, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Closing Value', dataIndex: 'closingValue', key: 'closingValue', align: 'right', width: 130, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const AssetSchedulePage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetScheduleRow[]>([]);
  const [assetId, setAssetId] = useState('');

  const handleSearch = async () => {
    setLoading(true);
    try {
      const res = await api.get('/asset/depreciation-schedule', { params: { assetId } });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Depreciation Schedule">
      <Space style={{ marginBottom: 16 }} wrap>
        <Input placeholder="Asset ID / Code" value={assetId} onChange={e => setAssetId(e.target.value)} style={{ width: 200 }} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 550 }} pagination={false} />
    </Card>
  );
};

export default AssetSchedulePage;
