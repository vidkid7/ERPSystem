import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface LoyaltyPoint {
  id: number;
  memberName: string;
  transactionDate: string;
  transactionType: string;
  points: number;
  balance: number;
}

const typeColor: Record<string, string> = { Earned: 'green', Redeemed: 'orange', Expired: 'red', Adjusted: 'blue' };

const columns = [
  { title: 'Member', dataIndex: 'memberName', key: 'memberName' },
  { title: 'Date', dataIndex: 'transactionDate', key: 'transactionDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Type', dataIndex: 'transactionType', key: 'transactionType', render: (t: string) => <Tag color={typeColor[t] || 'default'}>{t}</Tag> },
  { title: 'Points', dataIndex: 'points', key: 'points', render: (v: number, r: LoyaltyPoint) => <span style={{ color: r.transactionType === 'Earned' ? '#3f8600' : '#cf1322' }}>{v}</span>, align: 'right' as const },
  { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
];

const PointsListPage: React.FC = () => {
  const [data, setData] = useState<LoyaltyPoint[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/loyalty/points', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<LoyaltyPoint>
      title="Membership Points" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default PointsListPage;
