import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Warranty {
  id: number;
  product: string;
  customer: string;
  purchaseDate: string;
  warrantyExpires: string;
  status: string;
}

const statusColor: Record<string, string> = { Active: 'green', Expired: 'red', 'Expiring Soon': 'orange' };

const columns = [
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Purchase Date', dataIndex: 'purchaseDate', key: 'purchaseDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Warranty Expires', dataIndex: 'warrantyExpires', key: 'warrantyExpires', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 130, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const WarrantyListPage: React.FC = () => {
  const [data, setData] = useState<Warranty[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/warranty', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Warranty>
      title="Warranties" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Warranty"
    />
  );
};

export default WarrantyListPage;
