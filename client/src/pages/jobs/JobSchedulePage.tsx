import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface JobSchedule {
  id: number;
  jobName: string;
  schedule: string;
  lastRun: string;
  nextRun: string;
  status: string;
}

const statusColor: Record<string, string> = { Active: 'green', Inactive: 'default', Running: 'blue', Failed: 'red' };

const columns = [
  { title: 'Job Name', dataIndex: 'jobName', key: 'jobName' },
  { title: 'Schedule (Cron)', dataIndex: 'schedule', key: 'schedule', width: 160 },
  { title: 'Last Run', dataIndex: 'lastRun', key: 'lastRun', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 160 },
  { title: 'Next Run', dataIndex: 'nextRun', key: 'nextRun', render: (v: string) => v ? new Date(v).toLocaleString() : '-', width: 160 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 100, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const JobSchedulePage: React.FC = () => {
  const [data, setData] = useState<JobSchedule[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/jobs/schedule', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<JobSchedule>
      title="Job Schedules" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Schedule"
    />
  );
};

export default JobSchedulePage;
