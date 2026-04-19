import React, { useState, useEffect } from 'react';
import { Row, Col, Card, Statistic } from 'antd';
import { DollarOutlined, ArrowUpOutlined, ArrowDownOutlined, BankOutlined } from '@ant-design/icons';
import api from '../../services/api';

interface FinanceStats {
  revenue: number;
  expenses: number;
  profit: number;
  cashBalance: number;
}

const FinanceDashboardPage: React.FC = () => {
  const [stats, setStats] = useState<FinanceStats>({ revenue: 0, expenses: 0, profit: 0, cashBalance: 0 });

  useEffect(() => {
    api.get('/finance/dashboard').then(r => { if (r.data?.Data) setStats(r.data.Data); }).catch(() => {});
  }, []);

  return (
    <div>
      <Row gutter={16}>
        <Col span={6}>
          <Card><Statistic title="Revenue" value={stats.revenue} prefix={<ArrowUpOutlined />} precision={2} valueStyle={{ color: '#3f8600' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Expenses" value={stats.expenses} prefix={<ArrowDownOutlined />} precision={2} valueStyle={{ color: '#cf1322' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Profit" value={stats.profit} prefix={<DollarOutlined />} precision={2} valueStyle={{ color: stats.profit >= 0 ? '#3f8600' : '#cf1322' }} /></Card>
        </Col>
        <Col span={6}>
          <Card><Statistic title="Cash Balance" value={stats.cashBalance} prefix={<BankOutlined />} precision={2} valueStyle={{ color: '#1677ff' }} /></Card>
        </Col>
      </Row>
    </div>
  );
};

export default FinanceDashboardPage;
