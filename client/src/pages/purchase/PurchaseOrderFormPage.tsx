import React, { useEffect, useState, useCallback } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Table, Button, Space, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Vendor, Product } from '../../types';

interface LineItem {
  key: number;
  productId?: number;
  quantity: number;
  rate: number;
  amount: number;
}

let lineKey = 1;

const PurchaseOrderFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [vendors, setVendors] = useState<Vendor[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [lines, setLines] = useState<LineItem[]>([{ key: lineKey, quantity: 0, rate: 0, amount: 0 }]);

  useEffect(() => {
    Promise.all([
      api.get('/purchase/vendor', { params: { pageSize: 1000 } }),
      api.get('/inventory/product', { params: { pageSize: 1000 } }),
    ]).then(([vRes, pRes]) => {
      setVendors(vRes.data.data || []);
      setProducts(pRes.data.data || []);
    });
  }, []);

  const recalcLine = useCallback((index: number, field: string, value: number) => {
    setLines((prev) => {
      const updated = [...prev];
      const line = { ...updated[index], [field]: value };
      line.amount = (line.quantity || 0) * (line.rate || 0);
      updated[index] = line;
      return updated;
    });
  }, []);

  const addLine = () => {
    lineKey += 1;
    setLines((prev) => [...prev, { key: lineKey, quantity: 0, rate: 0, amount: 0 }]);
  };

  const removeLine = (key: number) => {
    setLines((prev) => prev.filter((l) => l.key !== key));
  };

  const totalAmount = lines.reduce((sum, l) => sum + (l.amount || 0), 0);

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
        quotationRef: values.quotationRef,
        expectedDelivery: values.expectedDelivery?.format('YYYY-MM-DD'),
        remarks: values.remarks,
        totalAmount,
        details: lines.filter((l) => l.productId).map((l) => ({
          productId: l.productId,
          quantity: l.quantity,
          rate: l.rate,
          amount: l.amount,
        })),
      };
      await api.post('/purchase/order', payload);
      message.success('Purchase order created');
      navigate('/purchase/orders');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally {
      setLoading(false);
    }
  };

  const lineColumns = [
    {
      title: 'Product', dataIndex: 'productId', width: 250,
      render: (_: any, record: LineItem, index: number) => (
        <Select style={{ width: '100%' }} showSearch optionFilterProp="children" placeholder="Select product"
          value={record.productId}
          onChange={(v) => { setLines((prev) => { const u = [...prev]; u[index] = { ...u[index], productId: v }; return u; }); }}
          options={products.map((p) => ({ label: `${p.code} - ${p.name}`, value: p.id }))}
        />
      ),
    },
    {
      title: 'Quantity', dataIndex: 'quantity', width: 120,
      render: (_: any, record: LineItem, index: number) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2} value={record.quantity}
          onChange={(v) => recalcLine(index, 'quantity', v || 0)} />
      ),
    },
    {
      title: 'Rate', dataIndex: 'rate', width: 120,
      render: (_: any, record: LineItem, index: number) => (
        <InputNumber style={{ width: '100%' }} min={0} precision={2} value={record.rate}
          onChange={(v) => recalcLine(index, 'rate', v || 0)} />
      ),
    },
    {
      title: 'Amount', dataIndex: 'amount', width: 120,
      render: (v: number) => (v || 0).toFixed(2),
    },
    {
      title: '', width: 50,
      render: (_: any, record: LineItem) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeLine(record.key)} />
      ),
    },
  ];

  return (
    <FormPage title="New Purchase Order" loading={loading} onSubmit={handleSubmit} backPath="/purchase/orders">
      <Form form={form} layout="vertical">
        <Space style={{ display: 'flex', flexWrap: 'wrap' }} size="large">
          <Form.Item name="vendorId" label="Vendor" rules={[{ required: true, message: 'Vendor is required' }]} style={{ minWidth: 250 }}>
            <Select showSearch optionFilterProp="children" placeholder="Select vendor"
              options={vendors.map((v) => ({ label: `${v.code} - ${v.name}`, value: v.id }))} />
          </Form.Item>
          <Form.Item name="date" label="Date" rules={[{ required: true, message: 'Date is required' }]}>
            <DatePicker style={{ width: 180 }} />
          </Form.Item>
          <Form.Item name="quotationRef" label="Quotation Ref">
            <Input style={{ width: 180 }} />
          </Form.Item>
          <Form.Item name="expectedDelivery" label="Expected Delivery">
            <DatePicker style={{ width: 180 }} />
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
      <Table dataSource={lines} columns={lineColumns} rowKey="key" pagination={false} size="small" scroll={{ x: 'max-content' }}
        summary={() => (
          <Table.Summary fixed>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0} colSpan={3} align="right"><strong>Total</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={3}><strong>{totalAmount.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={4} />
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />
    </FormPage>
  );
};

export default PurchaseOrderFormPage;
