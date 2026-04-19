import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const CashAndBankBookPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Particulars', dataIndex: 'particulars', key: 'particulars' },
    { title: 'Voucher', dataIndex: 'voucher', key: 'voucher' },
    { title: 'Receipt', dataIndex: 'receipt', key: 'receipt', align: 'right' as const },
    { title: 'Payment', dataIndex: 'payment', key: 'payment', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/cash-bank-book'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Cash & Bank Book" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default CashAndBankBookPage;
