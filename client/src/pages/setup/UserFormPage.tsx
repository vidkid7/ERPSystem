import React from 'react';
import { Card, Form, Input, Select, Switch, Button, message } from 'antd';

const { Option } = Select;

const UserFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    try {
      await form.validateFields();
      const values = form.getFieldsValue();
      console.log('Save user:', values);
      message.success('User saved successfully');
    } catch {
      message.error('Please fill in all required fields');
    }
  };

  return (
    <Card title="User Form">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="username" label="Username" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="fullName" label="Full Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="email" label="Email" rules={[{ required: true, type: 'email' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="password" label="Password" rules={[{ required: true, min: 6 }]}>
          <Input.Password />
        </Form.Item>
        <Form.Item name="role" label="Role" rules={[{ required: true }]}>
          <Select placeholder="Select role">
            <Option value="admin">Admin</Option>
            <Option value="manager">Manager</Option>
            <Option value="user">User</Option>
            <Option value="viewer">Viewer</Option>
          </Select>
        </Form.Item>
        <Form.Item name="userGroup" label="User Group">
          <Select placeholder="Select user group" allowClear />
        </Form.Item>
        <Form.Item name="isActive" label="Active" valuePropName="checked" initialValue={true}>
          <Switch />
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default UserFormPage;
