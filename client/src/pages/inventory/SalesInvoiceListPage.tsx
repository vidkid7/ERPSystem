import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface SalesInvoice { id: number; invoiceNo: string; date: string; party: string; amount: number; status: string; }

const SalesInvoiceListPage: React.FC = () => {
  const [data, setData] = useState<SalesInvoice[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Invoice No', dataIndex: 'invoiceNo', key: 'invoiceNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/sales-invoices').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Sales Invoices" columns={columns} dataSource={data} loading={loading} />;
};
export default SalesInvoiceListPage;
