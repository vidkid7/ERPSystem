import React from 'react';
import { Card, Form, Input, Button, Select, InputNumber, Space } from 'antd';

const PurchaseDebitNoteListPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = (values: Record<string, unknown>) => {
    console.log(values);
  };

  return (
    <Card title="Purchase Debit Note">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 700 }}>
        <Form.Item name="voucherNo" label="Voucher No">
          <Input />
        </Form.Item>
        <Form.Item name="date" label="Date" rules={[{ required: true }]}>
          <Input type="date" />
        </Form.Item>
        <Form.Item name="narration" label="Narration">
          <Input.TextArea rows={2} />
        </Form.Item>
        <Form.Item>
          <Space>
            <Button type="primary" htmlType="submit">Save</Button>
            <Button onClick={() => form.resetFields()}>Reset</Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  );
};

export default PurchaseDebitNoteListPage;
