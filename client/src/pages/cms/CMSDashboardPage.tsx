import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { ShoppingOutlined, PictureOutlined, CalendarOutlined, NotificationOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface CMSStats {
  productsDisplayed: number;
  activeBanners: number;
  events: number;
  notices: number;
}

const CMSDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<CMSStats>({ productsDisplayed: 0, activeBanners: 0, events: 0, notices: 0 });

  useEffect(() => {
    api.get('/cms/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Products Displayed" value={stats.productsDisplayed} prefix={<ShoppingOutlined />} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Active Banners" value={stats.activeBanners} prefix={<PictureOutlined />} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Events" value={stats.events} prefix={<CalendarOutlined />} valueStyle={{ color: '#722ed1' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Notices" value={stats.notices} prefix={<NotificationOutlined />} valueStyle={{ color: '#faad14' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default CMSDashboardPage;
