import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PurchaseInvoice { id: number; invoiceNo: string; date: string; supplier: string; amount: number; status: string; }

const PurchaseInvoiceListPage: React.FC = () => {
  const [data, setData] = useState<PurchaseInvoice[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Invoice No', dataIndex: 'invoiceNo', key: 'invoiceNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/purchase-invoices').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Purchase Invoices" columns={columns} dataSource={data} loading={loading} />;
};
export default PurchaseInvoiceListPage;
