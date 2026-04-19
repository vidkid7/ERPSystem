import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const GodownWiseStockPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Godown', dataIndex: 'godown', key: 'godown' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Opening', dataIndex: 'opening', key: 'opening', align: 'right' as const },
    { title: 'In', dataIndex: 'inQty', key: 'inQty', align: 'right' as const },
    { title: 'Out', dataIndex: 'outQty', key: 'outQty', align: 'right' as const },
    { title: 'Closing', dataIndex: 'closing', key: 'closing', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/godown-wise-stock', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Godown Wise Stock"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default GodownWiseStockPage;
