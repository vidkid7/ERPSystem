import React, { useEffect, useState } from 'react';
import { Tag, Card, Typography, Table, Statistic, Row, Col } from 'antd';
import api from '../../services/api';

const { Title } = Typography;

interface AssetStock {
  id: number;
  assetCode: string;
  name: string;
  assetModelName: string;
  assetCategoryName: string;
  location: string;
  status: string;
  purchaseCost: number;
  currentValue: number;
  assignedToEmployeeName: string;
}

const statusColors: Record<string, string> = {
  Available: 'green', Issued: 'blue', Damaged: 'red', Repair: 'orange', Disposed: 'default',
};

const columns = [
  { title: 'Asset Code', dataIndex: 'assetCode', key: 'assetCode', width: 130 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Category', dataIndex: 'assetCategoryName', key: 'assetCategoryName' },
  { title: 'Location', dataIndex: 'location', key: 'location' },
  { title: 'Purchase Cost', dataIndex: 'purchaseCost', key: 'purchaseCost', width: 130,
    render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }),
  },
  { title: 'Current Value', dataIndex: 'currentValue', key: 'currentValue', width: 130,
    render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }),
  },
  { title: 'Assigned To', dataIndex: 'assignedToEmployeeName', key: 'assignedToEmployeeName' },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110,
    render: (v: string) => <Tag color={statusColors[v] || 'default'}>{v}</Tag>,
  },
];

const AssetStockReportPage: React.FC = () => {
  const [data, setData] = useState<AssetStock[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    setLoading(true);
    api.get('/asset', { params: { pageSize: 1000 } })
      .then(r => setData(r.data.data || []))
      .finally(() => setLoading(false));
  }, []);

  const totalAssets = data.length;
  const totalValue = data.reduce((sum, a) => sum + (a.currentValue || 0), 0);
  const availableCount = data.filter(a => a.status === 'Available').length;
  const issuedCount = data.filter(a => a.status === 'Issued').length;

  return (
    <Card>
      <Title level={4}>Asset Stock Report</Title>
      <Row gutter={16} style={{ marginBottom: 24 }}>
        <Col span={6}><Statistic title="Total Assets" value={totalAssets} /></Col>
        <Col span={6}><Statistic title="Available" value={availableCount} valueStyle={{ color: '#3f8600' }} /></Col>
        <Col span={6}><Statistic title="Issued" value={issuedCount} valueStyle={{ color: '#1677ff' }} /></Col>
        <Col span={6}><Statistic title="Total Current Value" value={totalValue.toFixed(2)} /></Col>
      </Row>
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        size="middle"
        scroll={{ x: 'max-content' }}
        pagination={{ pageSize: 20, showTotal: (t) => `Total ${t} records` }}
      />
    </Card>
  );
};

export default AssetStockReportPage;
