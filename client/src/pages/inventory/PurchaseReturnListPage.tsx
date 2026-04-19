import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PurchaseReturn { id: number; returnNo: string; date: string; supplier: string; amount: number; reason: string; }

const PurchaseReturnListPage: React.FC = () => {
  const [data, setData] = useState<PurchaseReturn[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Return No', dataIndex: 'returnNo', key: 'returnNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Reason', dataIndex: 'reason', key: 'reason' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/purchase-returns').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Purchase Returns" columns={columns} dataSource={data} loading={loading} />;
};
export default PurchaseReturnListPage;
