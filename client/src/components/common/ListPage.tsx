import React from 'react';
import { Table, Input, Button, Space, Card, Typography } from 'antd';
import { PlusOutlined, SearchOutlined, ReloadOutlined } from '@ant-design/icons';
import type { ColumnsType } from 'antd/es/table';

const { Title } = Typography;

interface ListPageProps<T> {
  title: string;
  columns: ColumnsType<T>;
  dataSource: T[];
  loading?: boolean;
  total?: number;
  onSearch?: (value: string) => void;
  onAdd?: () => void;
  onRefresh?: () => void;
  onPageChange?: (page: number, pageSize: number) => void;
  page?: number;
  pageSize?: number;
  addButtonText?: string;
}

function ListPage<T extends { id?: number }>({
  title, columns, dataSource, loading, total, onSearch, onAdd, onRefresh,
  onPageChange, page = 1, pageSize = 20, addButtonText = 'Add New',
}: ListPageProps<T>) {
  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 16 }}>
        <Title level={4} style={{ margin: 0 }}>{title}</Title>
        <Space>
          {onSearch && (
            <Input.Search placeholder="Search..." allowClear onSearch={onSearch}
              style={{ width: 250 }} prefix={<SearchOutlined />} />
          )}
          {onRefresh && <Button icon={<ReloadOutlined />} onClick={onRefresh} />}
          {onAdd && <Button type="primary" icon={<PlusOutlined />} onClick={onAdd}>{addButtonText}</Button>}
        </Space>
      </div>
      <Table<T>
        columns={columns}
        dataSource={dataSource}
        loading={loading}
        rowKey="id"
        pagination={{
          current: page,
          pageSize,
          total,
          showSizeChanger: true,
          showTotal: (t) => `Total ${t} records`,
          onChange: onPageChange,
        }}
        size="middle"
        scroll={{ x: 'max-content' }}
      />
    </Card>
  );
}

export default ListPage;
