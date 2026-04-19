import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface JobType {
  id: number;
  jobType: string;
  code: string;
  description: string;
  isActive: boolean;
}

const columns = [
  { title: 'Job Type', dataIndex: 'jobType', key: 'jobType' },
  { title: 'Code', dataIndex: 'code', key: 'code', width: 120 },
  { title: 'Description', dataIndex: 'description', key: 'description' },
  { title: 'Active', dataIndex: 'isActive', key: 'isActive', width: 90, render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Yes' : 'No'}</Tag> },
];

const JobTypeListPage: React.FC = () => {
  const [data, setData] = useState<JobType[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/jobs/job-type', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<JobType>
      title="Job Types" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Job Type"
    />
  );
};

export default JobTypeListPage;
