import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;
interface RowType { id: number; [key: string]: any; }
const CostCenterAnalysisPage: React.FC = () => {
  const [data, setData] = useState<RowType[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Cost Center', dataIndex: 'costCenter', key: 'costCenter' },
    { title: 'Budget', dataIndex: 'budget', key: 'budget', align: 'right' as const },
    { title: 'Actual', dataIndex: 'actual', key: 'actual', align: 'right' as const },
    { title: 'Variance', dataIndex: 'variance', key: 'variance', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const res = await api.get('/account/cost-center-analysis'); setData(res.data?.Data || []); }
    catch { setData([]); } finally { setLoading(false); }
  };
  return (
    <Card title="Cost Center Analysis" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default CostCenterAnalysisPage;
