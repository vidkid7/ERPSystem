import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetMaintenance {
  id: number;
  asset: string;
  maintenanceType: string;
  date: string;
  cost: number;
  nextDue: string;
}

const columns = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Maintenance Type', dataIndex: 'maintenanceType', key: 'maintenanceType' },
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Cost', dataIndex: 'cost', key: 'cost', align: 'right' as const, width: 110, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Next Due', dataIndex: 'nextDue', key: 'nextDue', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 110 },
];

const AssetMaintenancePage: React.FC = () => {
  const [data, setData] = useState<AssetMaintenance[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/asset/maintenance', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<AssetMaintenance>
      title="Asset Maintenance" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Maintenance"
    />
  );
};

export default AssetMaintenancePage;
