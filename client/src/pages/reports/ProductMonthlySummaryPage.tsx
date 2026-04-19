import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const ProductMonthlySummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);

  const columns = [
    { title: 'Month', dataIndex: 'month', key: 'month' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Opening Qty', dataIndex: 'openingQty', key: 'openingQty', align: 'right' as const },
    { title: 'Inward Qty', dataIndex: 'inwardQty', key: 'inwardQty', align: 'right' as const },
    { title: 'Outward Qty', dataIndex: 'outwardQty', key: 'outwardQty', align: 'right' as const },
    { title: 'Closing Qty', dataIndex: 'closingQty', key: 'closingQty', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = {};
      if (dateRange) {
        params.fromDate = dateRange[0].format('YYYY-MM-DD');
        params.toDate = dateRange[1].format('YYYY-MM-DD');
      }
      const res = await api.get('/reporting/product-monthly-summary', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Product Monthly Summary">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker picker="month" onChange={(dates) => setDateRange(dates as any)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default ProductMonthlySummaryPage;
