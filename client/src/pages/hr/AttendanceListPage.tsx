import React, { useEffect, useState } from 'react';
import { Tag, DatePicker } from 'antd';
import dayjs from 'dayjs';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Attendance } from '../../types';

const statusColor: Record<string, string> = { Present: 'green', Absent: 'red', Late: 'orange', HalfDay: 'blue' };

const columns = [
  { title: 'Employee', dataIndex: 'employeeName', key: 'employeeName' },
  { title: 'Date', dataIndex: 'attendanceDate', key: 'attendanceDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Check In', dataIndex: 'checkInTime', key: 'checkInTime', render: (v: string) => v || '-' },
  { title: 'Check Out', dataIndex: 'checkOutTime', key: 'checkOutTime', render: (v: string) => v || '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const AttendanceListPage: React.FC = () => {
  const [data, setData] = useState<Attendance[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [date, setDate] = useState<string | undefined>(undefined);

  const fetchData = async (p = page, d = date) => {
    setLoading(true);
    try {
      const res = await api.get('/hr/attendance', { params: { date: d, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <>
      <div style={{ marginBottom: 16 }}>
        <DatePicker
          placeholder="Filter by date"
          onChange={(d) => { const v = d?.toISOString(); setDate(v); fetchData(1, v); }}
          allowClear
        />
      </div>
      <ListPage<Attendance>
        title="Attendance" columns={columns} dataSource={data} loading={loading}
        total={total} page={page}
        onPageChange={(p) => { setPage(p); fetchData(p); }}
        onRefresh={() => fetchData()}
      />
    </>
  );
};

export default AttendanceListPage;
