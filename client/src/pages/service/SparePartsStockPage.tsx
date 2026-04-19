import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, Input, Button, Space } from 'antd';
import type { ColumnsType } from 'antd/es/table';
import api from '../../services/api';

const { Title } = Typography;
const { Search } = Input;

interface SparePartStock {
  id: number;
  partNo: string;
  name: string;
  stock: number;
  minLevel: number;
  price: number;
}

const columns: ColumnsType<SparePartStock> = [
  { title: 'Part No', dataIndex: 'partNo', key: 'partNo', width: 130 },
  { title: 'Name', dataIndex: 'name', key: 'name' },
  { title: 'Stock', dataIndex: 'stock', key: 'stock', align: 'right', width: 100 },
  { title: 'Min Level', dataIndex: 'minLevel', key: 'minLevel', align: 'right', width: 100 },
  { title: 'Price', dataIndex: 'price', key: 'price', align: 'right', width: 110, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const SparePartsStockPage: React.FC = () => {
  const [data, setData] = useState<SparePartStock[]>([]);
  const [loading, setLoading] = useState(false);
  const [search, setSearch] = useState('');

  const fetchData = async (s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/spare-parts-stock', { params: { search: s, pageSize: 1000 } });
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <Card>
      <Title level={4}>Spare Parts Stock</Title>
      <Space style={{ marginBottom: 16 }} wrap>
        <Search placeholder="Search parts..." onSearch={(v) => { setSearch(v); fetchData(v); }} allowClear style={{ width: 280 }} />
        <Button onClick={() => fetchData()}>Refresh</Button>
        <Button>Export</Button>
      </Space>
      <Table columns={columns} dataSource={data} loading={loading}
        rowKey="id" size="small" scroll={{ x: 650 }}
        pagination={{ pageSize: 50, showTotal: (t) => `Total ${t} records` }}
        rowClassName={(r) => r.stock <= r.minLevel ? 'ant-table-row-warning' : ''}
      />
    </Card>
  );
};

export default SparePartsStockPage;
