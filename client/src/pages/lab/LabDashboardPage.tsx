import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { ExperimentOutlined, CheckCircleOutlined, ClockCircleOutlined, FileTextOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface LabStats {
  pendingTests: number;
  completedToday: number;
  revenue: number;
  pendingReports: number;
}

const LabDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<LabStats>({ pendingTests: 0, completedToday: 0, revenue: 0, pendingReports: 0 });

  useEffect(() => {
    api.get('/lab/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Pending Tests" value={stats.pendingTests} prefix={<ClockCircleOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Completed Today" value={stats.completedToday} prefix={<CheckCircleOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Revenue" value={stats.revenue} prefix={<ExperimentOutlined />} precision={2} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Pending Reports" value={stats.pendingReports} prefix={<FileTextOutlined />} valueStyle={{ color: '#cf1322' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default LabDashboardPage;
