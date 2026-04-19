import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space, Statistic, Row, Col } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface LabIncomeRow {
  id: number;
  date: string;
  testsCount: number;
  revenue: number;
  expenses: number;
  net: number;
}

const columns: ColumnsType<LabIncomeRow> = [
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Tests Count', dataIndex: 'testsCount', key: 'testsCount', align: 'right', width: 110 },
  { title: 'Revenue', dataIndex: 'revenue', key: 'revenue', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Expenses', dataIndex: 'expenses', key: 'expenses', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
  { title: 'Net', dataIndex: 'net', key: 'net', align: 'right', width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const LabIncomeReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<LabIncomeRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const totalRevenue = data.reduce((s, r) => s + (r.revenue || 0), 0);
  const totalNet = data.reduce((s, r) => s + (r.net || 0), 0);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/lab/income-report', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Lab Income Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      {data.length > 0 && (
        <Row gutter={16} style={{ marginBottom: 16 }}>
          <Col span={8}><Statistic title="Total Revenue" value={totalRevenue.toFixed(2)} /></Col>
          <Col span={8}><Statistic title="Net Income" value={totalNet.toFixed(2)} valueStyle={{ color: totalNet >= 0 ? '#3f8600' : '#cf1322' }} /></Col>
        </Row>
      )}
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 650 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default LabIncomeReportPage;
