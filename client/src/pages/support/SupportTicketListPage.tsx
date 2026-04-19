import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SupportTicket {
  id: number;
  ticketNumber: string;
  subject: string;
  category: string;
  priority: string;
  assignedTo: string;
  status: string;
  createdDate: string;
}

const priorityColor: Record<string, string> = { Low: 'blue', Medium: 'orange', High: 'red', Critical: 'magenta' };
const statusColor: Record<string, string> = { Open: 'blue', InProgress: 'orange', Resolved: 'green', Closed: 'default', Escalated: 'red' };

const columns = [
  { title: 'Ticket #', dataIndex: 'ticketNumber', key: 'ticketNumber', width: 120 },
  { title: 'Subject', dataIndex: 'subject', key: 'subject', ellipsis: true },
  { title: 'Category', dataIndex: 'category', key: 'category' },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', render: (p: string) => <Tag color={priorityColor[p] || 'default'}>{p}</Tag> },
  { title: 'Assigned To', dataIndex: 'assignedTo', key: 'assignedTo' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
  { title: 'Created', dataIndex: 'createdDate', key: 'createdDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
];

const SupportTicketListPage: React.FC = () => {
  const [data, setData] = useState<SupportTicket[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/support/ticket', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<SupportTicket>
      title="Support Tickets" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Ticket"
    />
  );
};

export default SupportTicketListPage;
