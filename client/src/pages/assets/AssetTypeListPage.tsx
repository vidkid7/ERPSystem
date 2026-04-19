import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetType {
  id: number;
  name: string;
  code: string;
  assetGroupName: string;
  depreciationRate: number;
  usefulLifeYears: number;
  depreciationMethod: string;
  isActive: boolean;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 100 },
  { title: 'Asset Group', dataIndex: 'assetGroupName', key: 'assetGroupName' },
  { title: 'Depr. Rate (%)', dataIndex: 'depreciationRate', key: 'depreciationRate', width: 120 },
  { title: 'Useful Life (Yrs)', dataIndex: 'usefulLifeYears', key: 'usefulLifeYears', width: 130 },
  { title: 'Method', dataIndex: 'depreciationMethod', key: 'depreciationMethod', width: 130 },
  { title: 'Status', dataIndex: 'isActive', key: 'isActive', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag>,
  },
];

const AssetTypeListPage: React.FC = () => {
  const [data, setData] = useState<AssetType[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/assettype', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<AssetType>
      title="Asset Types" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default AssetTypeListPage;
