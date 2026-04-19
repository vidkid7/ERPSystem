import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Vendor } from '../../types';

const columns = [
  { title: 'Code', dataIndex: 'code', key: 'code', width: 100 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Phone', dataIndex: 'phone', key: 'phone', width: 140 },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  { title: 'PAN No', dataIndex: 'panNo', key: 'panNo', width: 140 },
  { title: 'Status', dataIndex: 'isActive', key: 'isActive', width: 100,
    render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag>,
  },
];

const VendorListPage: React.FC = () => {
  const [data, setData] = useState<Vendor[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/account/vendor', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Vendor>
      title="Vendors" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default VendorListPage;
