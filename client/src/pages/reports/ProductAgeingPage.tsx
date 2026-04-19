import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const ProductAgeingPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Batch No', dataIndex: 'batchNo', key: 'batchNo' },
    { title: 'Qty', dataIndex: 'qty', key: 'qty', align: 'right' as const },
    { title: 'Value', dataIndex: 'value', key: 'value', align: 'right' as const },
    { title: 'Purchase Date', dataIndex: 'purchaseDate', key: 'purchaseDate' },
    { title: 'Ageing Days', dataIndex: 'ageingDays', key: 'ageingDays', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/product-ageing');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Product Ageing">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default ProductAgeingPage;
