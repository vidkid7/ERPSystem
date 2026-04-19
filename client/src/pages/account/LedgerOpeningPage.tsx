import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const LedgerOpeningPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Ledger', dataIndex: 'ledger', key: 'ledger' },
    { title: 'Opening Dr', dataIndex: 'openingDr', key: 'openingDr', align: 'right' as const },
    { title: 'Opening Cr', dataIndex: 'openingCr', key: 'openingCr', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/ledger-opening'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Ledger Opening" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LedgerOpeningPage;
