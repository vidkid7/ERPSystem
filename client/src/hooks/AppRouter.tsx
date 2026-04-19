import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';
import AppLayout from '../components/layout/AppLayout';

// Auth & Dashboard
import LoginPage from '../pages/auth/LoginPage';
import DashboardPage from '../pages/dashboard/DashboardPage';

// Account
import LedgerListPage from '../pages/account/LedgerListPage';
import LedgerFormPage from '../pages/account/LedgerFormPage';
import VoucherListPage from '../pages/account/VoucherListPage';
import VoucherFormPage from '../pages/account/VoucherFormPage';
import CustomerListPage from '../pages/account/CustomerListPage';
import VendorListPage from '../pages/account/VendorListPage';

// Inventory
import ProductListPage from '../pages/inventory/ProductListPage';
import ProductFormPage from '../pages/inventory/ProductFormPage';
import GodownListPage from '../pages/inventory/GodownListPage';
import StockListPage from '../pages/inventory/StockListPage';

// Purchase & Sales
import PurchaseListPage from '../pages/purchase/PurchaseListPage';
import PurchaseFormPage from '../pages/purchase/PurchaseFormPage';
import SalesListPage from '../pages/sales/SalesListPage';
import SalesFormPage from '../pages/sales/SalesFormPage';

// HR
import EmployeeListPage from '../pages/hr/EmployeeListPage';
import EmployeeFormPage from '../pages/hr/EmployeeFormPage';
import AttendanceListPage from '../pages/hr/AttendanceListPage';
import LeaveListPage from '../pages/hr/LeaveListPage';

// HMS
import PatientListPage from '../pages/hms/PatientListPage';
import PatientFormPage from '../pages/hms/PatientFormPage';
import OPDListPage from '../pages/hms/OPDListPage';
import IPDListPage from '../pages/hms/IPDListPage';
import BedListPage from '../pages/hms/BedListPage';

// Service
import ComplaintListPage from '../pages/service/ComplaintListPage';
import ComplaintFormPage from '../pages/service/ComplaintFormPage';
import JobCardListPage from '../pages/service/JobCardListPage';
import AppointmentListPage from '../pages/service/AppointmentListPage';

// Finance
import LoanListPage from '../pages/finance/LoanListPage';
import LoanFormPage from '../pages/finance/LoanFormPage';
import EMISchedulePage from '../pages/finance/EMISchedulePage';

// Task
import TaskListPage from '../pages/task/TaskListPage';
import TaskFormPage from '../pages/task/TaskFormPage';

// Lab
import SampleListPage from '../pages/lab/SampleListPage';
import LabReportListPage from '../pages/lab/LabReportListPage';

// CMS
import SliderListPage from '../pages/cms/SliderListPage';
import BannerListPage from '../pages/cms/BannerListPage';
import NoticeListPage from '../pages/cms/NoticeListPage';

// Support & Loyalty
import SupportTicketListPage from '../pages/support/SupportTicketListPage';
import PointsListPage from '../pages/loyalty/PointsListPage';

// Reports
import TrialBalancePage from '../pages/reports/TrialBalancePage';
import DayBookPage from '../pages/reports/DayBookPage';
import LedgerStatementPage from '../pages/reports/LedgerStatementPage';

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const isAuthenticated = useSelector((s: RootState) => s.auth.isAuthenticated);
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />;
};

const AppRouter: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/" element={<ProtectedRoute><AppLayout /></ProtectedRoute>}>
          <Route index element={<Navigate to="/dashboard" replace />} />
          <Route path="dashboard" element={<DashboardPage />} />

          {/* Account */}
          <Route path="account/ledgers" element={<LedgerListPage />} />
          <Route path="account/ledgers/new" element={<LedgerFormPage />} />
          <Route path="account/ledgers/:id" element={<LedgerFormPage />} />
          <Route path="account/vouchers" element={<VoucherListPage />} />
          <Route path="account/vouchers/new" element={<VoucherFormPage />} />
          <Route path="account/customers" element={<CustomerListPage />} />
          <Route path="account/vendors" element={<VendorListPage />} />

          {/* Inventory */}
          <Route path="inventory/products" element={<ProductListPage />} />
          <Route path="inventory/products/new" element={<ProductFormPage />} />
          <Route path="inventory/products/:id" element={<ProductFormPage />} />
          <Route path="inventory/godowns" element={<GodownListPage />} />
          <Route path="inventory/stock" element={<StockListPage />} />

          {/* Purchase & Sales */}
          <Route path="purchase/invoices" element={<PurchaseListPage />} />
          <Route path="purchase/invoices/new" element={<PurchaseFormPage />} />
          <Route path="sales/invoices" element={<SalesListPage />} />
          <Route path="sales/invoices/new" element={<SalesFormPage />} />

          {/* HR */}
          <Route path="hr/employees" element={<EmployeeListPage />} />
          <Route path="hr/employees/new" element={<EmployeeFormPage />} />
          <Route path="hr/employees/:id" element={<EmployeeFormPage />} />
          <Route path="hr/attendance" element={<AttendanceListPage />} />
          <Route path="hr/leaves" element={<LeaveListPage />} />

          {/* HMS */}
          <Route path="hms/patients" element={<PatientListPage />} />
          <Route path="hms/patients/new" element={<PatientFormPage />} />
          <Route path="hms/patients/:id" element={<PatientFormPage />} />
          <Route path="hms/opd" element={<OPDListPage />} />
          <Route path="hms/ipd" element={<IPDListPage />} />
          <Route path="hms/beds" element={<BedListPage />} />

          {/* Service */}
          <Route path="service/complaints" element={<ComplaintListPage />} />
          <Route path="service/complaints/new" element={<ComplaintFormPage />} />
          <Route path="service/jobcards" element={<JobCardListPage />} />
          <Route path="service/appointments" element={<AppointmentListPage />} />

          {/* Finance */}
          <Route path="finance/loans" element={<LoanListPage />} />
          <Route path="finance/loans/new" element={<LoanFormPage />} />
          <Route path="finance/loans/:id/emi" element={<EMISchedulePage />} />

          {/* Task */}
          <Route path="tasks" element={<TaskListPage />} />
          <Route path="tasks/new" element={<TaskFormPage />} />
          <Route path="tasks/:id" element={<TaskFormPage />} />

          {/* Lab */}
          <Route path="lab/samples" element={<SampleListPage />} />
          <Route path="lab/reports" element={<LabReportListPage />} />

          {/* CMS */}
          <Route path="cms/sliders" element={<SliderListPage />} />
          <Route path="cms/banners" element={<BannerListPage />} />
          <Route path="cms/notices" element={<NoticeListPage />} />

          {/* Support & Loyalty */}
          <Route path="support/tickets" element={<SupportTicketListPage />} />
          <Route path="loyalty/points" element={<PointsListPage />} />

          {/* Reports */}
          <Route path="reports/trial-balance" element={<TrialBalancePage />} />
          <Route path="reports/day-book" element={<DayBookPage />} />
          <Route path="reports/ledger-statement" element={<LedgerStatementPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouter;
