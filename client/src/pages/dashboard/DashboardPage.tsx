import React from 'react';
import { Row, Col, Card, Statistic, Typography } from 'antd';
import {
  DollarOutlined, ShoppingCartOutlined, TeamOutlined,
  ShopOutlined, RiseOutlined, FallOutlined,
} from '@ant-design/icons';

const { Title } = Typography;

const DashboardPage: React.FC = () => {
  return (
    <div>
      <Title level={3}>Dashboard</Title>
      <Row gutter={[16, 16]}>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Total Sales" value={125000} prefix={<DollarOutlined />} precision={2} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Total Purchases" value={89000} prefix={<ShoppingCartOutlined />} precision={2} valueStyle={{ color: '#cf1322' }} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Employees" value={42} prefix={<TeamOutlined />} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Products" value={156} prefix={<ShopOutlined />} /></Card>
        </Col>
      </Row>
      <Row gutter={[16, 16]} style={{ marginTop: 16 }}>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Revenue (This Month)" value={45000} prefix={<RiseOutlined />} precision={2} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Expenses (This Month)" value={32000} prefix={<FallOutlined />} precision={2} valueStyle={{ color: '#cf1322' }} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Pending Orders" value={8} /></Card>
        </Col>
        <Col xs={24} sm={12} lg={6}>
          <Card><Statistic title="Active Patients" value={23} prefix={<TeamOutlined />} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default DashboardPage;
