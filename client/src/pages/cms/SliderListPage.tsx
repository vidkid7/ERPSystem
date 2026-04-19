import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Slider {
  id: number;
  title: string;
  imageUrl: string;
  sortOrder: number;
  isActive: boolean;
}

const columns = [
  { title: 'Title', dataIndex: 'title', key: 'title' },
  { title: 'Image URL', dataIndex: 'imageUrl', key: 'imageUrl', ellipsis: true },
  { title: 'Order', dataIndex: 'sortOrder', key: 'sortOrder', width: 80 },
  { title: 'Active', dataIndex: 'isActive', key: 'isActive', render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Yes' : 'No'}</Tag> },
];

const SliderListPage: React.FC = () => {
  const [data, setData] = useState<Slider[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get('/cms/slider', { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Slider>
      title="Sliders" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Slider"
    />
  );
};

export default SliderListPage;
