import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PartyWiseRate {
  id: number;
  productName: string;
  partyName: string;
  rate: number;
  effectiveDate: string;
}

const columns = [
  { title: 'Product Name', dataIndex: 'productName', key: 'productName' },
  { title: 'Party Name', dataIndex: 'partyName', key: 'partyName' },
  { title: 'Rate', dataIndex: 'rate', key: 'rate', width: 120,
    render: (v: number) => v != null ? v.toFixed(2) : '-',
  },
  { title: 'Effective Date', dataIndex: 'effectiveDate', key: 'effectiveDate', width: 150,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
];

const PartyWiseRateListPage: React.FC = () => {
  const [data, setData] = useState<PartyWiseRate[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/partywiserate', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<PartyWiseRate>
      title="Party Wise Rates" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default PartyWiseRateListPage;
