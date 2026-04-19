import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const LCDetailsReportPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: "LC No", dataIndex: "lcNo", key: "lcNo" },
    { title: "Party", dataIndex: "party", key: "party" },
    { title: "Amount", dataIndex: "amount", key: "amount", align: "right" as const },
    { title: "Expiry", dataIndex: "expiry", key: "expiry" },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/lc-details-report'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="LC Details Report" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LCDetailsReportPage;
