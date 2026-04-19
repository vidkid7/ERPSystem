import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const PatientReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Doctor', dataIndex: 'doctor', key: 'doctor' },
    { title: 'Diagnosis', dataIndex: 'diagnosis', key: 'diagnosis' },
    { title: 'Bill Amount', dataIndex: 'billAmount', key: 'billAmount' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/patient-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Patient Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PatientReportPage;
