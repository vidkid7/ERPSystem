import React, { useEffect, useState } from 'react';
import { Card, Table, Button } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const HolidayListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Holiday Name', dataIndex: 'holidayName', key: 'holidayName' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Type', dataIndex: 'type', key: 'type' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/holiday'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Holiday List" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default HolidayListPage;
