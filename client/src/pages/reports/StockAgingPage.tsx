import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, Tag } from 'antd';
import api from '../../services/api';

const { Title } = Typography;

interface StockAgingItem {
  id: number;
  productCode: string;
  productName: string;
  godownName: string;
  quantity: number;
  agingBucket: string;
  daysInStock: number;
  value: number;
}

const bucketColor: Record<string, string> = {
  '0-30': 'green', '31-60': 'blue', '61-90': 'orange', '90+': 'red',
};

const columns = [
  { title: 'Product Code', dataIndex: 'productCode', key: 'productCode', width: 130 },
  { title: 'Product Name', dataIndex: 'productName', key: 'productName' },
  { title: 'Godown', dataIndex: 'godownName', key: 'godownName', width: 150 },
  { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', width: 100,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Value', dataIndex: 'value', key: 'value', width: 130,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Days in Stock', dataIndex: 'daysInStock', key: 'daysInStock', width: 120, align: 'right' as const },
  { title: 'Aging Bucket', dataIndex: 'agingBucket', key: 'agingBucket', width: 120,
    render: (v: string) => <Tag color={bucketColor[v] || 'default'}>{v} days</Tag> },
];

const StockAgingPage: React.FC = () => {
  const [data, setData] = useState<StockAgingItem[]>([]);
  const [loading, setLoading] = useState(false);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reports/stock-aging');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card>
      <Title level={4} style={{ marginBottom: 16 }}>Stock Aging Report</Title>
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        size="middle"
        pagination={{ pageSize: 50, showTotal: (t) => `Total ${t} items` }}
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
};

export default StockAgingPage;
