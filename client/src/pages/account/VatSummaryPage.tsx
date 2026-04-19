import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const VatSummaryPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Month', dataIndex: 'month', key: 'month' },
    { title: 'Sales VAT', dataIndex: 'salesVat', key: 'salesVat', align: 'right' as const },
    { title: 'Purchase VAT', dataIndex: 'purchaseVat', key: 'purchaseVat', align: 'right' as const },
    { title: 'Net VAT', dataIndex: 'netVat', key: 'netVat', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/vat-summary'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="VAT Summary" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default VatSummaryPage;
