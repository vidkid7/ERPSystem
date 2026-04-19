import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LoanSchedulePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Opening Balance', dataIndex: 'openingBalance', key: 'openingBalance' },
    { title: 'EMI', dataIndex: 'emi', key: 'emi' },
    { title: 'Principal', dataIndex: 'principal', key: 'principal' },
    { title: 'Interest', dataIndex: 'interest', key: 'interest' },
    { title: 'Closing Balance', dataIndex: 'closingBalance', key: 'closingBalance' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/loan-schedule'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Loan Schedule" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="date" size="small" />
    </Card>
  );
};
export default LoanSchedulePage;
