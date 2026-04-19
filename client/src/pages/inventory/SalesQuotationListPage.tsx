import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesQuotation { id: number; quoteNo: string; date: string; party: string; validUntil: string; amount: number; }

const SalesQuotationListPage: React.FC = () => {
  const [data, setData] = useState<SalesQuotation[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Quote No', dataIndex: 'quoteNo', key: 'quoteNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Valid Until', dataIndex: 'validUntil', key: 'validUntil' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/sales-quotations').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Sales Quotations" columns={columns} dataSource={data} loading={loading} />;
};
export default SalesQuotationListPage;
