import React, { useEffect, useState } from 'react';
import { Card, Row, Col, Statistic, Divider } from 'antd';
import api from '../../services/api';

interface ServiceStats {
  openTickets: number;
  closedToday: number;
  pendingJobcards: number;
}

const ServiceDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<ServiceStats>({ openTickets: 0, closedToday: 0, pendingJobcards: 0 });
  const [loading, setLoading] = useState(false);

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/service-dashboard');
      if (res.data.data) setStats(res.data.data);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Service Dashboard" loading={loading}>
      <Row gutter={[24, 24]}>
        <Col xs={24} sm={8}>
          <Statistic title="Open Tickets" value={stats.openTickets} valueStyle={{ color: '#cf1322' }} />
        </Col>
        <Col xs={24} sm={8}>
          <Statistic title="Closed Today" value={stats.closedToday} valueStyle={{ color: '#3f8600' }} />
        </Col>
        <Col xs={24} sm={8}>
          <Statistic title="Pending Jobcards" value={stats.pendingJobcards} valueStyle={{ color: '#d46b08' }} />
        </Col>
      </Row>
    </Card>
  );
};

export default ServiceDashboardPage;
