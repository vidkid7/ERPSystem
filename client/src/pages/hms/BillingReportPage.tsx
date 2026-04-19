import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const BillingReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Services', dataIndex: 'services', key: 'services' },
    { title: 'Total Amount', dataIndex: 'totalAmount', key: 'totalAmount' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/billing-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Billing Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default BillingReportPage;
