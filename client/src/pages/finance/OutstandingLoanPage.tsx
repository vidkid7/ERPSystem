import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const OutstandingLoanPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Borrower', dataIndex: 'borrower', key: 'borrower' },
    { title: 'Original Amount', dataIndex: 'originalAmount', key: 'originalAmount' },
    { title: 'Paid', dataIndex: 'paid', key: 'paid' },
    { title: 'Outstanding', dataIndex: 'outstanding', key: 'outstanding' },
    { title: 'Overdue', dataIndex: 'overdue', key: 'overdue' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/outstanding-loan'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Outstanding Loans" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="borrower" size="small" />
    </Card>
  );
};
export default OutstandingLoanPage;
