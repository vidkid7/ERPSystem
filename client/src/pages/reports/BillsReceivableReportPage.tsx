import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const BillsReceivableReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Bill No', dataIndex: 'billNo', key: 'billNo' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate' },
    { title: 'Days Overdue', dataIndex: 'daysOverdue', key: 'daysOverdue', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/bills-receivable');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Bills Receivable">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default BillsReceivableReportPage;
