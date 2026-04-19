import React, { useEffect, useState } from 'react';
import { Card, Table, Button, Space } from 'antd';
import { PlusOutlined } from '@ant-design/icons';
import api from '../../services/api';

const PrinterSetupPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  const columns = [
    { title: 'Printer Name', dataIndex: 'printerName', key: 'printerName' },
    { title: 'Type', dataIndex: 'type', key: 'type' },
    { title: 'IP/Port', dataIndex: 'ipPort', key: 'ipPort' },
    { title: 'Default', dataIndex: 'isDefault', key: 'isDefault', render: (v: boolean) => v ? 'Yes' : 'No' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try { const r = await api.get('/setup/printer'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Printer Setup" extra={<Button type="primary" icon={<PlusOutlined />} onClick={() => {}}>Add</Button>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PrinterSetupPage;
