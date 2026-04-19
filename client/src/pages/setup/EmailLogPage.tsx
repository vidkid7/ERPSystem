import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const EmailLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Recipient', dataIndex: 'recipient', key: 'recipient' },
    { title: 'Subject', dataIndex: 'subject', key: 'subject' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Sent Date', dataIndex: 'sentDate', key: 'sentDate' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/email-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Email Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default EmailLogPage;
