import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space, Tag } from 'antd';
import api from '../../services/api';

const BOMListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Product Name', dataIndex: 'productName', key: 'productName' },
    { title: 'BOM Code', dataIndex: 'bomCode', key: 'bomCode' },
    { title: 'Total Components', dataIndex: 'totalComponents', key: 'totalComponents', align: 'right' as const },
    {
      title: 'Active', dataIndex: 'isActive', key: 'isActive',
      render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Yes' : 'No'}</Tag>,
    },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/bom');
      setData(res.data.data || res.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Bill of Materials">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default BOMListPage;
