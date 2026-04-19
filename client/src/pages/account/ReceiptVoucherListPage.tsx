import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const ReceiptVoucherListPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: "Date", dataIndex: "date", key: "date" },
    { title: "Receipt No", dataIndex: "receiptNo", key: "receiptNo" },
    { title: "Party", dataIndex: "party", key: "party" },
    { title: "Amount", dataIndex: "amount", key: "amount", align: "right" as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/receipt-voucher-list'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Receipt Voucher List" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default ReceiptVoucherListPage;
