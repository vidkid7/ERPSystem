import React, { useEffect, useState } from 'react';
import { Card, Table, Button } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const ShiftListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Shift Name', dataIndex: 'shiftName', key: 'shiftName' },
    { title: 'Start Time', dataIndex: 'startTime', key: 'startTime' },
    { title: 'End Time', dataIndex: 'endTime', key: 'endTime' },
    { title: 'Days', dataIndex: 'days', key: 'days' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/shift'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Shift List" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default ShiftListPage;
