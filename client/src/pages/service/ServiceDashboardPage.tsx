import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { ToolOutlined, CheckCircleOutlined, ClockCircleOutlined, DollarOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface ServiceStats {
  total: number;
  pending: number;
  completed: number;
  revenue: number;
}

const ServiceDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<ServiceStats>({ total: 0, pending: 0, completed: 0, revenue: 0 });

  useEffect(() => {
    api.get('/service/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Total Jobcards" value={stats.total} prefix={<ToolOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Pending" value={stats.pending} prefix={<ClockCircleOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Completed" value={stats.completed} prefix={<CheckCircleOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Revenue" value={stats.revenue} prefix={<DollarOutlined />} precision={2} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default ServiceDashboardPage;
