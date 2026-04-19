import React, { useEffect, useState } from 'react';
import { Form, Input, Table, Typography, Button, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { Title } = Typography;

interface CheckParameter {
  key: string;
  parameter: string;
  specification: string;
  result: string;
}

const QualityCheckFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const [parameters, setParameters] = useState<CheckParameter[]>([]);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/inventory/quality-check/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue(d);
        setParameters(d?.parameters || []);
      });
    }
  }, [id]);

  const addParameter = () => {
    setParameters(prev => [...prev, { key: Date.now().toString(), parameter: '', specification: '', result: '' }]);
  };

  const removeParameter = (key: string) => {
    setParameters(prev => prev.filter(p => p.key !== key));
  };

  const paramColumns = [
    { title: 'Parameter', dataIndex: 'parameter', key: 'parameter' },
    { title: 'Specification', dataIndex: 'specification', key: 'specification' },
    { title: 'Result', dataIndex: 'result', key: 'result', width: 100 },
    {
      title: '', key: 'action', width: 50,
      render: (_: unknown, record: CheckParameter) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeParameter(record.key)} />
      ),
    },
  ];

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      const payload = { ...values, parameters };
      if (isEdit) {
        await api.post(`/inventory/quality-check/${id}`, payload);
        message.success('Quality check updated');
      } else {
        await api.post('/inventory/quality-check', payload);
        message.success('Quality check created');
      }
      navigate('/inventory/quality-checks');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Quality Check' : 'New Quality Check'} loading={loading} onSubmit={handleSubmit} backPath="/inventory/quality-checks">
      <Form form={form} layout="vertical">
        <Form.Item name="referenceNo" label="Reference No" rules={[{ required: true, message: 'Please enter reference number' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="product" label="Product" rules={[{ required: true, message: 'Please enter product' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="inspector" label="Inspector">
          <Input />
        </Form.Item>
        <Form.Item name="remarks" label="Remarks">
          <Input.TextArea rows={2} />
        </Form.Item>
      </Form>
      <Title level={5} style={{ marginTop: 16 }}>Check Parameters</Title>
      <Table columns={paramColumns} dataSource={parameters} rowKey="key" size="small" pagination={false} scroll={{ x: 500 }} />
      <Button type="dashed" icon={<PlusOutlined />} onClick={addParameter} style={{ marginTop: 8 }}>Add Parameter</Button>
    </FormPage>
  );
};

export default QualityCheckFormPage;
