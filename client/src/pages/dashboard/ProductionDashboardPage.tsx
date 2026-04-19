import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { BuildOutlined, SyncOutlined, CheckCircleOutlined, ThunderboltOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface ProductionStats {
  productionOrders: number;
  inProgress: number;
  completed: number;
  efficiency: number;
}

const ProductionDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<ProductionStats>({ productionOrders: 0, inProgress: 0, completed: 0, efficiency: 0 });

  useEffect(() => {
    api.get('/production/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Production Orders" value={stats.productionOrders} prefix={<BuildOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="In Progress" value={stats.inProgress} prefix={<SyncOutlined spin />} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Completed" value={stats.completed} prefix={<CheckCircleOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Efficiency (%)" value={stats.efficiency} prefix={<ThunderboltOutlined />} precision={1} valueStyle={{ color: '#722ed1' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default ProductionDashboardPage;
