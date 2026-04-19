import React, { useEffect, useState } from 'react';
import { Card, Table, Typography, Tag, Button } from 'antd';
import { ArrowLeftOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import api from '../../services/api';

const { Title } = Typography;

interface EMIItem {
  id: number;
  emiNumber: number;
  dueDate: string;
  emiAmount: number;
  principalAmount: number;
  interestAmount: number;
  balanceAmount: number;
  status: string;
}

const statusColor: Record<string, string> = { Paid: 'green', Pending: 'orange', Overdue: 'red' };

const columns = [
  { title: 'EMI #', dataIndex: 'emiNumber', key: 'emiNumber', width: 80 },
  { title: 'Due Date', dataIndex: 'dueDate', key: 'dueDate', render: (v: string) => v ? new Date(v).toLocaleDateString() : '-' },
  { title: 'EMI Amount', dataIndex: 'emiAmount', key: 'emiAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Principal', dataIndex: 'principalAmount', key: 'principalAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Interest', dataIndex: 'interestAmount', key: 'interestAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Balance', dataIndex: 'balanceAmount', key: 'balanceAmount', render: (v: number) => v?.toFixed(2), align: 'right' as const },
  { title: 'Status', dataIndex: 'status', key: 'status', render: (s: string) => <Tag color={statusColor[s] || 'default'}>{s}</Tag> },
];

const EMISchedulePage: React.FC = () => {
  const [data, setData] = useState<EMIItem[]>([]);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { loanId } = useParams<{ loanId: string }>();

  const fetchData = async () => {
    setLoading(true);
    try {
      const res = await api.get(`/finance/loan/${loanId}/emi-schedule`);
      setData(res.data.data || []);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, [loanId]);

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <div style={{ display: 'flex', alignItems: 'center', gap: 8 }}>
          <Button icon={<ArrowLeftOutlined />} onClick={() => navigate('/finance/loans')} />
          <Title level={4} style={{ margin: 0 }}>EMI Schedule</Title>
        </div>
      </div>
      <Table
        columns={columns}
        dataSource={data}
        loading={loading}
        rowKey="id"
        size="middle"
        pagination={false}
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
};

export default EMISchedulePage;
