import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const BGDetailsReportPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'BG No', dataIndex: 'bgNo', key: 'bgNo' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Issue Date', dataIndex: 'issueDate', key: 'issueDate' },
    { title: 'Expiry Date', dataIndex: 'expiryDate', key: 'expiryDate' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/bg-details'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Bank Guarantee Details" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default BGDetailsReportPage;
