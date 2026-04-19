import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Consumption { id: number; reference: string; date: string; product: string; quantity: number; godown: string; }

const ConsumptionListPage: React.FC = () => {
  const [data, setData] = useState<Consumption[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Reference', dataIndex: 'reference', key: 'reference' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', align: 'right' as const },
    { title: 'Godown', dataIndex: 'godown', key: 'godown' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/consumptions').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Consumption Entries" columns={columns} dataSource={data} loading={loading} />;
};
export default ConsumptionListPage;
