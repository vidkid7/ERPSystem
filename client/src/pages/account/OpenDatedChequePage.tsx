import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const OpenDatedChequePage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Cheque No', dataIndex: 'chequeNo', key: 'chequeNo' },
    { title: 'Bank', dataIndex: 'bank', key: 'bank' },
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/open-dated-cheque'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Open Dated Cheque" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default OpenDatedChequePage;
