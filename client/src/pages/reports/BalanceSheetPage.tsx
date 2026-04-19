import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Table, Typography, Statistic, Row, Col, Divider } from 'antd';
import dayjs from 'dayjs';
import api from '../../services/api';

const { Title } = Typography;

interface BalanceSheetItem {
  ledgerGroupName: string;
  amount: number;
}

interface BalanceSheetData {
  assets: BalanceSheetItem[];
  liabilities: BalanceSheetItem[];
  totalAssets: number;
  totalLiabilities: number;
}

const columns = [
  { title: 'Particulars', dataIndex: 'ledgerGroupName', key: 'ledgerGroupName' },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 160,
    render: (v: number) => v?.toFixed(2), align: 'right' as const },
];

const BalanceSheetPage: React.FC = () => {
  const [data, setData] = useState<BalanceSheetData | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchData = async (date?: string) => {
    setLoading(true);
    try {
      const res = await api.get('/reports/balance-sheet', { params: { asOfDate: date } });
      setData(res.data.data);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Balance Sheet</Title>
        <DatePicker onChange={(d) => fetchData(d?.format('YYYY-MM-DD'))} defaultValue={dayjs()} />
      </div>

      {data && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={12}><Statistic title="Total Assets" value={data.totalAssets} precision={2} /></Col>
          <Col span={12}><Statistic title="Total Liabilities" value={data.totalLiabilities} precision={2} /></Col>
        </Row>
      )}

      <Row gutter={24}>
        <Col span={12}>
          <Title level={5}>Assets</Title>
          <Table columns={columns} dataSource={data?.assets || []} loading={loading}
            rowKey="ledgerGroupName" size="small" pagination={false}
            summary={() => (
              <Table.Summary>
                <Table.Summary.Row>
                  <Table.Summary.Cell index={0}><strong>Total Assets</strong></Table.Summary.Cell>
                  <Table.Summary.Cell index={1} align="right"><strong>{data?.totalAssets?.toFixed(2)}</strong></Table.Summary.Cell>
                </Table.Summary.Row>
              </Table.Summary>
            )}
          />
        </Col>
        <Col span={12}>
          <Title level={5}>Liabilities</Title>
          <Table columns={columns} dataSource={data?.liabilities || []} loading={loading}
            rowKey="ledgerGroupName" size="small" pagination={false}
            summary={() => (
              <Table.Summary>
                <Table.Summary.Row>
                  <Table.Summary.Cell index={0}><strong>Total Liabilities</strong></Table.Summary.Cell>
                  <Table.Summary.Cell index={1} align="right"><strong>{data?.totalLiabilities?.toFixed(2)}</strong></Table.Summary.Cell>
                </Table.Summary.Row>
              </Table.Summary>
            )}
          />
        </Col>
      </Row>
    </Card>
  );
};

export default BalanceSheetPage;
