import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const UserWiseLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'User', dataIndex: 'user', key: 'user' },
    { title: 'Module', dataIndex: 'module', key: 'module' },
    { title: 'Action', dataIndex: 'action', key: 'action' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'IP Address', dataIndex: 'ipAddress', key: 'ipAddress' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/user-wise-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="User Wise Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default UserWiseLogPage;
