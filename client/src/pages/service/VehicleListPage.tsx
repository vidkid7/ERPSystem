import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Vehicle {
  id: number;
  registrationNo: string;
  customer: string;
  make: string;
  model: string;
  year: number;
  lastService: string;
}

const columns = [
  { title: 'Registration No', dataIndex: 'registrationNo', key: 'registrationNo', width: 140 },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Make', dataIndex: 'make', key: 'make', width: 110 },
  { title: 'Model', dataIndex: 'model', key: 'model', width: 110 },
  { title: 'Year', dataIndex: 'year', key: 'year', width: 80 },
  { title: 'Last Service', dataIndex: 'lastService', key: 'lastService', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
];

const VehicleListPage: React.FC = () => {
  const [data, setData] = useState<Vehicle[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const navigate = useNavigate();

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/vehicle', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Vehicle>
      title="Vehicles" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
      onAdd={() => navigate('/service/vehicle/new')} addButtonText="Add Vehicle"
    />
  );
};

export default VehicleListPage;
