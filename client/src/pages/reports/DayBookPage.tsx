import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, DatePicker, Statistic, Row, Col } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';
import type { DayBookEntry } from '../../types';

const { Title } = Typography;

interface FlatEntry {
  key: string;
  voucherNumber: string;
  voucherDate: string;
  ledgerName: string;
  debitAmount: number;
  creditAmount: number;
}

const columns = [
  { title: 'Date', dataIndex: 'voucherDate', key: 'voucherDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 110 },
  { title: 'Voucher No', dataIndex: 'voucherNumber', key: 'voucherNumber', width: 130 },
  { title: 'Ledger', dataIndex: 'ledgerName', key: 'ledgerName' },
  { title: 'Debit', dataIndex: 'debitAmount', key: 'debitAmount', render: (v: number) => v ? v.toFixed(2) : '-', align: 'right' as const },
  { title: 'Credit', dataIndex: 'creditAmount', key: 'creditAmount', render: (v: number) => v ? v.toFixed(2) : '-', align: 'right' as const },
];

const DayBookPage: React.FC = () => {
  const [data, setData] = useState<FlatEntry[]>([]);
  const [loading, setLoading] = useState(false);
  const [totals, setTotals] = useState({ debit: 0, credit: 0 });

  const fetchData = async (date?: string) => {
    setLoading(true);
    try {
      const res = await api.get('/reports/day-book', { params: { date } });
      const entries: DayBookEntry[] = res.data.data || [];
      const flat: FlatEntry[] = [];
      let totalDebit = 0;
      let totalCredit = 0;
      entries.forEach((entry, i) => {
        entry.details.forEach((d, j) => {
          flat.push({
            key: `${i}-${j}`,
            voucherNumber: entry.voucherNumber,
            voucherDate: entry.voucherDate,
            ledgerName: d.ledgerName,
            debitAmount: d.debitAmount,
            creditAmount: d.creditAmount,
          });
          totalDebit += d.debitAmount || 0;
          totalCredit += d.creditAmount || 0;
        });
      });
      setData(flat);
      setTotals({ debit: totalDebit, credit: totalCredit });
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(dayjs().toISOString()); }, []);

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Day Book</Title>
        <DatePicker defaultValue={dayjs()} onChange={(d) => fetchData(d?.toISOString())} />
      </div>
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col span={12}><Statistic title="Total Debit" value={totals.debit} precision={2} /></Col>
        <Col span={12}><Statistic title="Total Credit" value={totals.credit} precision={2} /></Col>
      </Row>
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="key"
        size="middle"
        pagination={false}
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
};

export default DayBookPage;
