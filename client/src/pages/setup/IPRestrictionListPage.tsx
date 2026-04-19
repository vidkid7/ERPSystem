import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface IPRestriction {
  id: number;
  ipAddress: string;
  description: string;
  isAllowed: boolean;
  createdDate: string;
}

const columns = [
  { title: 'IP Address', dataIndex: 'ipAddress', key: 'ipAddress' },
  { title: 'Description', dataIndex: 'description', key: 'description' },
  {
    title: 'Allowed', dataIndex: 'isAllowed', key: 'isAllowed', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Allowed' : 'Blocked'}</Tag>,
  },
  { title: 'Created Date', dataIndex: 'createdDate', key: 'createdDate', width: 150 },
];

const IPRestrictionListPage: React.FC = () => {
  const [data, setData] = useState<IPRestriction[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/iprestriction', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<IPRestriction>
      title="IP Restrictions" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default IPRestrictionListPage;
