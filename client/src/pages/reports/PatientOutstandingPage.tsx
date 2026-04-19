import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import api from '../../services/api';

const PatientOutstandingPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Patient Name', dataIndex: 'patientName', key: 'patientName' },
    { title: 'Bill No', dataIndex: 'billNo', key: 'billNo' },
    { title: 'Bill Date', dataIndex: 'billDate', key: 'billDate' },
    { title: 'Bill Amount', dataIndex: 'billAmount', key: 'billAmount', align: 'right' as const },
    { title: 'Paid Amount', dataIndex: 'paidAmount', key: 'paidAmount', align: 'right' as const },
    { title: 'Outstanding', dataIndex: 'outstanding', key: 'outstanding', align: 'right' as const },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get('/reporting/patient-outstanding');
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Patient Outstanding">
      <Space style={{ marginBottom: 16 }} wrap>
        <Button type="primary" onClick={fetchData}>Refresh</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default PatientOutstandingPage;
