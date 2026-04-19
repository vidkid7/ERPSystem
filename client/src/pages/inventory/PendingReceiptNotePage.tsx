import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const PendingReceiptNotePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'RN No', dataIndex: 'rnNo', key: 'rnNo' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Items', dataIndex: 'items', key: 'items' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/pending-receipt-notes', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Pending Receipt Note"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PendingReceiptNotePage;
