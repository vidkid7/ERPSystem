import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const InterestCalculationPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Loan No', dataIndex: 'loanNo', key: 'loanNo' },
    { title: 'Principal', dataIndex: 'principal', key: 'principal' },
    { title: 'Rate', dataIndex: 'rate', key: 'rate' },
    { title: 'Period', dataIndex: 'period', key: 'period' },
    { title: 'Interest', dataIndex: 'interest', key: 'interest' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/finance/interest-calculation'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Interest Calculation" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="loanNo" size="small" />
    </Card>
  );
};
export default InterestCalculationPage;
