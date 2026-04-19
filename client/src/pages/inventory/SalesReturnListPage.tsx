import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesReturn { id: number; returnNo: string; date: string; party: string; amount: number; reason: string; }

const SalesReturnListPage: React.FC = () => {
  const [data, setData] = useState<SalesReturn[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Return No', dataIndex: 'returnNo', key: 'returnNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Reason', dataIndex: 'reason', key: 'reason' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/sales-returns').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Sales Returns" columns={columns} dataSource={data} loading={loading} />;
};
export default SalesReturnListPage;
