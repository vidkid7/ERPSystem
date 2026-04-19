import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space, Tag } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LabIntegrationReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Test', dataIndex: 'test', key: 'test' },
    { title: 'Result', dataIndex: 'result', key: 'result' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Completed' ? 'green' : 'orange'}>{v}</Tag> },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/lab-integration'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Lab Integration" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LabIntegrationReportPage;
