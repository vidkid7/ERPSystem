import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const ServiceTenureReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Employee', dataIndex: 'employee', key: 'employee' },
    { title: 'Joined Date', dataIndex: 'joinedDate', key: 'joinedDate' },
    { title: 'Years', dataIndex: 'years', key: 'years' },
    { title: 'Months', dataIndex: 'months', key: 'months' },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hr/service-tenure'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Service Tenure" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="employee" size="small" />
    </Card>
  );
};
export default ServiceTenureReportPage;
