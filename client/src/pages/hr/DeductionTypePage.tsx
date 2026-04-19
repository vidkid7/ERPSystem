import React, { useEffect, useState } from 'react';
import { Card, Table, Button } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const DeductionTypePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Deduction Name', dataIndex: 'deductionName', key: 'deductionName' },
    { title: 'Type', dataIndex: 'type', key: 'type' },
    { title: 'Calculation', dataIndex: 'calculation', key: 'calculation' },
    { title: 'Mandatory', dataIndex: 'mandatory', key: 'mandatory', render: (v: boolean) => v ? 'Yes' : 'No' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/deduction-type'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Deduction Types" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default DeductionTypePage;
