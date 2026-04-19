import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const WardOccupancyPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const columns = [
    { title: 'Ward', dataIndex: 'ward', key: 'ward' },
    { title: 'Beds', dataIndex: 'beds', key: 'beds' },
    { title: 'Occupied', dataIndex: 'occupied', key: 'occupied' },
    { title: 'Available', dataIndex: 'available', key: 'available' },
    { title: 'Percentage', dataIndex: 'percentage', key: 'percentage', render: (v: number) => `${v}%` },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try { const r = await api.get('/hms/ward-occupancy'); setData(r.data?.Data || []); }
    catch { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card title="Ward Occupancy" extra={<Space><RangePicker /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}>
      <Table columns={columns} dataSource={data} loading={loading} rowKey="ward" size="small" />
    </Card>
  );
};
export default WardOccupancyPage;
