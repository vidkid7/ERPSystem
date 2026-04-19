import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const EmployeeSummaryPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Department', dataIndex: 'department', key: 'department' },
    { title: 'Active', dataIndex: 'active', key: 'active' },
    { title: 'On Leave', dataIndex: 'onLeave', key: 'onLeave' },
    { title: 'Total', dataIndex: 'total', key: 'total' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/employee-summary'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Employee Summary" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="department" size="small" />
    </Card>
  );
};
export default EmployeeSummaryPage;
