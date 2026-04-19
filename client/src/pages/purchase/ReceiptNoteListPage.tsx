import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import { useNavigate } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ReceiptNote {
  id: number;
  grnNo: string;
  date: string;
  vendorName: string;
  purchaseOrderRef: string;
  status: string;
}

const statusColor: Record<string, string> = {
  Draft: 'orange', Received: 'green', Partial: 'blue', Cancelled: 'red',
};

const columns = [
  { title: 'GRN No', dataIndex: 'grnNo', key: 'grnNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Vendor', dataIndex: 'vendorName', key: 'vendorName' },
  { title: 'PO Ref', dataIndex: 'purchaseOrderRef', key: 'purchaseOrderRef', width: 150 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const ReceiptNoteListPage: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<ReceiptNote[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/purchase/receipt-note', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<ReceiptNote>
      title="Goods Receipt Notes (GRN)" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
      onAdd={() => navigate('/purchase/receipt-note/new')}
      addButtonText="New GRN"
    />
  );
};

export default ReceiptNoteListPage;
