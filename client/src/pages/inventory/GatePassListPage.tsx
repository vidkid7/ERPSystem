import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface GatePass {
  id: number;
  gatePassNo: string;
  date: string;
  type: string;
  vehicleNo: string;
  status: string;
}

const typeColor: Record<string, string> = { Inward: 'blue', Outward: 'orange' };
const statusColor: Record<string, string> = { Draft: 'orange', Approved: 'green', Cancelled: 'red' };

const columns = [
  { title: 'Gate Pass No', dataIndex: 'gatePassNo', key: 'gatePassNo', width: 150 },
  { title: 'Date', dataIndex: 'date', key: 'date', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Type', dataIndex: 'type', key: 'type', width: 110,
    render: (v: string) => <Tag color={typeColor[v] || 'default'}>{v}</Tag> },
  { title: 'Vehicle No', dataIndex: 'vehicleNo', key: 'vehicleNo', width: 140 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const GatePassListPage: React.FC = () => {
  const [data, setData] = useState<GatePass[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/gate-pass', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<GatePass>
      title="Gate Passes" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default GatePassListPage;
