import React from 'react';
import { Card, Form, Switch, Input, Button, Space } from 'antd';

const SENTCustomPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = (values: Record<string, unknown>) => {
    console.log(values);
  };

  return (
    <Card title="SENT Custom">
      <Form form={form} layout="vertical" onFinish={handleSave} style={{ maxWidth: 700 }}>
        <Form.Item name="enabled" label="Enabled" valuePropName="checked">
          <Switch />
        </Form.Item>
        <Form.Item name="description" label="Description">
          <Input.TextArea rows={3} />
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

export default SENTCustomPage;
