import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const PurchaseReturnVatRegisterPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'DN No', dataIndex: 'dnNo', key: 'dnNo' },
    { title: 'Supplier', dataIndex: 'supplier', key: 'supplier' },
    { title: 'Taxable', dataIndex: 'taxable', key: 'taxable', align: 'right' as const },
    { title: 'VAT', dataIndex: 'vat', key: 'vat', align: 'right' as const },
    { title: 'Total', dataIndex: 'total', key: 'total', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/purchase-return-vat-register'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Purchase Return VAT Register" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PurchaseReturnVatRegisterPage;
