import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface LabReport {
  id: number;
  reportNumber: string;
  patientName: string;
  testName: string;
  resultDate: string;
  status: string;
}

const statusColor: Record<string, string> = { Pending: 'orange', Completed: 'green', Verified: 'blue', Delivered: 'default' };

const columns = [
  { title: 'Report #', dataIndex: 'reportNumber', key: 'reportNumber', width: 120 },
  { title: 'Patient', dataIndex: 'patientName', key: 'patientName' },
  { title: 'Test', dataIndex: 'testName', key: 'testName' },
  { title: 'Result Date', dataIndex: 'resultDate', key: 'resultDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const LabReportListPage: React.FC = () => {
  const [data, setData] = useState<LabReport[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/lab/report', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<LabReport>
      title="Lab Reports" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default LabReportListPage;
