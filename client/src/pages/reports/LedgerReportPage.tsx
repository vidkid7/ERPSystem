import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const LedgerReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Ledger Name', dataIndex: 'ledgerName', key: 'ledgerName' },
    { title: 'Group', dataIndex: 'group', key: 'group' },
    { title: 'Opening Balance', dataIndex: 'openingBalance', key: 'openingBalance', align: 'right' as const },
    { title: 'Debit', dataIndex: 'debit', key: 'debit', align: 'right' as const },
    { title: 'Credit', dataIndex: 'credit', key: 'credit', align: 'right' as const },
    { title: 'Closing Balance', dataIndex: 'closingBalance', key: 'closingBalance', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/ledgers');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Ledger Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default LedgerReportPage;
