import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const PatientDischargeSummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Admission Date', dataIndex: 'admissionDate', key: 'admissionDate' },
    { title: 'Discharge Date', dataIndex: 'dischargeDate', key: 'dischargeDate' },
    { title: 'Diagnosis', dataIndex: 'diagnosis', key: 'diagnosis' },
    { title: 'Bill', dataIndex: 'bill', key: 'bill' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/discharge-summary'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Discharge Summary" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PatientDischargeSummaryPage;
