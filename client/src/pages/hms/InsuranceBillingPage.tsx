import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const InsuranceBillingPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Patient', dataIndex: 'patient', key: 'patient' },
    { title: 'Insurance Co', dataIndex: 'insuranceCo', key: 'insuranceCo' },
    { title: 'Claim No', dataIndex: 'claimNo', key: 'claimNo' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount' },
    { title: 'Status', dataIndex: 'status', key: 'status', render: (v: string) => <Tag color={v === 'Approved' ? 'green' : v === 'Rejected' ? 'red' : 'orange'}>{v}</Tag> },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/insurance-billing'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Insurance Billing" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default InsuranceBillingPage;
