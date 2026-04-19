import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space, InputNumber } from 'antd';
import api from '../../services/api';

const NearExpiryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [days, setDays] = useState<number>(30);

  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Batch No', dataIndex: 'batchNo', key: 'batchNo' },
    { title: 'Qty', dataIndex: 'qty', key: 'qty', align: 'right' as const },
    { title: 'Expiry Date', dataIndex: 'expiryDate', key: 'expiryDate' },
    { title: 'Days to Expiry', dataIndex: 'daysToExpiry', key: 'daysToExpiry', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/near-expiry', { params: { days } });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Near Expiry Products">
      <Space style={{ marginBottom: 16 }} wrap>
        <span>Within days:</span>
        <InputNumber min={1} value={days} onChange={(v) => setDays(v ?? 30)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default NearExpiryPage;
