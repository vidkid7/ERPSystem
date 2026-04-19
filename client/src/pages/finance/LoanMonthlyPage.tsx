import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LoanMonthlyPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Month', dataIndex: 'month', key: 'month' },
    { title: 'Total Loans', dataIndex: 'totalLoans', key: 'totalLoans' },
    { title: 'Total Collection', dataIndex: 'totalCollection', key: 'totalCollection' },
    { title: 'Outstanding', dataIndex: 'outstanding', key: 'outstanding' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/loan-monthly'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Loan Monthly" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="month" size="small" />
    </Card>
  );
};
export default LoanMonthlyPage;
