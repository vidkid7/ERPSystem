import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface LabUnitsItem {
  id: number;
  name: string;
  code?: string;
}

const columns = [
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 120 },
];

const LabUnitsListPage: React.FC = () => {
  const [data, setData] = useState<LabUnitsItem[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/api/labunits', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<LabUnitsItem>
      title="Lab Units" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default LabUnitsListPage;
