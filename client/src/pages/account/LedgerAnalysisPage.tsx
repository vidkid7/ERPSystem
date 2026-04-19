import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const LedgerAnalysisPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Ledger', dataIndex: 'ledger', key: 'ledger' },
    { title: 'Opening', dataIndex: 'opening', key: 'opening', align: 'right' as const },
    { title: 'Debit', dataIndex: 'debit', key: 'debit', align: 'right' as const },
    { title: 'Credit', dataIndex: 'credit', key: 'credit', align: 'right' as const },
    { title: 'Closing', dataIndex: 'closing', key: 'closing', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/ledger-analysis'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Ledger Analysis" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LedgerAnalysisPage;
