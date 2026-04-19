import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Task {
  id: number;
  title: string;
  assignedTo: string;
  priority: string;
  dueDate: string;
  status: string;
}

const priorityColor: Record<string, string> = { Low: 'blue', Medium: 'orange', High: 'red', Critical: 'magenta' };
const statusColor: Record<string, string> = { Todo: 'default', InProgress: 'blue', Completed: 'green', Overdue: 'red', OnHold: 'orange' };

const columns = [
  { title: 'Title', dataIndex: 'title', key: 'title' },
  { title: 'Assigned To', dataIndex: 'assignedTo', key: 'assignedTo' },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', render: (p: string) => <Tag color={priorityColor[p] || 'default'}>{p}</Tag> },
  { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const TaskListPage: React.FC = () => {
  const [data, setData] = useState<Task[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/task/task', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Task>
      title="Tasks" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Task"
    />
  );
};

export default TaskListPage;
