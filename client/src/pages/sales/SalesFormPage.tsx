import React, { useEffect, useState, useCallback } from 'react';
import { Form, Input, Select, DatePicker, InputNumber, Table, Button, Space, message } from 'antd';
import { PlusOutlined, DeleteOutlined } from '@ant-design/icons';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';
import type { Customer, Product } from '../../types';

interface LineItem {
  key: number;
  productId?: number;
  quantity: number;
  rate: number;
  amount: number;
  taxAmount: number;
}

let lineKey = 1;

const SalesFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  const [loading, setLoading] = useState(false);
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [products, setProducts] = useState<Product[]>([]);
  const [lines, setLines] = useState<LineItem[]>([{ key: lineKey, quantity: 0, rate: 0, amount: 0, taxAmount: 0 }]);

  useEffect(() => {
    Promise.all([
      api.get('/sales/customer', { params: { pageSize: 1000 } }),
      api.get('/inventory/product', { params: { pageSize: 1000 } }),
    ]).then(([cRes, pRes]) => {
      setCustomers(cRes.data.data || []);
      setProducts(pRes.data.data || []);
    });

    if (isEdit) {
      api.get(`/sales/salesinvoice/${id}`).then((res) => {
        const inv = res.data.data || res.data;
        form.setFieldsValue({
          customerId: inv.customerId,
          invoiceDate: inv.invoiceDate ? dayjs(inv.invoiceDate) : undefined,
          invoiceNo: inv.invoiceNumber,
          remarks: inv.remarks,
        });
        if (inv.details?.length) {
          setLines(
            inv.details.map((d: any, i: number) => ({
              key: i + 1,
              productId: d.productId,
              quantity: d.quantity,
              rate: d.rate,
              amount: d.amount,
              taxAmount: d.taxAmount || 0,
            }))
          );
          lineKey = inv.details.length + 1;
        }
      });
    }
  }, [id]);

  const recalcLine = useCallback(
    (index: number, field: string, value: number) => {
      setLines((prev) => {
        const updated = [...prev];
        const line = { ...updated[index], [field]: value };
        line.amount = (line.quantity || 0) * (line.rate || 0);
        updated[index] = line;
        return updated;
      });
    },
    []
  );

  const addLine = () => {
    lineKey += 1;
    setLines((prev) => [...prev, { key: lineKey, quantity: 0, rate: 0, amount: 0, taxAmount: 0 }]);
  };

  const removeLine = (key: number) => {
    setLines((prev) => prev.filter((l) => l.key !== key));
  };

  const totals = lines.reduce(
    (acc, l) => ({
      totalAmount: acc.totalAmount + (l.amount || 0),
      taxAmount: acc.taxAmount + (l.taxAmount || 0),
      grandTotal: acc.grandTotal + (l.amount || 0) + (l.taxAmount || 0),
    }),
    { totalAmount: 0, taxAmount: 0, grandTotal: 0 }
  );

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      if (!lines.length || lines.every((l) => !l.productId)) {
        message.warning('Add at least one line item');
        return;
      }
      setLoading(true);
      const payload = {
        customerId: values.customerId,
        invoiceDate: values.invoiceDate?.format('YYYY-MM-DD'),
        invoiceNumber: values.invoiceNo,
        remarks: values.remarks,
        details: lines
          .filter((l) => l.productId)
          .map((l) => ({
            productId: l.productId,
            quantity: l.quantity,
            rate: l.rate,
            amount: l.amount,
            taxAmount: l.taxAmount,
          })),
      };

      if (isEdit) {
        await api.put(`/sales/salesinvoice/${id}`, { ...payload, id: Number(id) });
        message.success('Sales invoice updated');
      } else {
        await api.post('/sales/salesinvoice', payload);
        message.success('Sales invoice created');
      }
      navigate('/sales');
    } catch (err: any) {
      if (err.response) message.error(err.response.data?.responseMSG || 'Save failed');
    } finally {
      setLoading(false);
    }
  };

  const lineColumns = [
    {
      title: 'Product',
      dataIndex: 'productId',
      width: 250,
      render: (_: any, record: LineItem, index: number) => (
        <Select
          style={{ width: '100%' }}
          showSearch
          optionFilterProp="children"
          placeholder="Select product"
          value={record.productId}
          onChange={(v) => {
            setLines((prev) => {
              const updated = [...prev];
              updated[index] = { ...updated[index], productId: v };
              return updated;
            });
          }}
          options={products.map((p) => ({ label: `${p.code} - ${p.name}`, value: p.id }))}
        />
      ),
    },
    {
      title: 'Quantity',
      dataIndex: 'quantity',
      width: 120,
      render: (_: any, record: LineItem, index: number) => (
        <InputNumber
          style={{ width: '100%' }}
          min={0}
          precision={2}
          value={record.quantity}
          onChange={(v) => recalcLine(index, 'quantity', v || 0)}
        />
      ),
    },
    {
      title: 'Rate',
      dataIndex: 'rate',
      width: 120,
      render: (_: any, record: LineItem, index: number) => (
        <InputNumber
          style={{ width: '100%' }}
          min={0}
          precision={2}
          value={record.rate}
          onChange={(v) => recalcLine(index, 'rate', v || 0)}
        />
      ),
    },
    {
      title: 'Amount',
      dataIndex: 'amount',
      width: 120,
      render: (v: number) => (v || 0).toFixed(2),
    },
    {
      title: 'Tax Amount',
      dataIndex: 'taxAmount',
      width: 120,
      render: (_: any, record: LineItem, index: number) => (
        <InputNumber
          style={{ width: '100%' }}
          min={0}
          precision={2}
          value={record.taxAmount}
          onChange={(v) => {
            setLines((prev) => {
              const updated = [...prev];
              updated[index] = { ...updated[index], taxAmount: v || 0 };
              return updated;
            });
          }}
        />
      ),
    },
    {
      title: '',
      width: 50,
      render: (_: any, record: LineItem) => (
        <Button type="text" danger icon={<DeleteOutlined />} onClick={() => removeLine(record.key)} />
      ),
    },
  ];

  return (
    <FormPage
      title={isEdit ? 'Edit Sales Invoice' : 'New Sales Invoice'}
      loading={loading}
      onSubmit={handleSubmit}
      backPath="/sales"
    >
      <Form form={form} layout="vertical">
        <Space style={{ display: 'flex', flexWrap: 'wrap' }} size="large">
          <Form.Item name="customerId" label="Customer" rules={[{ required: true, message: 'Customer is required' }]} style={{ minWidth: 250 }}>
            <Select
              showSearch
              optionFilterProp="children"
              placeholder="Select customer"
              options={customers.map((c) => ({ label: `${c.code} - ${c.name}`, value: c.id }))}
            />
          </Form.Item>
          <Form.Item name="invoiceDate" label="Invoice Date" rules={[{ required: true, message: 'Date is required' }]}>
            <DatePicker style={{ width: 180 }} />
          </Form.Item>
          <Form.Item name="invoiceNo" label="Invoice No">
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
      <Table
        dataSource={lines}
        columns={lineColumns}
        rowKey="key"
        pagination={false}
        size="small"
        scroll={{ x: 'max-content' }}
        summary={() => (
          <Table.Summary fixed>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0} colSpan={3} align="right"><strong>Totals</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={3}><strong>{totals.totalAmount.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={4}><strong>{totals.taxAmount.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={5} />
            </Table.Summary.Row>
            <Table.Summary.Row>
              <Table.Summary.Cell index={0} colSpan={3} align="right"><strong>Grand Total</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={3} colSpan={2}><strong>{totals.grandTotal.toFixed(2)}</strong></Table.Summary.Cell>
              <Table.Summary.Cell index={5} />
            </Table.Summary.Row>
          </Table.Summary>
        )}
      />
    </FormPage>
  );
};

export default SalesFormPage;
