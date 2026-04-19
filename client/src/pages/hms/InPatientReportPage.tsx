import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const InPatientReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Ward', dataIndex: 'ward', key: 'ward' },
    { title: 'Bed', dataIndex: 'bed', key: 'bed' },
    { title: 'Admission Date', dataIndex: 'admissionDate', key: 'admissionDate' },
    { title: 'Doctor', dataIndex: 'doctor', key: 'doctor' },
    { title: 'Days', dataIndex: 'days', key: 'days' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/in-patient-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="In-Patient Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default InPatientReportPage;
