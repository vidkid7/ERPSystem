import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const LoanReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Loan No', dataIndex: 'loanNo', key: 'loanNo' },
    { title: 'Borrower', dataIndex: 'borrower', key: 'borrower' },
    { title: 'Loan Type', dataIndex: 'loanType', key: 'loanType' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Interest Rate', dataIndex: 'interestRate', key: 'interestRate', align: 'right' as const },
    { title: 'Start Date', dataIndex: 'startDate', key: 'startDate' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/loans');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Loan Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default LoanReportPage;
