import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface DispatchOrder { id: number; doNo: string; date: string; party: string; items: string; driver: string; status: string; }

const DispatchOrderListPage: React.FC = () => {
  const [data, setData] = useState<DispatchOrder[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'DO No', dataIndex: 'doNo', key: 'doNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Items', dataIndex: 'items', key: 'items' },
    { title: 'Driver', dataIndex: 'driver', key: 'driver' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/dispatch-orders').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Dispatch Orders" columns={columns} dataSource={data} loading={loading} />;
};
export default DispatchOrderListPage;
