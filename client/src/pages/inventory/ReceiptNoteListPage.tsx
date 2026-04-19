import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ReceiptNote { id: number; rnNo: string; date: string; supplier: string; items: string; status: string; }

const ReceiptNoteListPage: React.FC = () => {
  const [data, setData] = useState<ReceiptNote[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'RN No', dataIndex: 'rnNo', key: 'rnNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Items', dataIndex: 'items', key: 'items' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/receipt-notes').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Receipt Notes" columns={columns} dataSource={data} loading={loading} />;
};
export default ReceiptNoteListPage;
