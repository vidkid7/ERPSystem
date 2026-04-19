import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const OPDReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Patients Count', dataIndex: 'patientsCount', key: 'patientsCount' },
    { title: 'Revenue', dataIndex: 'revenue', key: 'revenue' },
    { title: 'Doctor Wise', dataIndex: 'doctorWise', key: 'doctorWise' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/opd-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="OPD Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="date" size="small" />
    </Card>
  );
};
export default OPDReportPage;
