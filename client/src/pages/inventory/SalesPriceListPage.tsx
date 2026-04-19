import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesPrice {
  id: number;
  priceType: string;
  effectiveFrom: string;
  effectiveTo: string;
  isActive: boolean;
}

const columns = [
  { title: 'Price Type', dataIndex: 'priceType', key: 'priceType' },
  { title: 'Effective From', dataIndex: 'effectiveFrom', key: 'effectiveFrom', width: 150,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
  { title: 'Effective To', dataIndex: 'effectiveTo', key: 'effectiveTo', width: 150,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
  { title: 'Status', dataIndex: 'isActive', key: 'isActive', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag>,
  },
];

const SalesPriceListPage: React.FC = () => {
  const [data, setData] = useState<SalesPrice[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/salesprice', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<SalesPrice>
      title="Sales Prices" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default SalesPriceListPage;
