import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Module {
  id: number;
  name: string;
  code: string;
  icon: string;
  displayOrder: number;
  isEnabled: boolean;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 120 },
  { title: 'Icon', dataIndex: 'icon', key: 'icon', width: 120 },
  { title: 'Display Order', dataIndex: 'displayOrder', key: 'displayOrder', width: 130 },
  {
    title: 'Enabled', dataIndex: 'isEnabled', key: 'isEnabled', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Enabled' : 'Disabled'}</Tag>,
  },
];

const ModuleListPage: React.FC = () => {
  const [data, setData] = useState<Module[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/module', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Module>
      title="Modules" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default ModuleListPage;
