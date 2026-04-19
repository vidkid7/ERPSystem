import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetInsurance {
  id: number;
  asset: string;
  insurer: string;
  policyNo: string;
  premium: number;
  expiryDate: string;
}

const columns = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Insurer', dataIndex: 'insurer', key: 'insurer' },
  { title: 'Policy No', dataIndex: 'policyNo', key: 'policyNo', width: 140 },
  { title: 'Premium', dataIndex: 'premium', key: 'premium', align: 'right' as const, width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Expiry Date', dataIndex: 'expiryDate', key: 'expiryDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 120 },
];

const AssetInsurancePage: React.FC = () => {
  const [data, setData] = useState<AssetInsurance[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/asset/insurance', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<AssetInsurance>
      title="Asset Insurance" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Insurance"
    />
  );
};

export default AssetInsurancePage;
