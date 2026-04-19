import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import { useNavigate } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesQuotation {
  id: number;
  quotationNo: string;
  date: string;
  customerName: string;
  totalAmount: number;
  status: string;
  validUntil: string;
}

const statusColor: Record<string, string> = {
  Draft: 'orange', Sent: 'blue', Approved: 'green', Rejected: 'red',
};

const columns = [
  { title: 'Quotation No', dataIndex: 'quotationNo', key: 'quotationNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Customer', dataIndex: 'customerName', key: 'customerName' },
  { title: 'Total Amount', dataIndex: 'totalAmount', key: 'totalAmount', width: 140,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Valid Until', dataIndex: 'validUntil', key: 'validUntil', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
];

const SalesQuotationListPage: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<SalesQuotation[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/sales/quotation', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<SalesQuotation>
      title="Sales Quotations" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
      onAdd={() => navigate('/sales/quotation/new')}
      addButtonText="New Quotation"
    />
  );
};

export default SalesQuotationListPage;
