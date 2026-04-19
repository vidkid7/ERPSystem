import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface MaterialRequisition {
  id: number;
  mrNo: string;
  department: string;
  date: string;
  status: string;
  totalItems: number;
}

const statusColor: Record<string, string> = { Pending: 'orange', Approved: 'green', Rejected: 'red', Issued: 'blue' };

const columns = [
  { title: 'MR No', dataIndex: 'mrNo', key: 'mrNo', width: 130 },
  { title: 'Department', dataIndex: 'department', key: 'department' },
  { title: 'Date', dataIndex: 'date', key: 'date', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-', width: 100 },
  { title: 'Status', dataIndex: 'status', key: 'status', width: 110, render: (v: string) => <Tag color={statusColor[v] || 'default'}>{v}</Tag> },
  { title: 'Total Items', dataIndex: 'totalItems', key: 'totalItems', align: 'right' as const, width: 110 },
];

const MaterialRequisitionListPage: React.FC = () => {
  const [data, setData] = useState<MaterialRequisition[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/inventory/material-requisition', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<MaterialRequisition>
      title="Material Requisitions" columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="New Requisition"
    />
  );
};

export default MaterialRequisitionListPage;
