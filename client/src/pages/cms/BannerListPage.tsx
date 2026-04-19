import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Banner {
  id: number;
  title: string;
  position: string;
  startDate: string;
  endDate: string;
  isActive: boolean;
}

const columns = [
  { title: 'Title', dataIndex: 'title', key: 'title' },
  { title: 'Position', dataIndex: 'position', key: 'position' },
  { title: 'Start Date', dataIndex: 'startDate', key: 'startDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'End Date', dataIndex: 'endDate', key: 'endDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'Active', dataIndex: 'isActive', key: 'isActive', render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Yes' : 'No'}</Tag> },
];

const BannerListPage: React.FC = () => {
  const [data, setData] = useState<Banner[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/cms/banner', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Banner>
      title="Banners" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Banner"
    />
  );
};

export default BannerListPage;
