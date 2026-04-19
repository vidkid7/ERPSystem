import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Product } from '../../types';

const columns = [
  { title: 'Code', dataIndex: 'code', key: 'code', width: 100 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Group', dataIndex: 'groupName', key: 'groupName' },
  { title: 'Unit', dataIndex: 'unit', key: 'unit', width: 80 },
  { title: 'Selling Price', dataIndex: 'sellingPrice', key: 'sellingPrice', render: (v: number) => v?.toFixed(2) },
  { title: 'Cost Price', dataIndex: 'costPrice', key: 'costPrice', render: (v: number) => v?.toFixed(2) },
];

const ProductListPage: React.FC = () => {
  const [data, setData] = useState<Product[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/product', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Product>
      title="Products" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default ProductListPage;
