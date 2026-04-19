import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const StockJournalListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Journal No', dataIndex: 'journalNo', key: 'journalNo' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Type', dataIndex: 'type', key: 'type' },
    { title: 'Items', dataIndex: 'items', key: 'items', align: 'right' as const },
    { title: 'Total', dataIndex: 'total', key: 'total', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/stockjournal');
      setData(res.data.data || res.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Stock Journal">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default StockJournalListPage;
