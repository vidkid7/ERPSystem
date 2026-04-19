import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Ticket {
  id: number;
  ticketNo: string;
  customer: string;
  issue: string;
  priority: string;
  status: string;
  assignedTo: string;
}

const priorityColor: Record<string, string> = { Low: 'default', Medium: 'blue', High: 'orange', Critical: 'red' };
const statusColor: Record<string, string> = { Open: 'orange', InProgress: 'blue', Resolved: 'green', Closed: 'default' };

const columns = [
  { title: 'Ticket No', dataIndex: 'ticketNo', key: 'ticketNo', width: 120 },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Issue', dataIndex: 'issue', key: 'issue' },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', width: 100, render: (v: string) => <Tag color={priorityColor[v] || 'default'}>{v}</Tag> },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Assigned To', dataIndex: 'assignedTo', key: 'assignedTo' },
];

const TicketListPage: React.FC = () => {
  const [data, setData] = useState<Ticket[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/ticket', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Ticket>
      title="Tickets" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Ticket"
    />
  );
};

export default TicketListPage;
