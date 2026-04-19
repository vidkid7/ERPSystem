import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const ServiceReminderPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Customer', dataIndex: 'customer', key: 'customer' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Service Type', dataIndex: 'serviceType', key: 'serviceType' },
    { title: 'Last Service Date', dataIndex: 'lastServiceDate', key: 'lastServiceDate' },
    { title: 'Next Service Date', dataIndex: 'nextServiceDate', key: 'nextServiceDate' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/service-reminder');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Service Reminders">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default ServiceReminderPage;
