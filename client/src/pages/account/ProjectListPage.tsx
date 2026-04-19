import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Project {
  id: number;
  name: string;
  code: string;
  startDate: string;
  endDate: string;
  status: string;
}

const statusColors: Record<string, string> = {
  Active: 'green',
  Completed: 'blue',
  'On Hold': 'orange',
  Cancelled: 'red',
};

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 120 },
  { title: 'Start Date', dataIndex: 'startDate', key: 'startDate', width: 130,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
  { title: 'End Date', dataIndex: 'endDate', key: 'endDate', width: 130,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '-',
  },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
  },
];

const ProjectListPage: React.FC = () => {
  const [data, setData] = useState<Project[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/project', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Project>
      title="Projects" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default ProjectListPage;
