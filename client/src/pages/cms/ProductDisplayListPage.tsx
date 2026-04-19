import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ProductDisplay {
  id: number;
  product: string;
  displayOrder: number;
  isFeatured: boolean;
  isPublished: boolean;
}

const columns = [
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Display Order', dataIndex: 'displayOrder', key: 'displayOrder', width: 140 },
  { title: 'Featured', dataIndex: 'isFeatured', key: 'isFeatured', width: 110,
    render: (v: boolean) => <Tag color={v ? 'blue' : 'default'}>{v ? 'Yes' : 'No'}</Tag>,
  },
  { title: 'Published', dataIndex: 'isPublished', key: 'isPublished', width: 110,
    render: (v: boolean) => <Tag color={v ? 'green' : 'default'}>{v ? 'Yes' : 'No'}</Tag>,
  },
];

const ProductDisplayListPage: React.FC = () => {
  const [data, setData] = useState<ProductDisplay[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/productdisplay', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<ProductDisplay>
      title="Product Display" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default ProductDisplayListPage;
