import React from 'react';
import { Card, Form, Input, Checkbox, Button, Space } from 'antd';
import api from '../../services/api';

const PERMISSIONS = ['View', 'Create', 'Edit', 'Delete', 'Print', 'Export', 'Approve'];

const UserGroupFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/setup/user-group', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="User Group Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Group Name" name="groupName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Code" name="code" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Permissions" name="permissions">
          <Checkbox.Group options={PERMISSIONS} />
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
export default UserGroupFormPage;
