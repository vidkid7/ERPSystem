import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Bed {
  id: number;
  bedNumber: string;
  wardName: string;
  roomNumber: string;
  type: string;
  status: string;
}

const statusColor: Record<string, string> = { Available: 'green', Occupied: 'red', Maintenance: 'orange' };

const columns = [
  { title: 'Bed No', dataIndex: 'bedNumber', key: 'bedNumber', width: 100 },
  { title: 'Ward', dataIndex: 'wardName', key: 'wardName' },
  { title: 'Room No', dataIndex: 'roomNumber', key: 'roomNumber', width: 100 },
  { title: 'Type', dataIndex: 'type', key: 'type' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const BedListPage: React.FC = () => {
  const [data, setData] = useState<Bed[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/hms/bed', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Bed>
      title="Bed Management" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Bed"
    />
  );
};

export default BedListPage;
