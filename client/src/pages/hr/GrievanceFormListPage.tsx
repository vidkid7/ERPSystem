import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface GrievanceForm {
  id: number;
  employee: string;
  grievanceType: string;
  status: string;
  submittedDate: string;
}

const statusColor: Record<string, string> = {
  Pending: 'orange', Resolved: 'green', Rejected: 'red', 'In Progress': 'blue',
};

const columns = [
  { title: 'Employee', dataIndex: 'employee', key: 'employee' },
  { title: 'Grievance Type', dataIndex: 'grievanceType', key: 'grievanceType' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 130,
    render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag>,
  },
  { title: 'Submitted Date', dataIndex: 'submittedDate', key: 'submittedDate', width: 150 },
];

const GrievanceFormListPage: React.FC = () => {
  const [data, setData] = useState<GrievanceForm[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/grievanceform', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<GrievanceForm>
      title="Grievance Forms" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default GrievanceFormListPage;
