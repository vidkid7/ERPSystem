import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';
import AppLayout from '../components/layout/AppLayout';
import LoginPage from '../pages/auth/LoginPage';
import DashboardPage from '../pages/dashboard/DashboardPage';
import LedgerListPage from '../pages/account/LedgerListPage';
import ProductListPage from '../pages/inventory/ProductListPage';
import PurchaseListPage from '../pages/purchase/PurchaseListPage';
import SalesListPage from '../pages/sales/SalesListPage';
import EmployeeListPage from '../pages/hr/EmployeeListPage';
import PatientListPage from '../pages/hms/PatientListPage';
import ComplaintListPage from '../pages/service/ComplaintListPage';
import TrialBalancePage from '../pages/reports/TrialBalancePage';

const ProtectedRoute: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const isAuthenticated = useSelector((s: RootState) => s.auth.isAuthenticated);
  return isAuthenticated ? <>{children}</> : <Navigate to="/login" replace />;
};

const Placeholder: React.FC<{ title: string }> = ({ title }) => (
  <div style={{ padding: 24 }}><h2>{title}</h2><p>This page is under development.</p></div>
);

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
          <Route path="account/vouchers" element={<Placeholder title="Vouchers" />} />
          <Route path="account/customers" element={<Placeholder title="Customers" />} />
          <Route path="account/vendors" element={<Placeholder title="Vendors" />} />

          {/* Inventory */}
          <Route path="inventory/products" element={<ProductListPage />} />
          <Route path="inventory/godowns" element={<Placeholder title="Godowns" />} />
          <Route path="inventory/stock" element={<Placeholder title="Stock" />} />

          {/* Purchase & Sales */}
          <Route path="purchase/invoices" element={<PurchaseListPage />} />
          <Route path="sales/invoices" element={<SalesListPage />} />

          {/* HR */}
          <Route path="hr/employees" element={<EmployeeListPage />} />
          <Route path="hr/attendance" element={<Placeholder title="Attendance" />} />
          <Route path="hr/leaves" element={<Placeholder title="Leave Management" />} />

          {/* HMS */}
          <Route path="hms/patients" element={<PatientListPage />} />
          <Route path="hms/opd" element={<Placeholder title="OPD Tickets" />} />
          <Route path="hms/ipd" element={<Placeholder title="IPD Admissions" />} />
          <Route path="hms/beds" element={<Placeholder title="Bed Management" />} />

          {/* Service */}
          <Route path="service/complaints" element={<ComplaintListPage />} />
          <Route path="service/jobcards" element={<Placeholder title="Job Cards" />} />
          <Route path="service/appointments" element={<Placeholder title="Service Appointments" />} />

          {/* Reports */}
          <Route path="reports/trial-balance" element={<TrialBalancePage />} />
          <Route path="reports/day-book" element={<Placeholder title="Day Book" />} />
          <Route path="reports/ledger-statement" element={<Placeholder title="Ledger Statement" />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouter;
