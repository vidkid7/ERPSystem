import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const TDSSummaryPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: 'PAN', dataIndex: 'pan', key: 'pan' },
    { title: 'TDS Amount', dataIndex: 'tdsAmount', key: 'tdsAmount', align: 'right' as const },
    { title: 'Voucher No', dataIndex: 'voucherNo', key: 'voucherNo' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/tds-summary'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="TDS Summary" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default TDSSummaryPage;
