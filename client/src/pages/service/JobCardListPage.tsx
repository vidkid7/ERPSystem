import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { JobCard } from '../../types';

const statusColor: Record<string, string> = { Pending: 'orange', InProgress: 'blue', Completed: 'green', Cancelled: 'red' };

const columns = [
  { title: 'Job Card #', dataIndex: 'jobCardNumber', key: 'jobCardNumber', width: 130 },
  { title: 'Complaint Ref', dataIndex: 'complaintRef', key: 'complaintRef' },
  { title: 'Technician', dataIndex: 'assignedToName', key: 'assignedToName' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
  { title: 'Start Date', dataIndex: 'jobCardDate', key: 'jobCardDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Completion Date', dataIndex: 'completionDate', key: 'completionDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
];

const JobCardListPage: React.FC = () => {
  const [data, setData] = useState<JobCard[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/service/jobcard', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<JobCard>
      title="Job Cards" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Job Card"
    />
  );
};

export default JobCardListPage;
