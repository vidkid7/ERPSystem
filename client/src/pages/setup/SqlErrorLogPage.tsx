import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const SqlErrorLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Error Message', dataIndex: 'errorMessage', key: 'errorMessage' },
    { title: 'Query', dataIndex: 'query', key: 'query' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'User', dataIndex: 'user', key: 'user' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/sql-error-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="SQL Error Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default SqlErrorLogPage;
