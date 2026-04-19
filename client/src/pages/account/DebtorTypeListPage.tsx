import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface DebtorType {
  id: number;
  name: string;
  code: string;
  creditDays: number;
  creditLimit: number;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 120 },
  { title: 'Credit Days', dataIndex: 'creditDays', key: 'creditDays', width: 130 },
  { title: 'Credit Limit', dataIndex: 'creditLimit', key: 'creditLimit', width: 140,
    render: (v: number) => v != null ? v.toFixed(2) : '-',
  },
];

const DebtorTypeListPage: React.FC = () => {
  const [data, setData] = useState<DebtorType[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/debtortype', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<DebtorType>
      title="Debtor Types" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default DebtorTypeListPage;
