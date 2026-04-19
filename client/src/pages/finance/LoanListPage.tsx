import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Loan {
  id: number;
  loanNumber: string;
  customerName: string;
  loanAmount: number;
  interestRate: number;
  tenureMonths: number;
  emiAmount: number;
  status: string;
}

const statusColor: Record<string, string> = { Active: 'green', Closed: 'default', Pending: 'orange', Defaulted: 'red', Approved: 'blue' };

const columns = [
  { title: 'Loan #', dataIndex: 'loanNumber', key: 'loanNumber', width: 120 },
  { title: 'Customer', dataIndex: 'customerName', key: 'customerName' },
  { title: 'Amount', dataIndex: 'loanAmount', key: 'loanAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Interest %', dataIndex: 'interestRate', key: 'interestRate', render: (v: number) => `${v}%`, width: 100 },
  { title: 'Tenure', dataIndex: 'tenureMonths', key: 'tenureMonths', render: (v: number) => `${v} months`, width: 110 },
  { title: 'EMI', dataIndex: 'emiAmount', key: 'emiAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const LoanListPage: React.FC = () => {
  const [data, setData] = useState<Loan[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/finance/loan', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Loan>
      title="Loans" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Loan"
    />
  );
};

export default LoanListPage;
