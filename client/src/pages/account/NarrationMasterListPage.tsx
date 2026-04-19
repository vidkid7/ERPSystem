import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface NarrationMaster {
  id: number;
  narration: string;
  voucherType: string;
  isDefault: boolean;
}

const columns = [
  { title: 'Narration', dataIndex: 'narration', key: 'narration' },
  { title: 'Voucher Type', dataIndex: 'voucherType', key: 'voucherType', width: 160 },
  { title: 'Default', dataIndex: 'isDefault', key: 'isDefault', width: 100,
    render: (v: boolean) => <Tag color={v ? 'blue' : 'default'}>{v ? 'Yes' : 'No'}</Tag>,
  },
];

const NarrationMasterListPage: React.FC = () => {
  const [data, setData] = useState<NarrationMaster[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/narrationmaster', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<NarrationMaster>
      title="Narration Masters" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default NarrationMasterListPage;
