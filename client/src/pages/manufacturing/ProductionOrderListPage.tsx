import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space, Tag } from 'antd';
import api from '../../services/api';

const statusColors: Record<string, string> = {
  Pending: 'orange',
  InProgress: 'blue',
  Completed: 'green',
  Cancelled: 'red',
};

const ProductionOrderListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Order No', dataIndex: 'orderNo', key: 'orderNo' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Planned Qty', dataIndex: 'plannedQty', key: 'plannedQty', align: 'right' as const },
    { title: 'Completed Qty', dataIndex: 'completedQty', key: 'completedQty', align: 'right' as const },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
    },
    { title: 'Target Date', dataIndex: 'targetDate', key: 'targetDate' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/productionorder');
      setData(res.data.data || res.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Production Orders">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default ProductionOrderListPage;
