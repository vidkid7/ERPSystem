import React, { useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import type { Dayjs } from 'dayjs';
import api from '../../services/api';

const { RangePicker } = DatePicker;

interface AssetReturnReportRow {
  id: number;
  asset: string;
  employee: string;
  returnDate: string;
  condition: string;
  remarks: string;
}

const columns: ColumnsType<AssetReturnReportRow> = [
  { title: 'Asset', dataIndex: 'asset', key: 'asset' },
  { title: 'Employee', dataIndex: 'employee', key: 'employee' },
  { title: 'Return Date', dataIndex: 'returnDate', key: 'returnDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 120 },
  { title: 'Condition', dataIndex: 'condition', key: 'condition', width: 110 },
  { title: 'Remarks', dataIndex: 'remarks', key: 'remarks' },
];

const AssetReturnReportPage: React.FC = () => {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<AssetReturnReportRow[]>([]);
  const [dates, setDates] = useState<[Dayjs | null, Dayjs | null] | null>(null);

  const handleSearch = async () => {
    setLoading(true);
    try {
      const params: Record<string, string> = {};
      if (dates?.[0]) params.fromDate = dates[0].format('YYYY-MM-DD');
      if (dates?.[1]) params.toDate = dates[1].format('YYYY-MM-DD');
      const res = await api.get('/asset/return-report', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  return (
    <Card title="Asset Return Report">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(d) => setDates(d as [Dayjs | null, Dayjs | null])} />
        <Button type="primary" onClick={handleSearch} loading={loading}>Search</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 700 }} pagination={{ pageSize: 50 }} />
    </Card>
  );
};

export default AssetReturnReportPage;
