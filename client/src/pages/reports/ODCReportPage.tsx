import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const ODCReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Cheque No', dataIndex: 'chequeNo', key: 'chequeNo' },
    { title: 'Bank', dataIndex: 'bank', key: 'bank' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/odc');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Open Dated Cheque Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default ODCReportPage;
