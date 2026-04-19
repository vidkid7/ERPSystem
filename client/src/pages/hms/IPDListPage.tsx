import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { IPDAdmission } from '../../types';

const statusColor: Record<string, string> = { Admitted: 'blue', Discharged: 'green', Transferred: 'orange', Critical: 'red' };

const columns = [
  { title: 'Admission #', dataIndex: 'admissionNumber', key: 'admissionNumber', width: 130 },
  { title: 'Patient', dataIndex: 'patientName', key: 'patientName' },
  { title: 'Bed No', dataIndex: 'bedNumber', key: 'bedNumber', width: 90 },
  { title: 'Ward', dataIndex: 'wardName', key: 'wardName' },
  { title: 'Admission Date', dataIndex: 'admissionDate', key: 'admissionDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Discharge Date', dataIndex: 'dischargeDate', key: 'dischargeDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const IPDListPage: React.FC = () => {
  const [data, setData] = useState<IPDAdmission[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/hms/ipd', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<IPDAdmission>
      title="IPD Admissions" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Admission"
    />
  );
};

export default IPDListPage;
