import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Tag } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const EmailTemplatePage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Template Name', dataIndex: 'templateName', key: 'templateName' },
    { title: 'Subject', dataIndex: 'subject', key: 'subject' },
    { title: 'Module', dataIndex: 'module', key: 'module' },
    { title: 'Active', dataIndex: 'active', key: 'active', render: (v: boolean) => <Tag color={v ? 'green' : 'red'}>{v ? 'Active' : 'Inactive'}</Tag> },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/email-template'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Email Templates" extra={<Button type="primary" icon={<PlusOutlined />}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default EmailTemplatePage;
