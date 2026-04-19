import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';

const { RangePicker } = DatePicker;

interface DairyPurchaseReportPageRow {
  id: number;
  description: string;
  quantity: number;
  amount: number;
}

const DairyPurchaseReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<DairyPurchaseReportPageRow[]>([]);

  const columns: ColumnsType<DairyPurchaseReportPageRow> = [
    { title: '#', dataIndex: 'id', key: 'id', width: 60 },
    { title: 'Description', dataIndex: 'description', key: 'description' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', align: 'right' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' },
  ];

  const handleSearch = () => {
    setLoading(true);
    setTimeout(() => { setData([]); setLoading(false); }, 300);
  };

  return (
    <Card title="Dairy Purchase Report">
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

export default DairyPurchaseReportPage;
