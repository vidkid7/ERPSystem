import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const IPDReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Admissions', dataIndex: 'admissions', key: 'admissions' },
    { title: 'Discharges', dataIndex: 'discharges', key: 'discharges' },
    { title: 'Revenue', dataIndex: 'revenue', key: 'revenue' },
    { title: 'Occupied Beds', dataIndex: 'occupiedBeds', key: 'occupiedBeds' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/ipd-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="IPD Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="date" size="small" />
    </Card>
  );
};
export default IPDReportPage;
