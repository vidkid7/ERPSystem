import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { OPDTicket } from '../../types';

const statusColor: Record<string, string> = { Open: 'blue', InProgress: 'orange', Completed: 'green', Cancelled: 'red' };

const columns = [
  { title: 'Ticket #', dataIndex: 'ticketNumber', key: 'ticketNumber', width: 120 },
  { title: 'Patient', dataIndex: 'patientName', key: 'patientName' },
  { title: 'Doctor', dataIndex: 'doctorName', key: 'doctorName' },
  { title: 'Date', dataIndex: 'ticketDate', key: 'ticketDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Diagnosis', dataIndex: 'diagnosis', key: 'diagnosis', ellipsis: true },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const OPDListPage: React.FC = () => {
  const [data, setData] = useState<OPDTicket[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/hms/opd', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<OPDTicket>
      title="OPD Tickets" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New OPD Ticket"
    />
  );
};

export default OPDListPage;
