import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { useSelector } from 'react-redux';
import type { RootState } from '../store';
import AppLayout from '../components/layout/AppLayout';

// Auth & Dashboard
import LoginPage from '../pages/auth/LoginPage';
import DashboardPage from '../pages/dashboard/DashboardPage';

// Account - Masters
import LedgerListPage from '../pages/account/LedgerListPage';
import LedgerFormPage from '../pages/account/LedgerFormPage';
import CustomerListPage from '../pages/account/CustomerListPage';
import VendorListPage from '../pages/account/VendorListPage';
import PaymentTermsListPage from '../pages/account/PaymentTermsListPage';
import PaymentModeListPage from '../pages/account/PaymentModeListPage';

// Account - Vouchers
import VoucherListPage from '../pages/account/VoucherListPage';
import VoucherFormPage from '../pages/account/VoucherFormPage';
import ReceiptVoucherPage from '../pages/account/ReceiptVoucherPage';
import PaymentVoucherPage from '../pages/account/PaymentVoucherPage';
import JournalVoucherPage from '../pages/account/JournalVoucherPage';
import ContraVoucherPage from '../pages/account/ContraVoucherPage';
import DebitNotePage from '../pages/account/DebitNotePage';
import CreditNotePage from '../pages/account/CreditNotePage';
import BankReconciliationPage from '../pages/account/BankReconciliationPage';

// Inventory - Masters
import ProductListPage from '../pages/inventory/ProductListPage';
import ProductFormPage from '../pages/inventory/ProductFormPage';
import GodownListPage from '../pages/inventory/GodownListPage';
import StockListPage from '../pages/inventory/StockListPage';
import ProductBrandListPage from '../pages/inventory/ProductBrandListPage';
import UnitListPage from '../pages/inventory/UnitListPage';
import RackListPage from '../pages/inventory/RackListPage';
import IndentListPage from '../pages/inventory/IndentListPage';
import GatePassListPage from '../pages/inventory/GatePassListPage';

// Purchase Cycle
import PurchaseListPage from '../pages/purchase/PurchaseListPage';
import PurchaseFormPage from '../pages/purchase/PurchaseFormPage';
import PurchaseQuotationListPage from '../pages/purchase/PurchaseQuotationListPage';
import PurchaseQuotationFormPage from '../pages/purchase/PurchaseQuotationFormPage';
import PurchaseOrderListPage from '../pages/purchase/PurchaseOrderListPage';
import PurchaseOrderFormPage from '../pages/purchase/PurchaseOrderFormPage';
import ReceiptNoteListPage from '../pages/purchase/ReceiptNoteListPage';
import ReceiptNoteFormPage from '../pages/purchase/ReceiptNoteFormPage';
import PurchaseReturnListPage from '../pages/purchase/PurchaseReturnListPage';

// Sales Cycle
import SalesListPage from '../pages/sales/SalesListPage';
import SalesFormPage from '../pages/sales/SalesFormPage';
import SalesQuotationListPage from '../pages/sales/SalesQuotationListPage';
import SalesQuotationFormPage from '../pages/sales/SalesQuotationFormPage';
import SalesOrderListPage from '../pages/sales/SalesOrderListPage';
import SalesOrderFormPage from '../pages/sales/SalesOrderFormPage';
import SalesDeliveryListPage from '../pages/sales/SalesDeliveryListPage';
import SalesReturnListPage from '../pages/sales/SalesReturnListPage';

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
import BalanceSheetPage from '../pages/reports/BalanceSheetPage';
import ProfitLossPage from '../pages/reports/ProfitLossPage';
import CashFlowPage from '../pages/reports/CashFlowPage';
import StockAgingPage from '../pages/reports/StockAgingPage';
import SalesAnalysisPage from '../pages/reports/SalesAnalysisPage';

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

          {/* Account - Masters */}
          <Route path="account/ledgers" element={<LedgerListPage />} />
          <Route path="account/ledgers/new" element={<LedgerFormPage />} />
          <Route path="account/ledgers/:id" element={<LedgerFormPage />} />
          <Route path="account/customers" element={<CustomerListPage />} />
          <Route path="account/vendors" element={<VendorListPage />} />
          <Route path="account/payment-terms" element={<PaymentTermsListPage />} />
          <Route path="account/payment-modes" element={<PaymentModeListPage />} />

          {/* Account - Vouchers */}
          <Route path="account/vouchers" element={<VoucherListPage />} />
          <Route path="account/vouchers/new" element={<VoucherFormPage />} />
          <Route path="account/vouchers/receipt" element={<ReceiptVoucherPage />} />
          <Route path="account/vouchers/payment" element={<PaymentVoucherPage />} />
          <Route path="account/vouchers/journal" element={<JournalVoucherPage />} />
          <Route path="account/vouchers/contra" element={<ContraVoucherPage />} />
          <Route path="account/vouchers/debit-note" element={<DebitNotePage />} />
          <Route path="account/vouchers/credit-note" element={<CreditNotePage />} />
          <Route path="account/bank-reconciliation" element={<BankReconciliationPage />} />

          {/* Inventory - Masters */}
          <Route path="inventory/products" element={<ProductListPage />} />
          <Route path="inventory/products/new" element={<ProductFormPage />} />
          <Route path="inventory/products/:id" element={<ProductFormPage />} />
          <Route path="inventory/godowns" element={<GodownListPage />} />
          <Route path="inventory/stock" element={<StockListPage />} />
          <Route path="inventory/brands" element={<ProductBrandListPage />} />
          <Route path="inventory/units" element={<UnitListPage />} />
          <Route path="inventory/racks" element={<RackListPage />} />
          <Route path="inventory/indents" element={<IndentListPage />} />
          <Route path="inventory/gate-passes" element={<GatePassListPage />} />

          {/* Purchase Cycle */}
          <Route path="purchase/quotations" element={<PurchaseQuotationListPage />} />
          <Route path="purchase/quotations/new" element={<PurchaseQuotationFormPage />} />
          <Route path="purchase/orders" element={<PurchaseOrderListPage />} />
          <Route path="purchase/orders/new" element={<PurchaseOrderFormPage />} />
          <Route path="purchase/receipt-notes" element={<ReceiptNoteListPage />} />
          <Route path="purchase/receipt-notes/new" element={<ReceiptNoteFormPage />} />
          <Route path="purchase/invoices" element={<PurchaseListPage />} />
          <Route path="purchase/invoices/new" element={<PurchaseFormPage />} />
          <Route path="purchase/returns" element={<PurchaseReturnListPage />} />

          {/* Sales Cycle */}
          <Route path="sales/quotations" element={<SalesQuotationListPage />} />
          <Route path="sales/quotations/new" element={<SalesQuotationFormPage />} />
          <Route path="sales/orders" element={<SalesOrderListPage />} />
          <Route path="sales/orders/new" element={<SalesOrderFormPage />} />
          <Route path="sales/deliveries" element={<SalesDeliveryListPage />} />
          <Route path="sales/invoices" element={<SalesListPage />} />
          <Route path="sales/invoices/new" element={<SalesFormPage />} />
          <Route path="sales/returns" element={<SalesReturnListPage />} />

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
          <Route path="reports/balance-sheet" element={<BalanceSheetPage />} />
          <Route path="reports/profit-loss" element={<ProfitLossPage />} />
          <Route path="reports/cash-flow" element={<CashFlowPage />} />
          <Route path="reports/stock-aging" element={<StockAgingPage />} />
          <Route path="reports/sales-analysis" element={<SalesAnalysisPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouter;
