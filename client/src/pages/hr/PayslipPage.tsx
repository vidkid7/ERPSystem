import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const PayslipPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Month', dataIndex: 'month', key: 'month' },
    { title: 'Basic', dataIndex: 'basic', key: 'basic' },
    { title: 'Allowance', dataIndex: 'allowance', key: 'allowance' },
    { title: 'Deductions', dataIndex: 'deductions', key: 'deductions' },
    { title: 'Net', dataIndex: 'net', key: 'net' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/payslip'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Payslip" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PayslipPage;
