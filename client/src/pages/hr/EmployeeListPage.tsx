import React, { useEffect, useState } from 'react';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';
import type { Employee } from '../../types';

const columns = [
  { title: 'Code', dataIndex: 'employeeCode', key: 'employeeCode', width: 100 },
  { title: 'Name', dataIndex: 'fullName', key: 'fullName' },
  { title: 'Phone', dataIndex: 'phone', key: 'phone' },
  { title: 'Email', dataIndex: 'email', key: 'email' },
  { title: 'Branch', dataIndex: 'branchName', key: 'branchName' },
  { title: 'Joining Date', dataIndex: 'joiningDate', key: 'joiningDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
];

const EmployeeListPage: React.FC = () => {
  const [data, setData] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');

  const fetchData = async (p = page, s = search) => {
    setLoading(true);
    try {
      const res = await api.get('/hr/employee', { params: { search: s, page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, []);

  return (
    <ListPage<Employee>
      title="Employees" columns={columns} dataSource={data} loading={loading}
      total={total} page={page} onSearch={(s) => { setSearch(s); fetchData(1, s); }}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()} addButtonText="Add Employee"
    />
  );
};

export default EmployeeListPage;
