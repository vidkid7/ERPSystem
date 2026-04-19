import React, { useEffect, useState } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Table, Button, Space, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Vendor, Product } from '../../types';

interface GRNLine {
  key: number;
  productId?: number;
  orderedQty: number;
  receivedQty: number;
  acceptedQty: number;
  rejectedQty: number;
}

let lineKey = 1;

const ReceiptNoteFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [vendors, setVendors] = useState<Vendor[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [lines, setLines] = useState<GRNLine[]>([{ key: lineKey, orderedQty: 0, receivedQty: 0, acceptedQty: 0, rejectedQty: 0 }]);

  useEffect(() => {
    Promise.all([
      api.get('/purchase/vendor', { params: { pageSize: 1000 } }),
      api.get('/inventory/product', { params: { pageSize: 1000 } }),
    ]).then(([vRes, pRes]) => {
      setVendors(vRes.data.data || []);
      setProducts(pRes.data.data || []);
    });
  }, []);

  const updateLine = (index: number, field: string, value: any) => {
    setLines((prev) => {
      const updated = [...prev];
      const line = { ...updated[index], [field]: value };
      if (field === 'receivedQty' || field === 'rejectedQty') {
        line.acceptedQty = Math.max(0, (line.receivedQty || 0) - (line.rejectedQty || 0));
      }
      updated[index] = line;
      return updated;
    });
  };

  const addLine = () => {
    lineKey += 1;
    setLines((prev) => [...prev, { key: lineKey, orderedQty: 0, receivedQty: 0, acceptedQty: 0, rejectedQty: 0 }]);
  };

  const removeLine = (key: number) => {
    setLines((prev) => prev.filter((l) => l.key !== key));
  };

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      if (!lines.length || lines.every((l) => !l.productId)) {
        message.warning('Add at least one line item');
        return;
      }
      setLoading(true);
      const payload = {
        vendorId: values.vendorId,
        date: values.date?.format('YYYY-MM-DD'),
        purchaseOrderRef: values.purchaseOrderRef,
        remarks: values.remarks,
        details: lines.filter((l) => l.productId).map((l) => ({
          productId: l.productId,
          orderedQty: l.orderedQty,
          receivedQty: l.receivedQty,
          acceptedQty: l.acceptedQty,
          rejectedQty: l.rejectedQty,
        })),
      };
      await api.post('/purchase/receipt-note', payload);
      message.success('GRN created successfully');
      navigate('/purchase/receipt-notes');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally {
      setLoading(false);
    }
  };

  const lineColumns = [
    {
      title: 'Product', dataIndex: 'productId', width: 250,
      render: (_: any, record: GRNLine, index: number) => (
        <Select style={{ width: '100%' }} showSearch optionFilterProp="children" placeholder="Select product"
          value={record.productId}
          onChange={(v) => updateLine(index, 'productId', v)}
          options={products.map((p) => ({ label: `${p.code} - ${p.name}`, value: p.id }))}
        />
      ),
    },
    {
      title: 'Ordered Qty', dataIndex: 'orderedQty', width: 120,
      render: (_: any, record: GRNLine, index: number) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2} value={record.orderedQty}
          onChange={(v) => updateLine(index, 'orderedQty', v || 0)} />
      ),
    },
    {
      title: 'Received Qty', dataIndex: 'receivedQty', width: 120,
      render: (_: any, record: GRNLine, index: number) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2} value={record.receivedQty}
          onChange={(v) => updateLine(index, 'receivedQty', v || 0)} />
      ),
    },
    {
      title: 'Accepted Qty', dataIndex: 'acceptedQty', width: 120,
      render: (v: number) => (v || 0).toFixed(2),
    },
    {
      title: 'Rejected Qty', dataIndex: 'rejectedQty', width: 120,
      render: (_: any, record: GRNLine, index: number) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2} value={record.rejectedQty}
          onChange={(v) => updateLine(index, 'rejectedQty', v || 0)} />
      ),
    },
    {
      title: '', width: 50,
      render: (_: any, record: GRNLine) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeLine(record.key)} />
      ),
    },
  ];

  return (
    <FormPage title="New Goods Receipt Note" loading={loading} onSubmit={handleSubmit} backPath="/purchase/receipt-notes">
      <Form form={form} layout="vertical">
        <Space style={{ display: 'flex', flexWrap: 'wrap' }} size="large">
          <Form.Item name="vendorId" label="Vendor" rules={[{ required: true, message: 'Vendor is required' }]} style={{ minWidth: 250 }}>
            <Select showSearch optionFilterProp="children" placeholder="Select vendor"
              options={vendors.map((v) => ({ label: `${v.code} - ${v.name}`, value: v.id }))} />
          </Form.Item>
          <Form.Item name="date" label="Date" rules={[{ required: true, message: 'Date is required' }]}>
            <DatePicker style={{ width: 180 }} />
          </Form.Item>
          <Form.Item name="purchaseOrderRef" label="PO Reference">
            <Input style={{ width: 180 }} />
          </Form.Item>
          <Form.Item name="remarks" label="Remarks">
            <Input style={{ width: 250 }} />
          </Form.Item>
        </Space>
      </Form>

      <div style={{ marginBottom: 8, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
        <strong>Line Items</strong>
        <Button type="dashed" icon={<PlusOutlined />} onClick={addLine}>Add Line</Button>
      </div>
      <Table dataSource={lines} columns={lineColumns} rowKey="key" pagination={false} size="small" scroll={{ x: 'max-content' }} />
    </FormPage>
  );
};

export default ReceiptNoteFormPage;
