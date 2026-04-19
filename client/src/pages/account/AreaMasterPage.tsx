import React, { useState, useEffect } from 'react';
import { Card, Table, Button, Space, Input } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import api from '../../services/api';
interface RowType { id: number; [key: string]: any; }
const AreaMasterPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');
  const columns = [
    { title: 'Code', dataIndex: 'code', key: 'code' },
    { title: 'Area Name', dataIndex: 'areaName', key: 'areaName' },
    { title: 'Salesman', dataIndex: 'salesman', key: 'salesman' },
  ];
  const fetchData = async () => {
    setLoading(true);
    try { const res = await api.get('/account/area-master-list'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  useEffect(() => { fetchData(); }, []);
  const filtered = data.filter(r => Object.values(r).some(v => String(v).toLowerCase().includes(search.toLowerCase())));
  return (
    <Card title="Area Master" extra={<Space><Input prefix={<SearchOutlined />} placeholder="Search..." value={search} onChange={e => setSearch(e.target.value)} /><Button type="primary" onClick={fetchData}>Refresh</Button></Space>}>
      <Table columns={columns} dataSource={filtered} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default AreaMasterPage;
