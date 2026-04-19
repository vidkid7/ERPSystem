import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ProductScheme {
  id: number;
  name: string;
  schemeType: string;
  discountPercent: number;
  startDate: string;
  endDate: string;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Scheme Type', dataIndex: 'schemeType', key: 'schemeType', width: 140 },
  { title: 'Discount %', dataIndex: 'discountPercent', key: 'discountPercent', width: 120,
    render: (v: number) => v != null ? `${v.toFixed(2)}%` : '-',
  },
  { title: 'Start Date', dataIndex: 'startDate', key: 'startDate', width: 130,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
  { title: 'End Date', dataIndex: 'endDate', key: 'endDate', width: 130,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
];

const ProductSchemeListPage: React.FC = () => {
  const [data, setData] = useState<ProductScheme[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/productscheme', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<ProductScheme>
      title="Product Schemes" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default ProductSchemeListPage;
