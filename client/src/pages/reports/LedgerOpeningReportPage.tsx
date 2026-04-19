import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const LedgerOpeningReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Ledger Name', dataIndex: 'ledgerName', key: 'ledgerName' },
    { title: 'Group', dataIndex: 'group', key: 'group' },
    { title: 'Opening Debit', dataIndex: 'openingDebit', key: 'openingDebit', align: 'right' as const },
    { title: 'Opening Credit', dataIndex: 'openingCredit', key: 'openingCredit', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/ledger-opening');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Ledger Opening Balances">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 700 }} />
    </Card>
  );
};

export default LedgerOpeningReportPage;
