import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Select, Table, Typography, Space } from 'antd';
import api from '../../services/api';

const { Title } = Typography;
const { RangePicker } = DatePicker;

interface SalesAnalysisRow {
  id: number;
  name: string;
  quantity: number;
  amount: number;
  percentage: number;
}

const SalesAnalysisPage: React.FC = () => {
  const [data, setData] = useState<SalesAnalysisRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [groupBy, setGroupBy] = useState<string>('Product');
  const [dateRange, setDateRange] = useState<[string, string] | null>(null);

  const fetchData = async (gb = groupBy, dr = dateRange) => {
    setLoading(true);
    try {
      const params: Record<string, any> = { groupBy: gb };
      if (dr) {
        params.fromDate = dr[0];
        params.toDate = dr[1];
      }
      const res = await api.get('/reports/sales-analysis', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  const handleGroupByChange = (value: string) => {
    setGroupBy(value);
    fetchData(value, dateRange);
  };

  const handleDateChange = (_: any, dateStrings: [string, string]) => {
    const dr = dateStrings[0] ? dateStrings : null;
    setDateRange(dr);
    fetchData(groupBy, dr);
  };

  const columns = [
    { title: groupBy, dataIndex: 'name', key: 'name' },
    { title: 'Quantity', dataIndex: 'quantity', key: 'quantity', width: 120,
      render: (v: number) => v?.toFixed(2), align: 'right' as const },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 150,
      render: (v: number) => v?.toFixed(2), align: 'right' as const },
    { title: '% Share', dataIndex: 'percentage', key: 'percentage', width: 100,
      render: (v: number) => `${v?.toFixed(1)}%`, align: 'right' as const },
  ];

  const totalAmount = data.reduce((sum, r) => sum + (r.amount || 0), 0);
  const totalQty = data.reduce((sum, r) => sum + (r.quantity || 0), 0);

  // Simple bar visualization
  const maxAmount = Math.max(...data.map((r) => r.amount || 0), 1);

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>Sales Analysis</Title>
        <Space>
          <Select value={groupBy} onChange={handleGroupByChange} style={{ width: 150 }}
            options={[
              { value: 'Product', label: 'By Product' },
              { value: 'Customer', label: 'By Customer' },
              { value: 'Agent', label: 'By Agent' },
            ]}
          />
          <RangePicker onChange={handleDateChange} />
        </Space>
      </div>

      {/* Bar chart visualization */}
      <div style={{ marginBottom: 24 }}>
        {data.slice(0, 10).map((row) => (
          <div key={row.id} style={{ display: 'flex', alignItems: 'center', marginBottom: 4 }}>
            <div style={{ width: 150, overflow: 'hidden', textOverflow: 'ellipsis', whiteSpace: 'nowrap', fontSize: 12 }}>
              {row.name}
            </div>
            <div style={{ flex: 1, marginLeft: 8 }}>
              <div
                style={{
                  width: `${(row.amount / maxAmount) * 100}%`,
                  height: 20,
                  backgroundColor: '#1890ff',
                  borderRadius: 2,
                  minWidth: 2,
                  transition: 'width 0.3s',
                }}
              />
            </div>
            <div style={{ width: 100, textAlign: 'right', fontSize: 12, marginLeft: 8 }}>
              {row.amount?.toFixed(2)}
            </div>
          </div>
        ))}
      </div>

      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="middle" pagination={{ pageSize: 20, showTotal: (t) => `Total ${t} records` }}
        scroll={{ x: 'max-content' }}
        summary={() => (
          <Table.Summary>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0}><strong>Total</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={1} align="right"><strong>{totalQty.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={2} align="right"><strong>{totalAmount.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={3} align="right"><strong>100.0%</strong></Table.Summary.Cell>
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />
    </Card>
  );
};

export default SalesAnalysisPage;
