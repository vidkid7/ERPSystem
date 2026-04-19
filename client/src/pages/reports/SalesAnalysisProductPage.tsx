import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const SalesAnalysisProductPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);

  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Total Qty', dataIndex: 'totalQty', key: 'totalQty', align: 'right' as const },
    { title: 'Total Amount', dataIndex: 'totalAmount', key: 'totalAmount', align: 'right' as const },
    { title: 'Customer Count', dataIndex: 'customerCount', key: 'customerCount', align: 'right' as const },
    { title: 'Avg Rate', dataIndex: 'avgRate', key: 'avgRate', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = {};
      if (dateRange) {
        params.fromDate = dateRange[0].format('YYYY-MM-DD');
        params.toDate = dateRange[1].format('YYYY-MM-DD');
      }
      const res = await api.get('/reporting/sales-analysis-product', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Sales Analysis by Product">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(dates) => setDateRange(dates as any)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default SalesAnalysisProductPage;
