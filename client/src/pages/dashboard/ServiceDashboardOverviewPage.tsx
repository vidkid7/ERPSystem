import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { ToolOutlined, ClockCircleOutlined, CheckCircleOutlined, SmileOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface ServiceOverviewStats {
  jobcards: number;
  pending: number;
  resolved: number;
  customerSatisfaction: number;
}

const ServiceDashboardOverviewPage: React.FC = () => {
  const [stats, setStats] = useState<ServiceOverviewStats>({ jobcards: 0, pending: 0, resolved: 0, customerSatisfaction: 0 });

  useEffect(() => {
    api.get('/service/dashboard-overview').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Total Jobcards" value={stats.jobcards} prefix={<ToolOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Pending" value={stats.pending} prefix={<ClockCircleOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Resolved" value={stats.resolved} prefix={<CheckCircleOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Customer Satisfaction" value={stats.customerSatisfaction} prefix={<SmileOutlined />} suffix="%" precision={1} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default ServiceDashboardOverviewPage;
