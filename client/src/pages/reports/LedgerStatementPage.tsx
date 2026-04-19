import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, DatePicker, Select, Space, Button, Statistic, Row, Col } from 'antd';
import { SearchOutlined } from '@ant-design/icons';
import api from '../../services/api';
import dayjs from 'dayjs';
import type { Ledger } from '../../types';

const { Title } = Typography;
const { RangePicker } = DatePicker;

interface StatementEntry {
  id: number;
  date: string;
  particular: string;
  debitAmount: number;
  creditAmount: number;
  balance: number;
}

const columns = [
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 110 },
  { title: 'Particular', dataIndex: 'particular', key: 'particular' },
  { title: 'Debit', dataIndex: 'debitAmount', key: 'debitAmount', render: (v: number) => v ? v.toFixed(2) : '-', align: 'right' as const },
  { title: 'Credit', dataIndex: 'creditAmount', key: 'creditAmount', render: (v: number) => v ? v.toFixed(2) : '-', align: 'right' as const },
  { title: 'Balance', dataIndex: 'balance', key: 'balance', render: (v: number) => v?.toFixed(2), align: 'right' as const },
];

const LedgerStatementPage: React.FC = () => {
  const [ledgers, setLedgers] = useState<Ledger[]>([]);
  const [data, setData] = useState<StatementEntry[]>([]);
  const [loading, setLoading] = useState(false);
  const [selectedLedger, setSelectedLedger] = useState<number | undefined>(undefined);
  const [dateRange, setDateRange] = useState<[string, string] | null>(null);
  const [closingBalance, setClosingBalance] = useState(0);

  useEffect(() => {
    api.get('/account/ledger', { params: { pageSize: 1000 } }).then((res) => {
      setLedgers(res.data.data || []);
    });
  }, []);

  const fetchData = async () => {
    if (!selectedLedger) return;
    setLoading(true);
    try {
      const params: Record<string, string | number> = { ledgerId: selectedLedger };
      if (dateRange) {
        params.fromDate = dateRange[0];
        params.toDate = dateRange[1];
      }
      const res = await api.get('/reports/ledger-statement', { params });
      const entries: StatementEntry[] = res.data.data || [];
      setData(entries);
      setClosingBalance(entries.length > 0 ? entries[entries.length - 1].balance : 0);
    } finally { setLoading(false); }
  };

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Ledger Statement</Title>
      </div>
      <Space style={{ marginBottom: 16 }} wrap>
        <Select
          showSearch
          placeholder="Select Ledger"
          style={{ width: 300 }}
          optionFilterProp="label"
          options={ledgers.map((l) => ({ value: l.id, label: `${l.code} - ${l.name}` }))}
          onChange={(v) => setSelectedLedger(v)}
        />
        <RangePicker
          onChange={(dates) => {
            if (dates && dates[0] && dates[1]) {
              setDateRange([dates[0].toISOString(), dates[1].toISOString()]);
            } else {
              setDateRange(null);
            }
          }}
        />
        <Button type="primary" icon={<SearchOutlined />} onClick={fetchData} disabled={!selectedLedger}>
          Generate
        </Button>
      </Space>
      {data.length > 0 && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={8}><Statistic title="Closing Balance" value={Math.abs(closingBalance)} precision={2} suffix={closingBalance >= 0 ? 'Dr' : 'Cr'} /></Col>
        </Row>
      )}
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        size="middle"
        pagination={false}
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
};

export default LedgerStatementPage;
