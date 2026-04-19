import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const CollectionReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Borrower', dataIndex: 'borrower', key: 'borrower' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Installment', dataIndex: 'installment', key: 'installment' },
    { title: 'Principal', dataIndex: 'principal', key: 'principal' },
    { title: 'Interest', dataIndex: 'interest', key: 'interest' },
    { title: 'Collected', dataIndex: 'collected', key: 'collected' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/collection-report'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Collection Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default CollectionReportPage;
