import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesOrder { id: number; orderNo: string; date: string; party: string; amount: number; status: string; }

const SalesOrderListPage: React.FC = () => {
  const [data, setData] = useState<SalesOrder[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Order No', dataIndex: 'orderNo', key: 'orderNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/sales-orders').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Sales Orders" columns={columns} dataSource={data} loading={loading} />;
};
export default SalesOrderListPage;
