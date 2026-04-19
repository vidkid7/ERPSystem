import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const PharmacyListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Medicine', dataIndex: 'medicine', key: 'medicine' },
    { title: 'Category', dataIndex: 'category', key: 'category' },
    { title: 'Stock', dataIndex: 'stock', key: 'stock', render: (v: number) => <Tag color={v > 10 ? 'green' : v > 0 ? 'orange' : 'red'}>{v}</Tag> },
    { title: 'Price', dataIndex: 'price', key: 'price' },
    { title: 'Expiry', dataIndex: 'expiry', key: 'expiry' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/pharmacy'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Pharmacy" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PharmacyListPage;
