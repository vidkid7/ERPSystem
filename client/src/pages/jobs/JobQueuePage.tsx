import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface JobQueue {
  id: number;
  jobId: string;
  type: string;
  status: string;
  created: string;
  started: string;
  completed: string;
  error: string;
}

const statusColor: Record<string, string> = { Pending: 'default', Running: 'blue', Completed: 'green', Failed: 'red', Cancelled: 'orange' };

const columns = [
  { title: 'Job ID', dataIndex: 'jobId', key: 'jobId', width: 130 },
  { title: 'Type', dataIndex: 'type', key: 'type', width: 120 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 100, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Created', dataIndex: 'created', key: 'created', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 150 },
  { title: 'Started', dataIndex: 'started', key: 'started', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 150 },
  { title: 'Completed', dataIndex: 'completed', key: 'completed', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 150 },
  { title: 'Error', dataIndex: 'error', key: 'error', ellipsis: true },
];

const JobQueuePage: React.FC = () => {
  const [data, setData] = useState<JobQueue[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/jobs/queue', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<JobQueue>
      title="Job Queue" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default JobQueuePage;
