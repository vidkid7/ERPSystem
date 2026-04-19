import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { ReloadOutlined } from '@ant-design/icons';
import api from '../../services/api';

const BedStatusPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Ward', dataIndex: 'ward', key: 'ward' },
    { title: 'Room', dataIndex: 'room', key: 'room' },
    { title: 'Bed No', dataIndex: 'bedNo', key: 'bedNo' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Available' ? 'green' : v === 'Occupied' ? 'red' : 'orange'}>{v}</Tag> },
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/bed-status'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Bed Status" extra={<Button icon={<ReloadOutlined />} onClick={fetchData}>Refresh</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default BedStatusPage;
