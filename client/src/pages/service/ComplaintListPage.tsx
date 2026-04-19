import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { ComplaintTicket } from '../../types';

const priorityColor: Record<string, string> = { Low: 'blue', Medium: 'orange', High: 'red', Critical: 'magenta' };

const columns = [
  { title: 'Ticket #', dataIndex: 'ticketNumber', key: 'ticketNumber' },
  { title: 'Date', dataIndex: 'ticketDate', key: 'ticketDate', render: (v: string) => new Date(v).toLocaleDateString() },
  { title: 'Customer', dataIndex: 'customerName', key: 'customerName' },
  { title: 'Description', dataIndex: 'complaintDescription', key: 'complaintDescription', ellipsis: true },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', render: (p: string) => <Tag color={priorityColor[p] || 'default'}>{p}</Tag> },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={s === 'Open' ? 'blue' : s === 'Closed' ? 'green' : 'orange'}>{s}</Tag> },
];

const ComplaintListPage: React.FC = () => {
  const [data, setData] = useState<ComplaintTicket[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/service/complaintticket', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<ComplaintTicket>
      title="Complaint Tickets" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Complaint"
    />
  );
};

export default ComplaintListPage;
