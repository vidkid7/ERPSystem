import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const PatientOutStandingPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Bill Date', dataIndex: 'billDate', key: 'billDate' },
    { title: 'Bill Amount', dataIndex: 'billAmount', key: 'billAmount', align: 'right' as const },
    { title: 'Paid', dataIndex: 'paid', key: 'paid', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/patient-outstanding'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Patient Outstanding" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PatientOutStandingPage;
