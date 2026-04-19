import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { TeamOutlined, CheckCircleOutlined, ClockCircleOutlined, UserAddOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface HRStats {
  totalEmployees: number;
  present: number;
  onLeave: number;
  newJoinings: number;
}

const HRDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<HRStats>({ totalEmployees: 0, present: 0, onLeave: 0, newJoinings: 0 });

  useEffect(() => {
    api.get('/hr/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Total Employees" value={stats.totalEmployees} prefix={<TeamOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Present" value={stats.present} prefix={<CheckCircleOutlined />} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="On Leave" value={stats.onLeave} prefix={<ClockCircleOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="New Joinings" value={stats.newJoinings} prefix={<UserAddOutlined />} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default HRDashboardPage;
