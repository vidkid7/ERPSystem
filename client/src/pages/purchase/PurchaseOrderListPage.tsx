import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import { useNavigate } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PurchaseOrder {
  id: number;
  orderNo: string;
  date: string;
  vendorName: string;
  totalAmount: number;
  status: string;
  expectedDelivery: string;
}

const statusColor: Record<string, string> = {
  Draft: 'orange', Approved: 'green', Sent: 'blue', Received: 'green', Cancelled: 'red',
};

const columns = [
  { title: 'Order No', dataIndex: 'orderNo', key: 'orderNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Vendor', dataIndex: 'vendorName', key: 'vendorName' },
  { title: 'Total Amount', dataIndex: 'totalAmount', key: 'totalAmount', width: 140,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Expected Delivery', dataIndex: 'expectedDelivery', key: 'expectedDelivery', width: 150,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
];

const PurchaseOrderListPage: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<PurchaseOrder[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/purchase/order', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<PurchaseOrder>
      title="Purchase Orders" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
      onAdd={() => navigate('/purchase/order/new')}
      addButtonText="New Order"
    />
  );
};

export default PurchaseOrderListPage;
