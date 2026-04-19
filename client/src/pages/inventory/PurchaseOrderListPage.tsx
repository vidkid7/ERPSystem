import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PurchaseOrder { id: number; poNo: string; date: string; supplier: string; amount: number; status: string; }

const PurchaseOrderListPage: React.FC = () => {
  const [data, setData] = useState<PurchaseOrder[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'PO No', dataIndex: 'poNo', key: 'poNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/purchase-orders').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Purchase Orders" columns={columns} dataSource={data} loading={loading} />;
};
export default PurchaseOrderListPage;
