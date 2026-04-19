import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const StockSummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Unit', dataIndex: 'unit', key: 'unit' },
    { title: 'Opening Stock', dataIndex: 'openingStock', key: 'openingStock', align: 'right' as const },
    { title: 'Inward', dataIndex: 'inward', key: 'inward', align: 'right' as const },
    { title: 'Outward', dataIndex: 'outward', key: 'outward', align: 'right' as const },
    { title: 'Closing Stock', dataIndex: 'closingStock', key: 'closingStock', align: 'right' as const },
    { title: 'Value', dataIndex: 'value', key: 'value', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/stock-summary');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Stock Summary">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default StockSummaryPage;
