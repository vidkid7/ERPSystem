import React, { useEffect, useState } from 'react';
import { Card, DatePicker, Button, Table, Space } from 'antd';
import api from '../../services/api';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

const CancelDayBookPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dateRange, setDateRange] = useState<[dayjs.Dayjs, dayjs.Dayjs] | null>(null);

  const columns = [
    { title: 'Voucher No', dataIndex: 'voucherNo', key: 'voucherNo' },
    { title: 'Voucher Type', dataIndex: 'voucherType', key: 'voucherType' },
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const },
    { title: 'Cancelled By', dataIndex: 'cancelledBy', key: 'cancelledBy' },
    { title: 'Reason', dataIndex: 'reason', key: 'reason' },
  ];

  const fetchData = async () => {
    setLoading(true);
    try {
      const params: any = {};
      if (dateRange) {
        params.fromDate = dateRange[0].format('YYYY-MM-DD');
        params.toDate = dateRange[1].format('YYYY-MM-DD');
      }
      const res = await api.get('/reporting/cancelled-vouchers', { params });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card title="Cancelled Vouchers">
      <Space style={{ marginBottom: 16 }} wrap>
        <RangePicker onChange={(dates) => setDateRange(dates as any)} />
        <Button type="primary" onClick={fetchData}>Generate Report</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey={(_, i) => String(i)} pagination={{ pageSize: 50 }} scroll={{ x: 900 }} />
    </Card>
  );
};

export default CancelDayBookPage;
