import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Select, Button, Table, Space } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const LedgerAnalysisPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);

  const columns = [
    { title: 'Ledger', dataIndex: 'ledgerName', key: 'ledgerName' },
    { title: 'Opening', dataIndex: 'openingBalance', key: 'openingBalance', align: 'right' as const },
    { title: 'Debit', dataIndex: 'totalDebit', key: 'totalDebit', align: 'right' as const },
    { title: 'Credit', dataIndex: 'totalCredit', key: 'totalCredit', align: 'right' as const },
    { title: 'Closing', dataIndex: 'closingBalance', key: 'closingBalance', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = {};
      if (dateRange) {
        params.fromDate = dateRange[0].format('YYYY-MM-DD');
        params.toDate = dateRange[1].format('YYYY-MM-DD');
      }
      const res = await api.get('/reporting/ledger-analysis', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Ledger Analysis">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(dates) => setDateRange(dates as any)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading} rowKey={(_, i) => String(i)}
        pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default LedgerAnalysisPage;
