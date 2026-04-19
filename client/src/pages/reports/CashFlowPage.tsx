import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Table, Typography, Statistic, Row, Col } from 'antd';
import api from '../../services/api';

const { Title } = Typography;
const { RangePicker } = DatePicker;

interface CashFlowItem {
  description: string;
  amount: number;
}

interface CashFlowData {
  operating: CashFlowItem[];
  investing: CashFlowItem[];
  financing: CashFlowItem[];
  totalOperating: number;
  totalInvesting: number;
  totalFinancing: number;
  netCashFlow: number;
}

const columns = [
  { title: 'Description', dataIndex: 'description', key: 'description' },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 160,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
];

const CashFlowPage: React.FC = () => {
  const [data, setData] = useState<CashFlowData | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchData = async (fromDate?: string, toDate?: string) => {
    setLoading(true);
    try {
      const res = await api.get('/reports/cash-flow', { params: { fromDate, toDate } });
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
        <Title level={4} style={{ margin: 0 }}>Cash Flow Statement</Title>
        <RangePicker onChange={handleDateChange} />
      </div>

      {data && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={6}><Statistic title="Operating" value={data.totalOperating} precision={2} /></Col>
          <Col span={6}><Statistic title="Investing" value={data.totalInvesting} precision={2} /></Col>
          <Col span={6}><Statistic title="Financing" value={data.totalFinancing} precision={2} /></Col>
          <Col span={6}><Statistic title="Net Cash Flow" value={data.netCashFlow} precision={2}
            valueStyle={{ color: data.netCashFlow >= 0 ? '#3f8600' : '#cf1322' }} /></Col>
        </Row>
      )}

      <Title level={5}>Operating Activities</Title>
      <Table columns={columns} dataSource={data?.operating || []} loading={loading}
        rowKey="description" size="small" pagination={false}
        summary={() => (
          <Table.Summary>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}><strong>Total Operating</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={1} align="right"><strong>{data?.totalOperating?.toFixed(2)}</strong></Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />

      <Title level={5} style={{ marginTop: 16 }}>Investing Activities</Title>
      <Table columns={columns} dataSource={data?.investing || []} loading={loading}
        rowKey="description" size="small" pagination={false}
        summary={() => (
          <Table.Summary>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}><strong>Total Investing</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={1} align="right"><strong>{data?.totalInvesting?.toFixed(2)}</strong></Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />

      <Title level={5} style={{ marginTop: 16 }}>Financing Activities</Title>
      <Table columns={columns} dataSource={data?.financing || []} loading={loading}
        rowKey="description" size="small" pagination={false}
        summary={() => (
          <Table.Summary>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}><strong>Total Financing</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={1} align="right"><strong>{data?.totalFinancing?.toFixed(2)}</strong></Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />

      <Row style={{ marginTop: 16 }}>
        <Col span={24}>
          <Statistic title="Net Cash Flow" value={data?.netCashFlow || 0} precision={2}
            valueStyle={{ fontSize: 24, color: (data?.netCashFlow || 0) >= 0 ? '#3f8600' : '#cf1322' }} />
        </Col>
      </Row>
    </Card>
  );
};

export default CashFlowPage;
