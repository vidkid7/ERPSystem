import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface ServiceContract {
  id: number;
  contractNo: string;
  customer: string;
  product: string;
  startDate: string;
  endDate: string;
  amount: number;
}

const columns = [
  { title: 'Contract No', dataIndex: 'contractNo', key: 'contractNo', width: 130 },
  { title: 'Customer', dataIndex: 'customer', key: 'customer' },
  { title: 'Product', dataIndex: 'product', key: 'product' },
  { title: 'Start', dataIndex: 'startDate', key: 'startDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'End', dataIndex: 'endDate', key: 'endDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', align: 'right' as const, width: 120, render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }) },
];

const ServiceContractListPage: React.FC = () => {
  const [data, setData] = useState<ServiceContract[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/service/contract', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<ServiceContract>
      title="Service Contracts" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Contract"
    />
  );
};

export default ServiceContractListPage;
