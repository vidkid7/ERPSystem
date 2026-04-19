import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const SalarySheetPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Department', dataIndex: 'department', key: 'department' },
    { title: 'Basic', dataIndex: 'basic', key: 'basic' },
    { title: 'HRA', dataIndex: 'hra', key: 'hra' },
    { title: 'Total', dataIndex: 'total', key: 'total' },
    { title: 'Net', dataIndex: 'net', key: 'net' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/salary-sheet'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Salary Sheet" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="employee" size="small" />
    </Card>
  );
};
export default SalarySheetPage;
