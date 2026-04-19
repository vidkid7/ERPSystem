import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Godown } from '../../types';

const columns = [
  { title: 'Code', dataIndex: 'code', key: 'code', width: 100 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Address', dataIndex: 'address', key: 'address' },
  {
    title: 'Active',
    dataIndex: 'isActive',
    key: 'isActive',
    render: (v: boolean) => <Tag color={v !== false ? 'green' : 'red'}>{v !== false ? 'Yes' : 'No'}</Tag>,
  },
];

const GodownListPage: React.FC = () => {
  const [data, setData] = useState<Godown[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/godown', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Godown>
      title="Godowns"
      columns={columns}
      dataSource={data}
      loading={loading}
      total={total}
      page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default GodownListPage;
