import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const HMSCashReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Receipts', dataIndex: 'receipts', key: 'receipts' },
    { title: 'Payments', dataIndex: 'payments', key: 'payments' },
    { title: 'Net', dataIndex: 'net', key: 'net' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/cash-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="HMS Cash Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="date" size="small" />
    </Card>
  );
};
export default HMSCashReportPage;
