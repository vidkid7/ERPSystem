import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LoginLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'User', dataIndex: 'user', key: 'user' },
    { title: 'Login Time', dataIndex: 'loginTime', key: 'loginTime' },
    { title: 'Logout Time', dataIndex: 'logoutTime', key: 'logoutTime' },
    { title: 'IP', dataIndex: 'ip', key: 'ip' },
    { title: 'Browser', dataIndex: 'browser', key: 'browser' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/login-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Login Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LoginLogPage;
