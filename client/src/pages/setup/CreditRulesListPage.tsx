import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface CreditRule {
  id: number;
  ruleName: string;
  creditDays: number;
  creditLimit: number;
  warningPercent: number;
  isActive: boolean;
}

const columns = [
  { title: 'Rule Name', dataIndex: 'ruleName', key: 'ruleName' },
  { title: 'Credit Days', dataIndex: 'creditDays', key: 'creditDays', width: 120 },
  { title: 'Credit Limit', dataIndex: 'creditLimit', key: 'creditLimit', width: 130 },
  { title: 'Warning %', dataIndex: 'warningPercent', key: 'warningPercent', width: 110 },
  {
    title: 'Status', dataIndex: 'isActive', key: 'isActive', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag>,
  },
];

const CreditRulesListPage: React.FC = () => {
  const [data, setData] = useState<CreditRule[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/creditrule', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<CreditRule>
      title="Credit Rules" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default CreditRulesListPage;
