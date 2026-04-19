import React from 'react';
import { Card, Form, Input, Select, DatePicker, Button, Space, Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons';
import api from '../../services/api';

const LeaveApplicationPage: React.FC = () => {
  const [form] = Form.useForm();

  const handleSubmit = async (values: any) => {
    try {
      await api.post('/hr/leave-application', values);
      form.resetFields();
    } catch { /* handled by interceptor */ }
  };

  return (
    <Card title="Leave Application">
      <Form form={form} layout="vertical" onFinish={handleSubmit} style={{ maxWidth: 600 }}>
        <Form.Item label="Employee" name="employee" rules={[{ required: true }]}>
          <Select placeholder="Select employee" />
        </Form.Item>
        <Form.Item label="Leave Type" name="leaveType" rules={[{ required: true }]}>
          <Select placeholder="Select leave type" />
        </Form.Item>
        <Form.Item label="From Date" name="fromDate" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="To Date" name="toDate" rules={[{ required: true }]}>
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item label="Reason" name="reason">
          <Input.TextArea rows={3} />
        </Form.Item>
        <Form.Item label="Attachment" name="attachment">
          <Upload beforeUpload={() => false} maxCount={1}>
            <Button icon={<UploadOutlined />}>Upload</Button>
          </Upload>
        </Form.Item>
        <Form.Item>
          <Space>
            <Button type="primary" htmlType="submit">Submit</Button>
            <Button onClick={() => form.resetFields()}>Reset</Button>
          </Space>
        </Form.Item>
      </Form>
    </Card>
  );
};
export default LeaveApplicationPage;
