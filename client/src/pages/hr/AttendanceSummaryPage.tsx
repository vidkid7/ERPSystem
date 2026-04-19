import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const AttendanceSummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Present', dataIndex: 'present', key: 'present' },
    { title: 'Absent', dataIndex: 'absent', key: 'absent' },
    { title: 'Leave', dataIndex: 'leave', key: 'leave' },
    { title: 'Total Days', dataIndex: 'totalDays', key: 'totalDays' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/attendance-summary'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Attendance Summary" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="employee" size="small" />
    </Card>
  );
};
export default AttendanceSummaryPage;
