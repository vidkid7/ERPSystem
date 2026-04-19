import React, { useState, useEffect } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface BOM { id: number; product: string; version: string; componentsCount: number; active: string; }

const BOMListPage: React.FC = () => {
  const [data, setData] = useState<BOM[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Version', dataIndex: 'version', key: 'version' },
    { title: 'Components Count', dataIndex: 'componentsCount', key: 'componentsCount', align: 'right' as const },
    { title: 'Active', dataIndex: 'active', key: 'active' },
  ];
  useEffect(() => {
    setLoading(true);
    api.get('/inventory/boms').then(r => setData(r.data?.Data || [])).finally(() => setLoading(false));
  }, []);
  return <ListPage title="Bill of Materials" columns={columns} dataSource={data} loading={loading} />;
};
export default BOMListPage;
