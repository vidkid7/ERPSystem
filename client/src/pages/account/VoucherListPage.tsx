import React, { useEffect, useState } from 'react';
import { Tag, DatePicker, Space } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Voucher } from '../../types';

const { RangePicker } = DatePicker;

const columns = [
  { title: 'Voucher No', dataIndex: 'voucherNumber', key: 'voucherNumber', width: 140 },
  { title: 'Date', dataIndex: 'voucherDate', key: 'voucherDate', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '' },
  { title: 'Total Debit', dataIndex: 'totalDebit', key: 'totalDebit', width: 130,
    render: (v: number) => v?.toFixed(2) },
  { title: 'Total Credit', dataIndex: 'totalCredit', key: 'totalCredit', width: 130,
    render: (v: number) => v?.toFixed(2) },
  { title: 'Status', dataIndex: 'isPosted', key: 'isPosted', width: 100,
    render: (v: boolean) => (
      <Tag color={v ? 'green' : 'orange'}>{v ? 'Posted' : 'Draft'}</Tag>
    ),
  },
];

const VoucherListPage: React.FC = () => {
  const [data, setData] = useState<Voucher[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [dateRange, setDateRange] = useState<[string, string] | null>(null);

  const fetchData = async (p = page, s = search, dr = dateRange) => {
    setLoading(true);
    try {
      const params: Record<string, any> = { search: s, page: p, pageSize: 20 };
      if (dr) {
        params.fromDate = dr[0];
        params.toDate = dr[1];
      }
      const res = await api.get('/account/voucher', { params });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  const handleDateChange = (_: any, dateStrings: [string, string]) => {
    const dr = dateStrings[0] ? dateStrings : null;
    setDateRange(dr);
    fetchData(1, search, dr);
  };

  return (
    <div>
      <Space style={{ marginBottom: 16 }}>
        <RangePicker onChange={handleDateChange} />
      </Space>
      <ListPage<Voucher>
        title="Vouchers" columns={columns} dataSource={data} loading={loading}
        total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
        onPageChange={(p) => { setPage(p); fetchData(p); }}
        onRefresh={() => fetchData()}
      />
    </div>
  );
};

export default VoucherListPage;
