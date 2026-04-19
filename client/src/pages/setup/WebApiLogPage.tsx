import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const WebApiLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'URL', dataIndex: 'url', key: 'url' },
    { title: 'Method', dataIndex: 'method', key: 'method' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Response Time', dataIndex: 'responseTime', key: 'responseTime' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/web-api-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Web API Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default WebApiLogPage;
