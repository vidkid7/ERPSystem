import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const JobLogPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Job Name', dataIndex: 'jobName', key: 'jobName' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Start Time', dataIndex: 'startTime', key: 'startTime' },
    { title: 'End Time', dataIndex: 'endTime', key: 'endTime' },
    { title: 'Error', dataIndex: 'error', key: 'error' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/job-log'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Job Log" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default JobLogPage;
