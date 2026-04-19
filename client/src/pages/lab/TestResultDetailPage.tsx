import React, { useEffect, useState } from 'react';
import { Form, Input, Table, Card, Typography, Button, Space, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { Title } = Typography;

interface Parameter {
  key: string;
  parameter: string;
  result: string;
  referenceRange: string;
  unit: string;
}

const TestResultDetailPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [parameters, setParameters] = useState<Parameter[]>([]);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();

  useEffect(() => {
    if (id) {
      api.get(`/lab/test-result/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue(d);
        setParameters(d?.parameters || []);
      });
    }
  }, [id]);

  const paramColumns = [
    { title: 'Parameter', dataIndex: 'parameter', key: 'parameter' },
    { title: 'Result', dataIndex: 'result', key: 'result' },
    { title: 'Reference Range', dataIndex: 'referenceRange', key: 'referenceRange' },
    { title: 'Unit', dataIndex: 'unit', key: 'unit', width: 100 },
  ];

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      await api.post(`/lab/test-result/${id}`, { ...values, parameters });
      message.success('Test result saved');
      navigate('/lab/reports');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title="Test Result Detail" loading={loading} onSubmit={handleSubmit} backPath="/lab/reports">
      <Form form={form} layout="vertical">
        <Form.Item name="patientName" label="Patient" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="testName" label="Test">
          <Input />
        </Form.Item>
        <Form.Item name="sampleId" label="Sample ID">
          <Input />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks">
          <Input.TextArea rows={2} />
        </Form.Item>
      </Form>
      <Title level={5} style={{ marginTop: 16 }}>Parameters</Title>
      <Table
        columns={paramColumns}
        dataSource={parameters}
        rowKey="key"
        size="small"
        pagination={false}
        scroll={{ x: 600 }}
      />
    </FormPage>
  );
};

export default TestResultDetailPage;
