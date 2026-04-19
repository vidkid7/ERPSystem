import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Video {
  id: number;
  title: string;
  videoUrl: string;
  category: string;
  isPublished: boolean;
}

const columns = [
  { title: 'Title', dataIndex: 'title', key: 'title' },
  { title: 'Video URL', dataIndex: 'videoUrl', key: 'videoUrl', ellipsis: true },
  { title: 'Category', dataIndex: 'category', key: 'category' },
  { title: 'Published', dataIndex: 'isPublished', key: 'isPublished', width: 110,
    render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Yes' : 'No'}</Tag>,
  },
];

const VideoListPage: React.FC = () => {
  const [data, setData] = useState<Video[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/video', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Video>
      title="Videos" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default VideoListPage;
