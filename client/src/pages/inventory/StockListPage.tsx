import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Stock } from '../../types';

const columns = [
  { title: 'Product', dataIndex: 'productName', key: 'productName' },
  { title: 'Godown', dataIndex: 'godownName', key: 'godownName' },
  { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', render: (v: number) => v?.toFixed(2) },
  { title: 'Cost Price', dataIndex: 'rate', key: 'rate', render: (v: number) => v?.toFixed(2) },
  {
    title: 'Total Value',
    key: 'totalValue',
    render: (_: any, record: Stock) => ((record.quantity || 0) * (record.rate || 0)).toFixed(2),
  },
];

const StockListPage: React.FC = () => {
  const [data, setData] = useState<Stock[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/stock', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Stock>
      title="Current Stock"
      columns={columns}
      dataSource={data}
      loading={loading}
      total={total}
      page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default StockListPage;
