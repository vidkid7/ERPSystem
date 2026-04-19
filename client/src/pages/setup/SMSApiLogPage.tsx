import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const SMSApiLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Mobile', dataIndex: 'mobile', key: 'mobile' },
    { title: 'Message', dataIndex: 'message', key: 'message' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Sent Date', dataIndex: 'sentDate', key: 'sentDate' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/sms-api-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="SMS API Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default SMSApiLogPage;
