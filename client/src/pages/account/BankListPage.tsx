import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const BankListPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: "Bank Name", dataIndex: "bankName", key: "bankName" },
    { title: "Account No", dataIndex: "accountNo", key: "accountNo" },
    { title: "Branch", dataIndex: "branch", key: "branch" },
    { title: "Balance", dataIndex: "balance", key: "balance", align: "right" as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/bank-list'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Bank List" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default BankListPage;
