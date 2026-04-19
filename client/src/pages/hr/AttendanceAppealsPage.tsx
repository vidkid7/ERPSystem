import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space, Tag } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const AttendanceAppealsPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Appeal Reason', dataIndex: 'appealReason', key: 'appealReason' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Approved' ? 'green' : v === 'Rejected' ? 'red' : 'orange'}>{v}</Tag> },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/attendance-appeals'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Attendance Appeals" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default AttendanceAppealsPage;
