import React from 'react';
import { Card, Form, Input, Button, Space, Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import api from '../../services/api';

const CompanyFormPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/setup/company', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Company Form">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Company Name" name="companyName" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item label="Address" name="address">
          <Input.TextArea rows={2} />
        </Form.Item>
        <Form.Item label="Contact" name="contact">
          <Input />
        </Form.Item>
        <Form.Item label="PAN" name="pan">
          <Input />
        </Form.Item>
        <Form.Item label="Registration No" name="registrationNo">
          <Input />
        </Form.Item>
        <Form.Item label="Logo" name="logo">
          <Upload beforeUpload={() => false} maxCount={1}>
            <Button icon={<UploadOutlined />}>Upload Logo</Button>
          </Upload>
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
export default CompanyFormPage;
