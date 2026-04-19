import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const CustomerReportPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: "Party", dataIndex: "party", key: "party" },
    { title: "Phone", dataIndex: "phone", key: "phone" },
    { title: "Balance", dataIndex: "balance", key: "balance", align: "right" as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/customer-report'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Customer Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default CustomerReportPage;
