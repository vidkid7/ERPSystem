import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Indent {
  id: number;
  indentNo: string;
  date: string;
  requestedBy: string;
  priority: string;
  status: string;
}

const priorityColor: Record<string, string> = {
  Low: 'default', Normal: 'blue', High: 'orange', Urgent: 'red',
};

const statusColor: Record<string, string> = {
  Draft: 'orange', Pending: 'orange', Approved: 'green', Rejected: 'red', Completed: 'green',
};

const columns = [
  { title: 'Indent No', dataIndex: 'indentNo', key: 'indentNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Requested By', dataIndex: 'requestedBy', key: 'requestedBy' },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', width: 110,
    render: (v: string) => <Tag color={priorityColor[v] || 'default'}>{v}</Tag> },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const IndentListPage: React.FC = () => {
  const [data, setData] = useState<Indent[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/indent', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Indent>
      title="Material Indents" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default IndentListPage;
