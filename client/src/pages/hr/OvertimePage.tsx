import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const OvertimePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Hours', dataIndex: 'hours', key: 'hours' },
    { title: 'Rate', dataIndex: 'rate', key: 'rate' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/overtime'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Overtime" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default OvertimePage;
