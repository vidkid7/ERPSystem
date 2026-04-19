import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface ProductionPlanRow {
  id: number;
  product: string;
  plannedQty: number;
  produced: number;
  variance: number;
  date: string;
}

const columns: ColumnsType<ProductionPlanRow> = [
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Planned Qty', dataIndex: 'plannedQty', key: 'plannedQty', align: 'right', width: 120 },
  { title: 'Produced', dataIndex: 'produced', key: 'produced', align: 'right', width: 110 },
  { title: 'Variance', dataIndex: 'variance', key: 'variance', align: 'right', width: 110,
    render: (v: number) => <span style={{ color: v >= 0 ? '#3f8600' : '#cf1322' }}>{v}</span> },
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
];

const ProductionPlanPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<ProductionPlanRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/inventory/production-plan', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Production Plan">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 650 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default ProductionPlanPage;
