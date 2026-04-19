import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';

const { RangePicker } = DatePicker;

interface BalanceSheetAsTRow {
  id: number;
  description: string;
  amount: number;
}

const BalanceSheetAsTPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<BalanceSheetAsTRow[]>([]);

  const columns: ColumnsType<BalanceSheetAsTRow> = [
    { title: '#', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Description', dataIndex: 'description', key: 'description' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' },
  ];

  const handleSearch = () => {
    setLoading(true);
    setTimeout(() => { setData([]); setLoading(false); }, 300);
  };

  return (
    <Card title="Balance Sheet (As Template)">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button onClick={handleSearch}>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 600 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default BalanceSheetAsTPage;
