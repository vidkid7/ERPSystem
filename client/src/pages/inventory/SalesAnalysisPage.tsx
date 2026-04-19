import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const SalesAnalysisPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Jan', dataIndex: 'jan', key: 'jan', align: 'right' as const },
    { title: 'Feb', dataIndex: 'feb', key: 'feb', align: 'right' as const },
    { title: 'Mar', dataIndex: 'mar', key: 'mar', align: 'right' as const },
    { title: 'Apr', dataIndex: 'apr', key: 'apr', align: 'right' as const },
    { title: 'May', dataIndex: 'may', key: 'may', align: 'right' as const },
    { title: 'Jun', dataIndex: 'jun', key: 'jun', align: 'right' as const },
    { title: 'Total', dataIndex: 'total', key: 'total', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/sales-analysis', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Sales Analysis"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default SalesAnalysisPage;
