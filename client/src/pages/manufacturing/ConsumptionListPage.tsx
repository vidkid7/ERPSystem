import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const ConsumptionListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Qty', dataIndex: 'qty', key: 'qty', align: 'right' as const },
    { title: 'Purpose', dataIndex: 'purpose', key: 'purpose' },
    { title: 'Department', dataIndex: 'department', key: 'department' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/consumption');
      setData(res.data.data || res.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Consumption Entries">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default ConsumptionListPage;
