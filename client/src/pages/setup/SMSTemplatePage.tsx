import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const SMSTemplatePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Template Name', dataIndex: 'templateName', key: 'templateName' },
    { title: 'Module', dataIndex: 'module', key: 'module' },
    { title: 'Text', dataIndex: 'text', key: 'text' },
    { title: 'Active', dataIndex: 'active', key: 'active', render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag> },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/sms-template'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="SMS Templates" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default SMSTemplatePage;
