import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const LeaveBalancePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Leave Type', dataIndex: 'leaveType', key: 'leaveType' },
    { title: 'Entitled', dataIndex: 'entitled', key: 'entitled' },
    { title: 'Used', dataIndex: 'used', key: 'used' },
    { title: 'Balance', dataIndex: 'balance', key: 'balance' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/leave-balance'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Leave Balance" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default LeaveBalancePage;
