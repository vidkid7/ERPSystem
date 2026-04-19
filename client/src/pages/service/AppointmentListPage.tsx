import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Appointment {
  id: number;
  appointmentNumber: string;
  customerName: string;
  appointmentDate: string;
  timeSlot: string;
  serviceType: string;
  status: string;
}

const statusColor: Record<string, string> = { Scheduled: 'blue', Confirmed: 'green', Completed: 'green', Cancelled: 'red', NoShow: 'orange' };

const columns = [
  { title: 'Appointment #', dataIndex: 'appointmentNumber', key: 'appointmentNumber', width: 140 },
  { title: 'Customer', dataIndex: 'customerName', key: 'customerName' },
  { title: 'Date', dataIndex: 'appointmentDate', key: 'appointmentDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Time Slot', dataIndex: 'timeSlot', key: 'timeSlot', width: 120 },
  { title: 'Service Type', dataIndex: 'serviceType', key: 'serviceType' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const AppointmentListPage: React.FC = () => {
  const [data, setData] = useState<Appointment[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/appointment', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Appointment>
      title="Appointments" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Appointment"
    />
  );
};

export default AppointmentListPage;
