import React, { useState } from 'react';
import { Card, Table, Select, Button, Space, Row, Col } from 'antd';
import api from '../../services/api';

const MONTHS = ['January','February','March','April','May','June','July','August','September','October','November','December'];

const PayrollPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [month, setMonth] = useState<number>(new Date().getMonth() + 1);
  const [year, setYear] = useState<number>(new Date().getFullYear());

  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Basic', dataIndex: 'basic', key: 'basic' },
    { title: 'Allowance', dataIndex: 'allowance', key: 'allowance' },
    { title: 'Deduction', dataIndex: 'deduction', key: 'deduction' },
    { title: 'Net', dataIndex: 'net', key: 'net' },
  ];

  const handleProcess = async () => {
    setLoading(true);
    try { const r = await api.get(`/hr/payroll?month=${month}&year=${year}`); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  return (
    <Card title="Payroll Processing">
      <Row gutter={16} style={{ marginBottom: 16 }}>
        <Col>
          <Select value={month} onChange={setMonth} style={{ width: 140 }}>
            {MONTHS.map((m, i) => <Select.Option key={i + 1} value={i + 1}>{m}</Select.Option>)}
          </Select>
        </Col>
        <Col>
          <Select value={year} onChange={setYear} style={{ width: 100 }}>
            {[2023, 2024, 2025, 2026].map(y => <Select.Option key={y} value={y}>{y}</Select.Option>)}
          </Select>
        </Col>
        <Col>
          <Space>
            <Button type="primary" onClick={handleProcess}>Process</Button>
          </Space>
        </Col>
      </Row>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="employee" size="small" />
    </Card>
  );
};
export default PayrollPage;
