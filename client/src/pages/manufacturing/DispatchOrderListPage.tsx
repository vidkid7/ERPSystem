import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space, Tag } from 'antd';
import api from '../../services/api';

const statusColors: Record<string, string> = {
  Pending: 'orange',
  Dispatched: 'blue',
  Delivered: 'green',
  Cancelled: 'red',
};

const DispatchOrderListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Order No', dataIndex: 'orderNo', key: 'orderNo' },
    { title: 'Customer', dataIndex: 'customer', key: 'customer' },
    { title: 'Dispatch Date', dataIndex: 'dispatchDate', key: 'dispatchDate' },
    { title: 'Items', dataIndex: 'items', key: 'items', align: 'right' as const },
    {
      title: 'Status', dataIndex: 'status', key: 'status',
      render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
    },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/dispatchorder');
      setData(res.data.data || res.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Dispatch Orders">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default DispatchOrderListPage;
