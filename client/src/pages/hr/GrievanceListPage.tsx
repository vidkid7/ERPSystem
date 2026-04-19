import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space, Tag } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const GrievanceListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Type', dataIndex: 'type', key: 'type' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Resolved' ? 'green' : 'orange'}>{v}</Tag> },
    { title: 'Resolved', dataIndex: 'resolved', key: 'resolved' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/grievance-list'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Grievance List" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default GrievanceListPage;
