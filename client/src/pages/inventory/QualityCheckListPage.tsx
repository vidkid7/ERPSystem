import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface QualityCheck {
  id: number;
  reference: string;
  product: string;
  checkDate: string;
  inspector: string;
  result: string;
}

const resultColor: Record<string, string> = { Pass: 'green', Fail: 'red', Pending: 'orange' };

const columns = [
  { title: 'Reference', dataIndex: 'reference', key: 'reference', width: 130 },
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Check Date', dataIndex: 'checkDate', key: 'checkDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 120 },
  { title: 'Inspector', dataIndex: 'inspector', key: 'inspector' },
  { title: 'Result', dataIndex: 'result', key: 'result', width: 100, render: (v: string) => <Tag color={resultColor[v] || 'default'}>{v}</Tag> },
];

const QualityCheckListPage: React.FC = () => {
  const [data, setData] = useState<QualityCheck[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/quality-check', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<QualityCheck>
      title="Quality Checks" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Quality Check"
    />
  );
};

export default QualityCheckListPage;
