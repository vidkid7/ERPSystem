import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space, Tag } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LoanReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Borrower', dataIndex: 'borrower', key: 'borrower' },
    { title: 'Loan Amount', dataIndex: 'loanAmount', key: 'loanAmount' },
    { title: 'Interest Rate', dataIndex: 'interestRate', key: 'interestRate' },
    { title: 'Term', dataIndex: 'term', key: 'term' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Active' ? 'green' : v === 'Closed' ? 'default' : 'orange'}>{v}</Tag> },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/loan-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Loan List Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LoanReportPage;
