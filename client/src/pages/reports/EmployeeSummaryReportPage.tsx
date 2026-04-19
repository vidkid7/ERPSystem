import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const EmployeeSummaryReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Emp Code', dataIndex: 'empCode', key: 'empCode' },
    { title: 'Name', dataIndex: 'name', key: 'name' },
    { title: 'Department', dataIndex: 'department', key: 'department' },
    { title: 'Designation', dataIndex: 'designation', key: 'designation' },
    { title: 'Join Date', dataIndex: 'joinDate', key: 'joinDate' },
    { title: 'Salary', dataIndex: 'salary', key: 'salary', align: 'right' as const },
    { title: 'Status', dataIndex: 'status', key: 'status' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/employee-summary');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Employee Summary Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default EmployeeSummaryReportPage;
