import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetLocation {
  id: number;
  locationCode: string;
  locationName: string;
  department: string;
  building: string;
}

const columns = [
  { title: 'Location Code', dataIndex: 'locationCode', key: 'locationCode', width: 140 },
  { title: 'Location Name', dataIndex: 'locationName', key: 'locationName' },
  { title: 'Department', dataIndex: 'department', key: 'department' },
  { title: 'Building', dataIndex: 'building', key: 'building' },
];

const AssetLocationPage: React.FC = () => {
  const [data, setData] = useState<AssetLocation[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/asset/location', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<AssetLocation>
      title="Asset Locations" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Location"
    />
  );
};

export default AssetLocationPage;
