import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const DoctorWiseRevenueReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Doctor', dataIndex: 'doctor', key: 'doctor' },
    { title: 'Patients', dataIndex: 'patients', key: 'patients' },
    { title: 'Revenue', dataIndex: 'revenue', key: 'revenue' },
    { title: 'Commission', dataIndex: 'commission', key: 'commission' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/doctor-wise-revenue'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Doctor Revenue" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="doctor" size="small" />
    </Card>
  );
};
export default DoctorWiseRevenueReportPage;
