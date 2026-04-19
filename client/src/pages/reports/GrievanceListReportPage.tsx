import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const GrievanceListReportPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Emp Name', dataIndex: 'empName', key: 'empName' },
    { title: 'Grievance Type', dataIndex: 'grievanceType', key: 'grievanceType' },
    { title: 'Submitted Date', dataIndex: 'submittedDate', key: 'submittedDate' },
    { title: 'Status', dataIndex: 'status', key: 'status' },
    { title: 'Resolved Date', dataIndex: 'resolvedDate', key: 'resolvedDate' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/grievance-list');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Grievance List Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 800 }} />
    </Card>
  );
};

export default GrievanceListReportPage;
