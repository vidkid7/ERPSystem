import React, { useState } from 'react';
import { Card, Table, DatePicker, Button, Space } from 'antd';
import api from '../../services/api';
const { RangePicker } = DatePicker;

const ProductVoucherPage: React.FC = () => {
  const [data, setData] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [dates, setDates] = useState<any>(null);
  const columns = [
    { title: 'Date', dataIndex: 'date', key: 'date' },
    { title: 'Voucher', dataIndex: 'voucher', key: 'voucher' },
    { title: 'Product', dataIndex: 'product', key: 'product' },
    { title: 'In Qty', dataIndex: 'inQty', key: 'inQty', align: 'right' as const },
    { title: 'Out Qty', dataIndex: 'outQty', key: 'outQty', align: 'right' as const },
    { title: 'Balance', dataIndex: 'balance', key: 'balance', align: 'right' as const },
  ];
  const handleSearch = async () => {
    setLoading(true);
    try {
      const params = dates
        ? { fromDate: dates[0]?.format('YYYY-MM-DD'), toDate: dates[1]?.format('YYYY-MM-DD') }
        : {};
      const r = await api.get('/inventory/product-voucher', { params });
      setData(r.data?.Data || []);
    } catch (e) { setData([]); }
    finally { setLoading(false); }
  };
  return (
    <Card
      title="Product Voucher"
      extra={<Space><RangePicker onChange={setDates} /><Button type="primary" onClick={handleSearch}>Search</Button></Space>}
    >
      <Table columns={columns} dataSource={data} loading={loading} rowKey="id" size="small" />
    </Card>
  );
};
export default ProductVoucherPage;
