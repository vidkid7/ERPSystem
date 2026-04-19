import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Patient } from '../../types';

const columns = [
  { title: 'Patient #', dataIndex: 'patientNumber', key: 'patientNumber', width: 140 },
  { title: 'Name', dataIndex: 'fullName', key: 'fullName' },
  { title: 'Gender', dataIndex: 'gender', key: 'gender', width: 80 },
  { title: 'Phone', dataIndex: 'phone', key: 'phone' },
  { title: 'DOB', dataIndex: 'dateOfBirth', key: 'dateOfBirth', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
];

const PatientListPage: React.FC = () => {
  const [data, setData] = useState<Patient[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/hms/patient', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Patient>
      title="Patients" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Register Patient"
    />
  );
};

export default PatientListPage;
