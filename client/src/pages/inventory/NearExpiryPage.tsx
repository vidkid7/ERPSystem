import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const NearExpiryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'Batch', dataIndex: 'batch', key: 'batch' },
    { title: 'Expiry Date', dataIndex: 'expiryDate', key: 'expiryDate' },
    { title: 'Qty', dataIndex: 'qty', key: 'qty', align: 'right' as const },
    { title: 'Godown', dataIndex: 'godown', key: 'godown' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/near-expiry', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Near Expiry Products"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default NearExpiryPage;
