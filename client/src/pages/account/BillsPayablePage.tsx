import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const BillsPayablePage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Bill No', dataIndex: 'billNo', key: 'billNo' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/bills-payable'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Bills Payable" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default BillsPayablePage;
