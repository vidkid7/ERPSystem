import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Sample {
  id: number;
  sampleNumber: string;
  patientName: string;
  testName: string;
  collectionDate: string;
  status: string;
}

const statusColor: Record<string, string> = { Collected: 'blue', Processing: 'orange', Completed: 'green', Rejected: 'red' };

const columns = [
  { title: 'Sample #', dataIndex: 'sampleNumber', key: 'sampleNumber', width: 120 },
  { title: 'Patient', dataIndex: 'patientName', key: 'patientName' },
  { title: 'Test', dataIndex: 'testName', key: 'testName' },
  { title: 'Collection Date', dataIndex: 'collectionDate', key: 'collectionDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const SampleListPage: React.FC = () => {
  const [data, setData] = useState<Sample[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/lab/sample', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Sample>
      title="Sample Collections" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Sample"
    />
  );
};

export default SampleListPage;
