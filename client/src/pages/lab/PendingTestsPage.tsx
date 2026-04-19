import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface PendingTest {
  id: number;
  sampleId: string;
  patient: string;
  tests: string;
  collectedDate: string;
  priority: string;
  status: string;
}

const priorityColor: Record<string, string> = { Routine: 'default', Urgent: 'orange', STAT: 'red' };
const statusColor: Record<string, string> = { Pending: 'orange', Processing: 'blue', Completed: 'green' };

const columns = [
  { title: 'Sample ID', dataIndex: 'sampleId', key: 'sampleId', width: 120 },
  { title: 'Patient', dataIndex: 'patient', key: 'patient' },
  { title: 'Tests', dataIndex: 'tests', key: 'tests' },
  { title: 'Collected Date', dataIndex: 'collectedDate', key: 'collectedDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 130 },
  { title: 'Priority', dataIndex: 'priority', key: 'priority', width: 100, render: (v: string) => <Tag color={priorityColor[v] || 'default'}>{v}</Tag> },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
];

const PendingTestsPage: React.FC = () => {
  const [data, setData] = useState<PendingTest[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/lab/pending-tests', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<PendingTest>
      title="Pending Tests" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default PendingTestsPage;
