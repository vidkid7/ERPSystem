import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface EntityNumbering {
  id: number;
  entityName: string;
  numberingMethod: string;
  prefix: string;
  suffix: string;
  nextNumber: number;
}

const columns = [
  { title: 'Entity Name', dataIndex: 'entityName', key: 'entityName' },
  { title: 'Numbering Method', dataIndex: 'numberingMethod', key: 'numberingMethod' },
  { title: 'Prefix', dataIndex: 'prefix', key: 'prefix', width: 100 },
  { title: 'Suffix', dataIndex: 'suffix', key: 'suffix', width: 100 },
  { title: 'Next Number', dataIndex: 'nextNumber', key: 'nextNumber', width: 120 },
];

const EntityNumberingPage: React.FC = () => {
  const [data, setData] = useState<EntityNumbering[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/api/entitynumbering', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<EntityNumbering>
      title="Entity Numbering" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default EntityNumberingPage;
