import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { LeaveRequest } from '../../types';

const statusColor: Record<string, string> = { Approved: 'green', Pending: 'orange', Rejected: 'red', Cancelled: 'default' };

const columns = [
  { title: 'Employee', dataIndex: 'employeeName', key: 'employeeName' },
  { title: 'Leave Type', dataIndex: 'leaveType', key: 'leaveType' },
  { title: 'From', dataIndex: 'fromDate', key: 'fromDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'To', dataIndex: 'toDate', key: 'toDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Days', dataIndex: 'totalDays', key: 'totalDays', width: 80 },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const LeaveListPage: React.FC = () => {
  const [data, setData] = useState<LeaveRequest[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/hr/leave', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<LeaveRequest>
      title="Leave Requests" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Leave Request"
    />
  );
};

export default LeaveListPage;
