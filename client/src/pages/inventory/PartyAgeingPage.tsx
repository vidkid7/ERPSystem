import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const PartyAgeingPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Party', dataIndex: 'party', key: 'party' },
    { title: '0-30 Days', dataIndex: 'days030', key: 'days030', align: 'right' as const },
    { title: '31-60 Days', dataIndex: 'days3160', key: 'days3160', align: 'right' as const },
    { title: '61-90 Days', dataIndex: 'days6190', key: 'days6190', align: 'right' as const },
    { title: '90+ Days', dataIndex: 'days90plus', key: 'days90plus', align: 'right' as const },
    { title: 'Total', dataIndex: 'total', key: 'total', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/party-ageing', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Party Ageing"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default PartyAgeingPage;
