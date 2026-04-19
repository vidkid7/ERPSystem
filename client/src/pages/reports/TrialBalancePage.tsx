import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, DatePicker, Tag, Statistic, Row, Col } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';
import type { TrialBalance } from '../../types';

const { Title } = Typography;

const TrialBalancePage: React.FC = () => {
  const [data, setData] = useState<TrialBalance | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchData = async (date?: string) => {
    setLoading(true);
    try {
      const res = await api.get('/reports/trial-balance', { params: { asOfDate: date } });
      setData(res.data.data);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  const columns = [
    { title: 'Code', dataIndex: 'ledgerCode', key: 'ledgerCode', width: 100 },
    { title: 'Ledger', dataIndex: 'ledgerName', key: 'ledgerName' },
    { title: 'Debit', dataIndex: 'debitTotal', key: 'debitTotal', render: (v: number) => v?.toFixed(2), align: 'right' as const },
    { title: 'Credit', dataIndex: 'creditTotal', key: 'creditTotal', render: (v: number) => v?.toFixed(2), align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', render: (v: number) => Math.abs(v).toFixed(2), align: 'right' as const },
    { title: 'Type', dataIndex: 'balanceType', key: 'balanceType', render: (t: string) => <Tag color={t === 'Dr' ? 'blue' : 'red'}>{t}</Tag> },
  ];

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Trial Balance</Title>
        <DatePicker onChange={(d) => fetchData(d?.toISOString())} defaultValue={dayjs()} />
      </div>
      {data && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={8}><Statistic title="Total Debit" value={data.totalDebit} precision={2} /></Col>
          <Col span={8}><Statistic title="Total Credit" value={data.totalCredit} precision={2} /></Col>
          <Col span={8}><Statistic title="Balanced" value={data.isBalanced ? 'Yes' : 'No'} valueStyle={{ color: data.isBalanced ? '#3f8600' : '#cf1322' }} /></Col>
        </Row>
      )}
      <Table columns={columns} dataSource={data?.ledgers || []} loading={loading} rowKey="ledgerId" size="middle" pagination={false} />
    </Card>
  );
};

export default TrialBalancePage;
