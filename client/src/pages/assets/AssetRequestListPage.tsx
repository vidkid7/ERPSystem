import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetRequest {
  id: number;
  assetCode: string;
  name: string;
  assignedToEmployeeName: string;
  location: string;
  status: string;
  currentValue: number;
}

const statusColors: Record<string, string> = {
  Available: 'green', Issued: 'blue', Damaged: 'red', Repair: 'orange', Disposed: 'default',
};

const columns = [
  { title: 'Asset Code', dataIndex: 'assetCode', key: 'assetCode', width: 130 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Assigned To', dataIndex: 'assignedToEmployeeName', key: 'assignedToEmployeeName' },
  { title: 'Location', dataIndex: 'location', key: 'location' },
  { title: 'Current Value', dataIndex: 'currentValue', key: 'currentValue', width: 130,
    render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }),
  },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
  },
];

const AssetRequestListPage: React.FC = () => {
  const [data, setData] = useState<AssetRequest[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/asset', { params: { search: s, page: p, pageSize: 20, status: 'Issued' } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<AssetRequest>
      title="Asset Requests (Issued Assets)" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default AssetRequestListPage;
