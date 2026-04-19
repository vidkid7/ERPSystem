import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface Event {
  id: number;
  title: string;
  eventType: string;
  startDate: string;
  endDate: string;
  venue: string;
  isPublished: boolean;
}

const columns = [
  { title: 'Title', dataIndex: 'title', key: 'title' },
  { title: 'Event Type', dataIndex: 'eventType', key: 'eventType' },
  { title: 'Start Date', dataIndex: 'startDate', key: 'startDate', width: 130 },
  { title: 'End Date', dataIndex: 'endDate', key: 'endDate', width: 130 },
  { title: 'Venue', dataIndex: 'venue', key: 'venue' },
  { title: 'Published', dataIndex: 'isPublished', key: 'isPublished', width: 110,
    render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Yes' : 'No'}</Tag>,
  },
];

const EventListPage: React.FC = () => {
  const [data, setData] = useState<Event[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/event', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Event>
      title="Events" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default EventListPage;
