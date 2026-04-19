import React, { useEffect, useState } from 'react';
import { Card, Table, Button } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const DoctorSchedulePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Doctor', dataIndex: 'doctor', key: 'doctor' },
    { title: 'Weekday', dataIndex: 'weekday', key: 'weekday' },
    { title: 'Start Time', dataIndex: 'startTime', key: 'startTime' },
    { title: 'End Time', dataIndex: 'endTime', key: 'endTime' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/doctor-schedule'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Doctor Schedule" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default DoctorSchedulePage;
