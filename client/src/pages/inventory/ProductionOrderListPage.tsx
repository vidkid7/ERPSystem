import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ProductionOrder { id: number; orderNo: string; product: string; quantity: number; status: string; date: string; }

const ProductionOrderListPage: React.FC = () => {
  const [data, setData] = useState<ProductionOrder[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Order No', dataIndex: 'orderNo', key: 'orderNo' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/production-orders').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Production Orders" columns={columns} dataSource={data} loading={loading} />;
};
export default ProductionOrderListPage;
