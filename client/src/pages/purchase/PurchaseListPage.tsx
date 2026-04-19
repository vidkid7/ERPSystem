import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { PurchaseInvoice } from '../../types';

const columns = [
  { title: 'Invoice #', dataIndex: 'invoiceNumber', key: 'invoiceNumber' },
  { title: 'Date', dataIndex: 'invoiceDate', key: 'invoiceDate', render: (v: string) => new Date(v).toLocaleDateString() },
  { title: 'Vendor', dataIndex: 'vendorName', key: 'vendorName' },
  { title: 'Amount', dataIndex: 'totalAmount', key: 'totalAmount', render: (v: number) => v?.toFixed(2) },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={s === 'Posted' ? 'green' : 'blue'}>{s}</Tag> },
];

const PurchaseListPage: React.FC = () => {
  const [data, setData] = useState<PurchaseInvoice[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/purchase/purchaseinvoice', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<PurchaseInvoice>
      title="Purchase Invoices" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Purchase"
    />
  );
};

export default PurchaseListPage;
