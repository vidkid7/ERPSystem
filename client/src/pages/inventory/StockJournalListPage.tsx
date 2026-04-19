import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface StockJournal { id: number; journalNo: string; date: string; fromGodown: string; toGodown: string; status: string; }

const StockJournalListPage: React.FC = () => {
  const [data, setData] = useState<StockJournal[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Journal No', dataIndex: 'journalNo', key: 'journalNo' },
    { title: 'From Godown', dataIndex: 'fromGodown', key: 'fromGodown' },
    { title: 'To Godown', dataIndex: 'toGodown', key: 'toGodown' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/stock-journals').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Stock Journals" columns={columns} dataSource={data} loading={loading} />;
};
export default StockJournalListPage;
