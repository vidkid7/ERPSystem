import React, { useEffect, useState } from 'react';
import { Tag } from 'antd';
import { useParams } from 'react-router-dom';
import ListPage from '../../components/common/ListPage';
import api from '../../services/api';

interface AssetTransaction {
  id: number;
  transactionType: string;
  transactionDate: string;
  fromEmployeeName: string;
  toEmployeeName: string;
  amount: number;
  documentNo: string;
  remarks: string;
}

const typeColors: Record<string, string> = {
  Inward: 'green', Issue: 'blue', Transfer: 'cyan',
  Return: 'purple', Damage: 'red', Repair: 'orange', Disposal: 'default',
};

const columns = [
  { title: 'Type', dataIndex: 'transactionType', key: 'transactionType', width: 110,
    render: (v: string) => <Tag color={typeColors[v] || 'default'}>{v}</Tag>,
  },
  { title: 'Date', dataIndex: 'transactionDate', key: 'transactionDate', width: 120,
    render: (v: string) => v ? new Date(v).toLocaleDateString() : '',
  },
  { title: 'From Employee', dataIndex: 'fromEmployeeName', key: 'fromEmployeeName' },
  { title: 'To Employee', dataIndex: 'toEmployeeName', key: 'toEmployeeName' },
  { title: 'Amount', dataIndex: 'amount', key: 'amount', width: 110,
    render: (v: number) => v?.toLocaleString(undefined, { minimumFractionDigits: 2 }),
  },
  { title: 'Document No', dataIndex: 'documentNo', key: 'documentNo', width: 130 },
  { title: 'Remarks', dataIndex: 'remarks', key: 'remarks' },
];

const AssetTransactionListPage: React.FC = () => {
  const { assetId } = useParams<{ assetId: string }>();
  const [data, setData] = useState<AssetTransaction[]>([]);
  const [loading, setLoading] = useState(false);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);

  const fetchData = async (p = page) => {
    setLoading(true);
    try {
      const res = await api.get(`/assettransaction/${assetId}`, { params: { page: p, pageSize: 20 } });
      setData(res.data.data || []);
      setTotal(res.data.totalCount || 0);
    } finally { setLoading(false); }
  };

  useEffect(() => { fetchData(); }, [assetId]);

  return (
    <ListPage<AssetTransaction>
      title={`Asset Transactions - Asset #${assetId}`}
      columns={columns} dataSource={data} loading={loading}
      total={total} page={page}
      onPageChange={(p) => { setPage(p); fetchData(p); }}
      onRefresh={() => fetchData()}
    />
  );
};

export default AssetTransactionListPage;
