import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const ReceiptNoteAnalysisPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'This Month', dataIndex: 'thisMonth', key: 'thisMonth', align: 'right' as const },
    { title: 'Last Month', dataIndex: 'lastMonth', key: 'lastMonth', align: 'right' as const },
    { title: 'Total', dataIndex: 'total', key: 'total', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/receipt-note-analysis', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Receipt Note Analysis"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default ReceiptNoteAnalysisPage;
