import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const IRDApiLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Invoice No', dataIndex: 'invoiceNo', key: 'invoiceNo' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Response', dataIndex: 'response', key: 'response' },
    { title: 'Sent Date', dataIndex: 'sentDate', key: 'sentDate' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/ird-api-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="IRD API Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default IRDApiLogPage;
