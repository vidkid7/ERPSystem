import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space, InputNumber } from 'antd';
import api from '../../services/api';

const LoanDetailsPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [loanId, setLoanId] = useState<number | null>(null);

  const columns = [
    { title: 'Month', dataIndex: 'month', key: 'month' },
    { title: 'EMI Amount', dataIndex: 'emiAmount', key: 'emiAmount', align: 'right' as const },
    { title: 'Principal', dataIndex: 'principal', key: 'principal', align: 'right' as const },
    { title: 'Interest', dataIndex: 'interest', key: 'interest', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = {};
      if (loanId) params.loanId = loanId;
      const res = await api.get('/reporting/loan-details', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Loan Details – EMI Schedule">
      <Space style={{ marginBottom: 16 }} wrap>
        <span>Loan ID:</span>
        <InputNumber min={1} value={loanId ?? undefined} onChange={(v) => setLoanId(v)} placeholder="Enter Loan ID" />
        <Button type="primary" onClick={fetchData}>Load Schedule</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default LoanDetailsPage;
