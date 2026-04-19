import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { ShoppingCartOutlined, InboxOutlined, TeamOutlined, DollarOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface PurchaseStats {
  pendingPOs: number;
  receivedToday: number;
  suppliers: number;
  outstanding: number;
}

const PurchaseDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<PurchaseStats>({ pendingPOs: 0, receivedToday: 0, suppliers: 0, outstanding: 0 });

  useEffect(() => {
    api.get('/purchase/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Pending POs" value={stats.pendingPOs} prefix={<ShoppingCartOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Received Today" value={stats.receivedToday} prefix={<InboxOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Suppliers" value={stats.suppliers} prefix={<TeamOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Outstanding" value={stats.outstanding} prefix={<DollarOutlined />} precision={2} valueStyle={{ color: '#cf1322' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default PurchaseDashboardPage;
