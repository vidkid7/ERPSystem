import React from 'react';
import { Card, Form, Input, Button, message, Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons';

const CompanyDetailPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSave = async () => {
    const values = form.getFieldsValue();
    console.log('Save company:', values);
    message.success('Company details saved successfully');
  };

  return (
    <Card title="Company Details">
      <Form form={form} layout="vertical" style={{ maxWidth: 600 }}>
        <Form.Item name="companyName" label="Company Name" rules={[{ required: true }]}>
          <Input />
        </Form.Item>
        <Form.Item name="address" label="Address">
          <Input.TextArea rows={3} />
        </Form.Item>
        <Form.Item name="phone" label="Phone">
          <Input />
        </Form.Item>
        <Form.Item name="email" label="Email" rules={[{ type: 'email' }]}>
          <Input />
        </Form.Item>
        <Form.Item name="panNo" label="PAN No">
          <Input />
        </Form.Item>
        <Form.Item name="vatNo" label="VAT No">
          <Input />
        </Form.Item>
        <Form.Item name="logo" label="Company Logo">
          <Upload beforeUpload={() => false} maxCount={1} listType="picture">
            <Button icon={<UploadOutlined />}>Upload Logo</Button>
          </Upload>
        </Form.Item>
        <Button type="primary" onClick={handleSave}>Save</Button>
      </Form>
    </Card>
  );
};

export default CompanyDetailPage;
