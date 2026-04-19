import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesReturn {
  id: number;
  returnNo: string;
  date: string;
  customerName: string;
  invoiceRef: string;
  amount: number;
  status: string;
}

const statusColor: Record<string, string> = {
  Draft: 'orange', Approved: 'green', Received: 'blue', Cancelled: 'red',
};

const columns = [
  { title: 'Return No', dataIndex: 'returnNo', key: 'returnNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Customer', dataIndex: 'customerName', key: 'customerName' },
  { title: 'Invoice Ref', dataIndex: 'invoiceRef', key: 'invoiceRef', width: 150 },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 130,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const SalesReturnListPage: React.FC = () => {
  const [data, setData] = useState<SalesReturn[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/sales/return', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<SalesReturn>
      title="Sales Returns" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default SalesReturnListPage;
