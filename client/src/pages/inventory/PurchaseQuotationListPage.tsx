import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PurchaseQuotation { id: number; quoteNo: string; date: string; supplier: string; validUntil: string; amount: number; }

const PurchaseQuotationListPage: React.FC = () => {
  const [data, setData] = useState<PurchaseQuotation[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Quote No', dataIndex: 'quoteNo', key: 'quoteNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Valid Until', dataIndex: 'validUntil', key: 'validUntil' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/purchase-quotations').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Purchase Quotations" columns={columns} dataSource={data} loading={loading} />;
};
export default PurchaseQuotationListPage;
