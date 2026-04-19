import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const DepartmentWiseOPDPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Department', dataIndex: 'department', key: 'department' },
    { title: 'Patients', dataIndex: 'patients', key: 'patients' },
    { title: 'Revenue', dataIndex: 'revenue', key: 'revenue' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/department-wise-opd'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Department Wise OPD" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="department" size="small" />
    </Card>
  );
};
export default DepartmentWiseOPDPage;
