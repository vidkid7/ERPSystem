import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const PendingPOSummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'PO No', dataIndex: 'poNo', key: 'poNo' },
    { title: 'Vendor', dataIndex: 'vendor', key: 'vendor' },
    { title: 'Order Date', dataIndex: 'orderDate', key: 'orderDate' },
    { title: 'Ordered Qty', dataIndex: 'orderedQty', key: 'orderedQty', align: 'right' as const },
    { title: 'Received Qty', dataIndex: 'receivedQty', key: 'receivedQty', align: 'right' as const },
    { title: 'Pending Qty', dataIndex: 'pendingQty', key: 'pendingQty', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/pending-po-summary');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Pending PO Summary">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default PendingPOSummaryPage;
