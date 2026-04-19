import React, { useEffect, useState } from 'react';
import { Form, Input, InputNumber, DatePicker, message } from 'antd';
import { useNavigate, useParams } from 'react-router-dom';
import dayjs from 'dayjs';
import FormPage from '../../components/common/FormPage';
import api from '../../services/api';

const { TextArea } = Input;

const LoanFormPage: React.FC = () => {
  const [form] = Form.useForm();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = !!id;

  useEffect(() => {
    if (isEdit) {
      api.get(`/finance/loan/${id}`).then((res) => {
        const d = res.data.data;
        form.setFieldsValue({
          ...d,
          disbursementDate: d.disbursementDate ? dayjs(d.disbursementDate) : undefined,
        });
      });
    }
  }, [id]);

  const handleSubmit = async () => {
    try {
      const values = await form.validateFields();
      setLoading(true);
      const payload = {
        ...values,
        disbursementDate: values.disbursementDate?.toISOString(),
      };
      if (isEdit) {
        await api.post(`/finance/loan/${id}`, payload);
        message.success('Loan updated successfully');
      } else {
        await api.post('/finance/loan', payload);
        message.success('Loan created successfully');
      }
      navigate('/finance/loans');
    } catch {
      // validation error
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormPage title={isEdit ? 'Edit Loan' : 'New Loan'} loading={loading} onSubmit={handleSubmit} backPath="/finance/loans">
      <Form form={form} layout="vertical">
        <Form.Item name="customerId" label="Customer ID" rules={[{ required: true, message: 'Please enter customer ID' }]}>
          <InputNumber style={{ width: '100%' }} min={1} />
        </Form.Item>
        <Form.Item name="loanAmount" label="Loan Amount" rules={[{ required: true, message: 'Please enter loan amount' }]}>
          <InputNumber style={{ width: '100%' }} min={0} precision={2} />
        </Form.Item>
        <Form.Item name="interestRate" label="Interest Rate (%)" rules={[{ required: true, message: 'Please enter interest rate' }]}>
          <InputNumber style={{ width: '100%' }} min={0} max={100} precision={2} />
        </Form.Item>
        <Form.Item name="tenureMonths" label="Tenure (Months)" rules={[{ required: true, message: 'Please enter tenure' }]}>
          <InputNumber style={{ width: '100%' }} min={1} />
        </Form.Item>
        <Form.Item name="disbursementDate" label="Disbursement Date">
          <DatePicker style={{ width: '100%' }} />
        </Form.Item>
        <Form.Item name="securityDetails" label="Security Details">
          <TextArea rows={3} />
        </Form.Item>
      </Form>
    </FormPage>
  );
};

export default LoanFormPage;
