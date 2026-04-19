import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const OutsourcedTestListPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Test', dataIndex: 'test', key: 'test' },
    { title: 'Lab', dataIndex: 'lab', key: 'lab' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Completed' ? 'green' : v === 'Pending' ? 'orange' : 'blue'}>{v}</Tag> },
    { title: 'Result Date', dataIndex: 'resultDate', key: 'resultDate' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/outsourced-test'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Outsourced Tests" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default OutsourcedTestListPage;
