import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import { useNavigate } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Asset {
  id: number;
  assetCode: string;
  name: string;
  assetModelName: string;
  assetCategoryName: string;
  location: string;
  status: string;
  currentValue: number;
  assignedToEmployeeName: string;
  isActive: boolean;
}

const statusColors: Record<string, string> = {
  Available: 'green',
  Issued: 'blue',
  Damaged: 'red',
  Repair: 'orange',
  Disposed: 'default',
};

const columns = [
  { title: 'Asset Code', dataIndex: 'assetCode', key: 'assetCode', width: 130 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Model', dataIndex: 'assetModelName', key: 'assetModelName' },
  { title: 'Category', dataIndex: 'assetCategoryName', key: 'assetCategoryName' },
  { title: 'Location', dataIndex: 'location', key: 'location' },
  { title: 'Current Value', dataIndex: 'currentValue', key: 'currentValue', width: 130,
    render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }),
  },
  { title: 'Assigned To', dataIndex: 'assignedToEmployeeName', key: 'assignedToEmployeeName' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
  },
];

const AssetListPage: React.FC = () => {
  const [data, setData] = useState<Asset[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const navigate = useNavigate();

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/asset', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Asset>
      title="Assets" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
      onAdd={() => navigate('/assets/asset/new')}
    />
  );
};

export default AssetListPage;
