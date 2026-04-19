import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Table, Typography, Statistic, Row, Col } from 'antd';
import dayjs from 'dayjs';
import api from '../../services/api';

const { Title } = Typography;
const { RangePicker } = DatePicker;

interface PLItem {
  ledgerGroupName: string;
  amount: number;
}

interface ProfitLossData {
  income: PLItem[];
  expenses: PLItem[];
  totalIncome: number;
  totalExpenses: number;
  netProfit: number;
}

const columns = [
  { title: 'Particulars', dataIndex: 'ledgerGroupName', key: 'ledgerGroupName' },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 160,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
];

const ProfitLossPage: React.FC = () => {
  const [data, setData] = useState<ProfitLossData | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchData = async (fromDate?: string, toDate?: string) => {
    setLoading(true);
    try {
      const res = await api.get('/reports/profit-loss', { params: { fromDate, toDate } });
      setData(res.data.data);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  const handleDateChange = (_: any, dateStrings: [string, string]) => {
    if (dateStrings[0]) {
      fetchData(dateStrings[0], dateStrings[1]);
    } else {
      fetchData();
    }
  };

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Profit & Loss Statement</Title>
        <RangePicker onChange={handleDateChange} />
      </div>

      {data && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={8}><Statistic title="Total Income" value={data.totalIncome} precision={2} valueStyle={{ color: '#3f8600' }} /></Col>
          <Col span={8}><Statistic title="Total Expenses" value={data.totalExpenses} precision={2} valueStyle={{ color: '#cf1322' }} /></Col>
          <Col span={8}><Statistic title="Net Profit" value={data.netProfit} precision={2}
            valueStyle={{ color: data.netProfit >= 0 ? '#3f8600' : '#cf1322' }} /></Col>
        </Row>
      )}

      <Row gutter={24}>
        <Col span={12}>
          <Title level={5}>Income</Title>
          <Table columns={columns} dataSource={data?.income || []} loading={loading}
            rowKey="ledgerGroupName" size="small" pagination={false}
            summary={() => (
              <Table.Summary>
                <Table.Summary.Row>
                  <Table.Summary.Cell index={0}><strong>Total Income</strong></Table.Summary.Cell>
                  <Table.Summary.Cell index={1} align="right"><strong>{data?.totalIncome?.toFixed(2)}</strong></Table.Summary.Cell>
                </Table.Summary.Row>
              </Table.Summary>
            )}
          />
        </Col>
        <Col span={12}>
          <Title level={5}>Expenses</Title>
          <Table columns={columns} dataSource={data?.expenses || []} loading={loading}
            rowKey="ledgerGroupName" size="small" pagination={false}
            summary={() => (
              <Table.Summary>
                <Table.Summary.Row>
                  <Table.Summary.Cell index={0}><strong>Total Expenses</strong></Table.Summary.Cell>
                  <Table.Summary.Cell index={1} align="right"><strong>{data?.totalExpenses?.toFixed(2)}</strong></Table.Summary.Cell>
                </Table.Summary.Row>
              </Table.Summary>
            )}
          />
        </Col>
      </Row>
    </Card>
  );
};

export default ProfitLossPage;
