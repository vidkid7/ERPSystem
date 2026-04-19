import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Doctor {
  id: number;
  name: string;
  specialization: string;
  phone: string;
  nmc: string;
  isActive: boolean;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Specialization', dataIndex: 'specialization', key: 'specialization' },
  { title: 'Phone', dataIndex: 'phone', key: 'phone', width: 140 },
  { title: 'NMC Reg.', dataIndex: 'nmc', key: 'nmc', width: 120 },
  { title: 'Status', dataIndex: 'isActive', key: 'isActive', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag>,
  },
];

const DoctorListPage: React.FC = () => {
  const [data, setData] = useState<Doctor[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/doctor', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Doctor>
      title="Doctors" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default DoctorListPage;
