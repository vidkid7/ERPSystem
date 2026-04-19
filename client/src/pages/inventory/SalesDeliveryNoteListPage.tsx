import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesDeliveryNote { id: number; dnNo: string; date: string; party: string; itemsCount: number; status: string; }

const SalesDeliveryNoteListPage: React.FC = () => {
  const [data, setData] = useState<SalesDeliveryNote[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'DN No', dataIndex: 'dnNo', key: 'dnNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Items Count', dataIndex: 'itemsCount', key: 'itemsCount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/sales-delivery-notes').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Sales Delivery Notes" columns={columns} dataSource={data} loading={loading} />;
};
export default SalesDeliveryNoteListPage;
