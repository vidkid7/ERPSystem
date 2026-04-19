import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space, Tag } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LoanDetailsPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Borrower', dataIndex: 'borrower', key: 'borrower' },
    { title: 'Installment No', dataIndex: 'installmentNo', key: 'installmentNo' },
    { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate' },
    { title: 'Principal', dataIndex: 'principal', key: 'principal' },
    { title: 'Interest', dataIndex: 'interest', key: 'interest' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Paid' ? 'green' : v === 'Overdue' ? 'red' : 'orange'}>{v}</Tag> },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/loan-details'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Loan Details" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LoanDetailsPage;
