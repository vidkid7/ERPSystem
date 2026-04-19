import React from 'react';
import { Card, Typography, Button, Space } from 'antd';
import { ArrowLeftOutlined, SaveOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

const { Title } = Typography;

interface FormPageProps {
  title: string;
  loading?: boolean;
  onSubmit?: () => void;
  children: React.ReactNode;
  backPath?: string;
}

const FormPage: React.FC<FormPageProps> = ({ title, loading, onSubmit, children, backPath }) => {
  const navigate = useNavigate();

  return (
    <Card>
      <div style={{ display: 'flex', justifyContent: 'space-between', marginBottom: 24 }}>
        <Space>
          {backPath && (
            <Button icon={<ArrowLeftOutlined />} onClick={() => navigate(backPath)} />
          )}
          <Title level={4} style={{ margin: 0 }}>{title}</Title>
        </Space>
        {onSubmit && (
          <Button type="primary" icon={<SaveOutlined />} loading={loading} onClick={onSubmit}>
            Save
          </Button>
        )}
      </div>
      {children}
    </Card>
  );
};

export default FormPage;
