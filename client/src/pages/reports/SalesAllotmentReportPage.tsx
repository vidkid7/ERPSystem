import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const SalesAllotmentReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Allotment No', dataIndex: 'allotmentNo', key: 'allotmentNo' },
    { title: 'Customer', dataIndex: 'customer', key: 'customer' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Allotted Qty', dataIndex: 'allottedQty', key: 'allottedQty', align: 'right' as const },
    { title: 'Delivered Qty', dataIndex: 'deliveredQty', key: 'deliveredQty', align: 'right' as const },
    { title: 'Pending Qty', dataIndex: 'pendingQty', key: 'pendingQty', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/sales-allotment');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Sales Allotment Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default SalesAllotmentReportPage;
