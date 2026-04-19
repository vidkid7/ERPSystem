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
import LedgerGroupListPage from '../pages/account/LedgerGroupListPage';
import CostCenterListPage from '../pages/account/CostCenterListPage';
import SalesManListPage from '../pages/account/SalesManListPage';
import NarrationMasterListPage from '../pages/account/NarrationMasterListPage';
import ProjectListPage from '../pages/account/ProjectListPage';
import AreaMasterListPage from '../pages/account/AreaMasterListPage';
import CostCategoryListPage from '../pages/account/CostCategoryListPage';
import VoucherModeListPage from '../pages/account/VoucherModeListPage';
import FreightTypeListPage from '../pages/account/FreightTypeListPage';
import DepartmentListPage from '../pages/account/DepartmentListPage';
import LedgerCategoryListPage from '../pages/account/LedgerCategoryListPage';
import LedgerChannelListPage from '../pages/account/LedgerChannelListPage';
import DebtorTypeListPage from '../pages/account/DebtorTypeListPage';
import DebtorRouteListPage from '../pages/account/DebtorRouteListPage';

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
import ProductGroupListPage from '../pages/inventory/ProductGroupListPage';
import ProductCategoryListPage from '../pages/inventory/ProductCategoryListPage';
import ProductTypeListPage from '../pages/inventory/ProductTypeListPage';
import ProductCompanyListPage from '../pages/inventory/ProductCompanyListPage';
import ProductDivisionListPage from '../pages/inventory/ProductDivisionListPage';
import ProductColorListPage from '../pages/inventory/ProductColorListPage';
import ProductFlavourListPage from '../pages/inventory/ProductFlavourListPage';
import ProductSchemeListPage from '../pages/inventory/ProductSchemeListPage';
import SalesPriceListPage from '../pages/inventory/SalesPriceListPage';
import PartyWiseRateListPage from '../pages/inventory/PartyWiseRateListPage';
import DeliveryThroughListPage from '../pages/inventory/DeliveryThroughListPage';
import FixedUnitListPage from '../pages/inventory/FixedUnitListPage';

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
import ReligionListPage from '../pages/hr/ReligionListPage';
import GrievanceTypeListPage from '../pages/hr/GrievanceTypeListPage';
import GrievanceFormListPage from '../pages/hr/GrievanceFormListPage';

// HMS
import PatientListPage from '../pages/hms/PatientListPage';
import PatientFormPage from '../pages/hms/PatientFormPage';
import OPDListPage from '../pages/hms/OPDListPage';
import IPDListPage from '../pages/hms/IPDListPage';
import BedListPage from '../pages/hms/BedListPage';
import DoctorListPage from '../pages/hms/DoctorListPage';
import HmsDepartmentListPage from '../pages/hms/HmsDepartmentListPage';
import HmsDesignationListPage from '../pages/hms/HmsDesignationListPage';
import OPDTicketTypeListPage from '../pages/hms/OPDTicketTypeListPage';
import OPDServiceTypeListPage from '../pages/hms/OPDServiceTypeListPage';
import AdmissionTypeListPage from '../pages/hms/AdmissionTypeListPage';
import DischargeTypeListPage from '../pages/hms/DischargeTypeListPage';
import DiscountTypeListPage from '../pages/hms/DiscountTypeListPage';
import DepositTypeListPage from '../pages/hms/DepositTypeListPage';
import BillingTypeListPage from '../pages/hms/BillingTypeListPage';
import DiagnosisListPage from '../pages/hms/DiagnosisListPage';
import VitalListPage from '../pages/hms/VitalListPage';
import FloorListPage from '../pages/hms/FloorListPage';
import RoomListPage from '../pages/hms/RoomListPage';
import WardListPage from '../pages/hms/WardListPage';

// Lab
import SampleListPage from '../pages/lab/SampleListPage';
import LabReportListPage from '../pages/lab/LabReportListPage';
import LabCategoryListPage from '../pages/lab/LabCategoryListPage';
import LabTestListPage from '../pages/lab/LabTestListPage';
import LabTechnicianListPage from '../pages/lab/LabTechnicianListPage';
import SpecimenListPage from '../pages/lab/SpecimenListPage';
import ContainerListPage from '../pages/lab/ContainerListPage';
import LabMethodListPage from '../pages/lab/LabMethodListPage';
import LabLookupListPage from '../pages/lab/LabLookupListPage';
import LabPackageListPage from '../pages/lab/LabPackageListPage';

// Service
import ComplaintListPage from '../pages/service/ComplaintListPage';
import ComplaintFormPage from '../pages/service/ComplaintFormPage';
import JobCardListPage from '../pages/service/JobCardListPage';
import AppointmentListPage from '../pages/service/AppointmentListPage';
import TicketForListPage from '../pages/service/TicketForListPage';
import NatureListPage from '../pages/service/NatureListPage';
import SourceListPage from '../pages/service/SourceListPage';
import JobTypeListPage from '../pages/service/JobTypeListPage';
import JobCardTypeListPage from '../pages/service/JobCardTypeListPage';
import JobServiceTypeListPage from '../pages/service/JobServiceTypeListPage';
import InspectionTypeGroupListPage from '../pages/service/InspectionTypeGroupListPage';
import DeviceTypeListPage from '../pages/service/DeviceTypeListPage';
import DeviceModelListPage from '../pages/service/DeviceModelListPage';

// Finance
import LoanListPage from '../pages/finance/LoanListPage';
import LoanFormPage from '../pages/finance/LoanFormPage';
import EMISchedulePage from '../pages/finance/EMISchedulePage';

// Assets
import AssetGroupListPage from '../pages/assets/AssetGroupListPage';
import AssetTypeListPage from '../pages/assets/AssetTypeListPage';
import AssetModelListPage from '../pages/assets/AssetModelListPage';
import AssetCategoryListPage from '../pages/assets/AssetCategoryListPage';
import AssetListPage from '../pages/assets/AssetListPage';
import AssetFormPage from '../pages/assets/AssetFormPage';
import AssetTransactionListPage from '../pages/assets/AssetTransactionListPage';
import AssetInwardPage from '../pages/assets/AssetInwardPage';
import AssetIssuePage from '../pages/assets/AssetIssuePage';
import AssetTransferPage from '../pages/assets/AssetTransferPage';
import AssetReturnPage from '../pages/assets/AssetReturnPage';
import AssetDamagePage from '../pages/assets/AssetDamagePage';
import AssetDisposalPage from '../pages/assets/AssetDisposalPage';
import AssetRequestListPage from '../pages/assets/AssetRequestListPage';
import AssetStockReportPage from '../pages/assets/AssetStockReportPage';

// Manufacturing
import BOMListPage from '../pages/manufacturing/BOMListPage';
import ProductionOrderListPage from '../pages/manufacturing/ProductionOrderListPage';
import StockJournalListPage from '../pages/manufacturing/StockJournalListPage';
import ConsumptionListPage from '../pages/manufacturing/ConsumptionListPage';
import DispatchOrderListPage from '../pages/manufacturing/DispatchOrderListPage';

// Task
import TaskListPage from '../pages/task/TaskListPage';
import TaskFormPage from '../pages/task/TaskFormPage';

// CMS
import SliderListPage from '../pages/cms/SliderListPage';
import BannerListPage from '../pages/cms/BannerListPage';
import NoticeListPage from '../pages/cms/NoticeListPage';
import GalleryListPage from '../pages/cms/GalleryListPage';
import VideoListPage from '../pages/cms/VideoListPage';
import IntroductionPage from '../pages/cms/IntroductionPage';
import EventTypeListPage from '../pages/cms/EventTypeListPage';
import EventListPage from '../pages/cms/EventListPage';
import ProductDisplayListPage from '../pages/cms/ProductDisplayListPage';

// Support & Loyalty
import SupportTicketListPage from '../pages/support/SupportTicketListPage';
import PointsListPage from '../pages/loyalty/PointsListPage';

// Setup
import UserGroupListPage from '../pages/setup/UserGroupListPage';
import UserListPage from '../pages/setup/UserListPage';
import UserFormPage from '../pages/setup/UserFormPage';
import IPRestrictionListPage from '../pages/setup/IPRestrictionListPage';
import BranchListPage from '../pages/setup/BranchListPage';
import SubBranchListPage from '../pages/setup/SubBranchListPage';
import ModuleListPage from '../pages/setup/ModuleListPage';
import CreditRulesListPage from '../pages/setup/CreditRulesListPage';
import GeneralConfigPage from '../pages/setup/GeneralConfigPage';
import CompanyDetailPage from '../pages/setup/CompanyDetailPage';
import IRDDetailsPage from '../pages/setup/IRDDetailsPage';
import EmailSetupPage from '../pages/setup/EmailSetupPage';
import OneSignalSetupPage from '../pages/setup/OneSignalSetupPage';
import FonepaySetupPage from '../pages/setup/FonepaySetupPage';
import PaymentGatewayPage from '../pages/setup/PaymentGatewayPage';
import DocumentTypePage from '../pages/setup/DocumentTypePage';
import EntityNumberingPage from '../pages/setup/EntityNumberingPage';

// Logs
import UserWiseLogPage from '../pages/logs/UserWiseLogPage';
import LoginLogPage from '../pages/logs/LoginLogPage';
import WebApiLogPage from '../pages/logs/WebApiLogPage';
import IRDApiLogPage from '../pages/logs/IRDApiLogPage';
import SMSApiLogPage from '../pages/logs/SMSApiLogPage';
import NotificationLogPage from '../pages/logs/NotificationLogPage';
import EmailLogPage from '../pages/logs/EmailLogPage';
import JobLogPage from '../pages/logs/JobLogPage';
import SqlErrorLogPage from '../pages/logs/SqlErrorLogPage';

// Reports
import TrialBalancePage from '../pages/reports/TrialBalancePage';
import DayBookPage from '../pages/reports/DayBookPage';
import LedgerStatementPage from '../pages/reports/LedgerStatementPage';
import BalanceSheetPage from '../pages/reports/BalanceSheetPage';
import ProfitLossPage from '../pages/reports/ProfitLossPage';
import CashFlowPage from '../pages/reports/CashFlowPage';
import StockAgingPage from '../pages/reports/StockAgingPage';
import SalesAnalysisPage from '../pages/reports/SalesAnalysisPage';
import LedgerGroupReportPage from '../pages/reports/LedgerGroupReportPage';
import LedgerReportPage from '../pages/reports/LedgerReportPage';
import LedgerAnalysisPage from '../pages/reports/LedgerAnalysisPage';
import CostCenterReportPage from '../pages/reports/CostCenterReportPage';
import BGDetailsReportPage from '../pages/reports/BGDetailsReportPage';
import PDCReportPage from '../pages/reports/PDCReportPage';
import ODCReportPage from '../pages/reports/ODCReportPage';
import BillsReceivableReportPage from '../pages/reports/BillsReceivableReportPage';
import BillsPayableReportPage from '../pages/reports/BillsPayableReportPage';
import LedgerOpeningReportPage from '../pages/reports/LedgerOpeningReportPage';
import VATSummaryReportPage from '../pages/reports/VATSummaryReportPage';
import TDSSummaryReportPage from '../pages/reports/TDSSummaryReportPage';
import CancelDayBookPage from '../pages/reports/CancelDayBookPage';
import CashBankBookPage from '../pages/reports/CashBankBookPage';
import PatientOutstandingPage from '../pages/reports/PatientOutstandingPage';
import StockSummaryPage from '../pages/reports/StockSummaryPage';
import ProductVoucherPage from '../pages/reports/ProductVoucherPage';
import ProductAgeingPage from '../pages/reports/ProductAgeingPage';
import NearExpiryPage from '../pages/reports/NearExpiryPage';
import StockTransferReportPage from '../pages/reports/StockTransferReportPage';
import PendingPOSummaryPage from '../pages/reports/PendingPOSummaryPage';
import SalesAllotmentReportPage from '../pages/reports/SalesAllotmentReportPage';
import PurchaseAnalysisProductPage from '../pages/reports/PurchaseAnalysisProductPage';
import SalesAnalysisProductPage from '../pages/reports/SalesAnalysisProductPage';
import ProductMonthlySummaryPage from '../pages/reports/ProductMonthlySummaryPage';
import EmployeeSummaryReportPage from '../pages/reports/EmployeeSummaryReportPage';
import ServiceTenureReportPage from '../pages/reports/ServiceTenureReportPage';
import GrievanceListReportPage from '../pages/reports/GrievanceListReportPage';
import AttendanceAppealsPage from '../pages/reports/AttendanceAppealsPage';
import LoanReportPage from '../pages/reports/LoanReportPage';
import LoanDetailsPage from '../pages/reports/LoanDetailsPage';
import LoanMonthlyReportPage from '../pages/reports/LoanMonthlyReportPage';
import ServiceReminderPage from '../pages/reports/ServiceReminderPage';
import FourthCallLogPage from '../pages/reports/FourthCallLogPage';
import ServiceDashboardPage from '../pages/reports/ServiceDashboardPage';

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
          <Route path="account/ledger-groups" element={<LedgerGroupListPage />} />
          <Route path="account/ledgers" element={<LedgerListPage />} />
          <Route path="account/ledgers/new" element={<LedgerFormPage />} />
          <Route path="account/ledgers/:id" element={<LedgerFormPage />} />
          <Route path="account/customers" element={<CustomerListPage />} />
          <Route path="account/vendors" element={<VendorListPage />} />
          <Route path="account/payment-terms" element={<PaymentTermsListPage />} />
          <Route path="account/payment-modes" element={<PaymentModeListPage />} />
          <Route path="account/cost-centers" element={<CostCenterListPage />} />
          <Route path="account/cost-categories" element={<CostCategoryListPage />} />
          <Route path="account/salesmen" element={<SalesManListPage />} />
          <Route path="account/narrations" element={<NarrationMasterListPage />} />
          <Route path="account/projects" element={<ProjectListPage />} />
          <Route path="account/areas" element={<AreaMasterListPage />} />
          <Route path="account/voucher-modes" element={<VoucherModeListPage />} />
          <Route path="account/freight-types" element={<FreightTypeListPage />} />
          <Route path="account/departments" element={<DepartmentListPage />} />
          <Route path="account/ledger-categories" element={<LedgerCategoryListPage />} />
          <Route path="account/ledger-channels" element={<LedgerChannelListPage />} />
          <Route path="account/debtor-types" element={<DebtorTypeListPage />} />
          <Route path="account/debtor-routes" element={<DebtorRouteListPage />} />

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
          <Route path="inventory/product-groups" element={<ProductGroupListPage />} />
          <Route path="inventory/product-categories" element={<ProductCategoryListPage />} />
          <Route path="inventory/product-types" element={<ProductTypeListPage />} />
          <Route path="inventory/product-companies" element={<ProductCompanyListPage />} />
          <Route path="inventory/product-divisions" element={<ProductDivisionListPage />} />
          <Route path="inventory/product-colors" element={<ProductColorListPage />} />
          <Route path="inventory/product-flavours" element={<ProductFlavourListPage />} />
          <Route path="inventory/product-schemes" element={<ProductSchemeListPage />} />
          <Route path="inventory/sales-prices" element={<SalesPriceListPage />} />
          <Route path="inventory/party-wise-rates" element={<PartyWiseRateListPage />} />
          <Route path="inventory/delivery-through" element={<DeliveryThroughListPage />} />
          <Route path="inventory/fixed-units" element={<FixedUnitListPage />} />
          <Route path="inventory/products" element={<ProductListPage />} />
          <Route path="inventory/products/new" element={<ProductFormPage />} />
          <Route path="inventory/products/:id" element={<ProductFormPage />} />
          <Route path="inventory/brands" element={<ProductBrandListPage />} />
          <Route path="inventory/units" element={<UnitListPage />} />
          <Route path="inventory/godowns" element={<GodownListPage />} />
          <Route path="inventory/stock" element={<StockListPage />} />
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
          <Route path="hr/religions" element={<ReligionListPage />} />
          <Route path="hr/grievance-types" element={<GrievanceTypeListPage />} />
          <Route path="hr/grievances" element={<GrievanceFormListPage />} />

          {/* HMS */}
          <Route path="hms/doctors" element={<DoctorListPage />} />
          <Route path="hms/departments" element={<HmsDepartmentListPage />} />
          <Route path="hms/designations" element={<HmsDesignationListPage />} />
          <Route path="hms/opd-ticket-types" element={<OPDTicketTypeListPage />} />
          <Route path="hms/opd-service-types" element={<OPDServiceTypeListPage />} />
          <Route path="hms/admission-types" element={<AdmissionTypeListPage />} />
          <Route path="hms/discharge-types" element={<DischargeTypeListPage />} />
          <Route path="hms/discount-types" element={<DiscountTypeListPage />} />
          <Route path="hms/deposit-types" element={<DepositTypeListPage />} />
          <Route path="hms/billing-types" element={<BillingTypeListPage />} />
          <Route path="hms/diagnoses" element={<DiagnosisListPage />} />
          <Route path="hms/vitals" element={<VitalListPage />} />
          <Route path="hms/floors" element={<FloorListPage />} />
          <Route path="hms/rooms" element={<RoomListPage />} />
          <Route path="hms/wards" element={<WardListPage />} />
          <Route path="hms/patients" element={<PatientListPage />} />
          <Route path="hms/patients/new" element={<PatientFormPage />} />
          <Route path="hms/patients/:id" element={<PatientFormPage />} />
          <Route path="hms/opd" element={<OPDListPage />} />
          <Route path="hms/ipd" element={<IPDListPage />} />
          <Route path="hms/beds" element={<BedListPage />} />

          {/* Lab */}
          <Route path="lab/categories" element={<LabCategoryListPage />} />
          <Route path="lab/tests" element={<LabTestListPage />} />
          <Route path="lab/technicians" element={<LabTechnicianListPage />} />
          <Route path="lab/specimens" element={<SpecimenListPage />} />
          <Route path="lab/containers" element={<ContainerListPage />} />
          <Route path="lab/methods" element={<LabMethodListPage />} />
          <Route path="lab/lookups" element={<LabLookupListPage />} />
          <Route path="lab/packages" element={<LabPackageListPage />} />
          <Route path="lab/samples" element={<SampleListPage />} />
          <Route path="lab/reports" element={<LabReportListPage />} />

          {/* Service */}
          <Route path="service/ticket-for" element={<TicketForListPage />} />
          <Route path="service/natures" element={<NatureListPage />} />
          <Route path="service/sources" element={<SourceListPage />} />
          <Route path="service/job-types" element={<JobTypeListPage />} />
          <Route path="service/jobcard-types" element={<JobCardTypeListPage />} />
          <Route path="service/job-service-types" element={<JobServiceTypeListPage />} />
          <Route path="service/inspection-type-groups" element={<InspectionTypeGroupListPage />} />
          <Route path="service/device-types" element={<DeviceTypeListPage />} />
          <Route path="service/device-models" element={<DeviceModelListPage />} />
          <Route path="service/complaints" element={<ComplaintListPage />} />
          <Route path="service/complaints/new" element={<ComplaintFormPage />} />
          <Route path="service/jobcards" element={<JobCardListPage />} />
          <Route path="service/appointments" element={<AppointmentListPage />} />

          {/* Finance */}
          <Route path="finance/loans" element={<LoanListPage />} />
          <Route path="finance/loans/new" element={<LoanFormPage />} />
          <Route path="finance/loans/:id/emi" element={<EMISchedulePage />} />

          {/* Assets */}
          <Route path="assets/groups" element={<AssetGroupListPage />} />
          <Route path="assets/types" element={<AssetTypeListPage />} />
          <Route path="assets/models" element={<AssetModelListPage />} />
          <Route path="assets/categories" element={<AssetCategoryListPage />} />
          <Route path="assets/list" element={<AssetListPage />} />
          <Route path="assets/list/new" element={<AssetFormPage />} />
          <Route path="assets/list/:id" element={<AssetFormPage />} />
          <Route path="assets/transactions" element={<AssetTransactionListPage />} />
          <Route path="assets/inward" element={<AssetInwardPage />} />
          <Route path="assets/issue" element={<AssetIssuePage />} />
          <Route path="assets/transfer" element={<AssetTransferPage />} />
          <Route path="assets/return" element={<AssetReturnPage />} />
          <Route path="assets/damage" element={<AssetDamagePage />} />
          <Route path="assets/disposal" element={<AssetDisposalPage />} />
          <Route path="assets/requests" element={<AssetRequestListPage />} />
          <Route path="assets/stock-report" element={<AssetStockReportPage />} />

          {/* Manufacturing */}
          <Route path="manufacturing/bom" element={<BOMListPage />} />
          <Route path="manufacturing/production-orders" element={<ProductionOrderListPage />} />
          <Route path="manufacturing/stock-journals" element={<StockJournalListPage />} />
          <Route path="manufacturing/consumption" element={<ConsumptionListPage />} />
          <Route path="manufacturing/dispatch-orders" element={<DispatchOrderListPage />} />

          {/* Task */}
          <Route path="tasks" element={<TaskListPage />} />
          <Route path="tasks/new" element={<TaskFormPage />} />
          <Route path="tasks/:id" element={<TaskFormPage />} />

          {/* CMS */}
          <Route path="cms/sliders" element={<SliderListPage />} />
          <Route path="cms/banners" element={<BannerListPage />} />
          <Route path="cms/notices" element={<NoticeListPage />} />
          <Route path="cms/gallery" element={<GalleryListPage />} />
          <Route path="cms/videos" element={<VideoListPage />} />
          <Route path="cms/introduction" element={<IntroductionPage />} />
          <Route path="cms/event-types" element={<EventTypeListPage />} />
          <Route path="cms/events" element={<EventListPage />} />
          <Route path="cms/product-display" element={<ProductDisplayListPage />} />

          {/* Support & Loyalty */}
          <Route path="support/tickets" element={<SupportTicketListPage />} />
          <Route path="loyalty/points" element={<PointsListPage />} />

          {/* Setup */}
          <Route path="setup/company-detail" element={<CompanyDetailPage />} />
          <Route path="setup/general-config" element={<GeneralConfigPage />} />
          <Route path="setup/branches" element={<BranchListPage />} />
          <Route path="setup/sub-branches" element={<SubBranchListPage />} />
          <Route path="setup/users" element={<UserListPage />} />
          <Route path="setup/users/new" element={<UserFormPage />} />
          <Route path="setup/users/:id" element={<UserFormPage />} />
          <Route path="setup/user-groups" element={<UserGroupListPage />} />
          <Route path="setup/ip-restrictions" element={<IPRestrictionListPage />} />
          <Route path="setup/modules" element={<ModuleListPage />} />
          <Route path="setup/credit-rules" element={<CreditRulesListPage />} />
          <Route path="setup/document-types" element={<DocumentTypePage />} />
          <Route path="setup/entity-numbering" element={<EntityNumberingPage />} />
          <Route path="setup/ird-details" element={<IRDDetailsPage />} />
          <Route path="setup/email-setup" element={<EmailSetupPage />} />
          <Route path="setup/onesignal" element={<OneSignalSetupPage />} />
          <Route path="setup/fonepay" element={<FonepaySetupPage />} />
          <Route path="setup/payment-gateway" element={<PaymentGatewayPage />} />

          {/* Logs */}
          <Route path="logs/user-wise" element={<UserWiseLogPage />} />
          <Route path="logs/login" element={<LoginLogPage />} />
          <Route path="logs/web-api" element={<WebApiLogPage />} />
          <Route path="logs/ird-api" element={<IRDApiLogPage />} />
          <Route path="logs/sms" element={<SMSApiLogPage />} />
          <Route path="logs/notifications" element={<NotificationLogPage />} />
          <Route path="logs/email" element={<EmailLogPage />} />
          <Route path="logs/jobs" element={<JobLogPage />} />
          <Route path="logs/sql-errors" element={<SqlErrorLogPage />} />

          {/* Reports */}
          <Route path="reports/trial-balance" element={<TrialBalancePage />} />
          <Route path="reports/balance-sheet" element={<BalanceSheetPage />} />
          <Route path="reports/profit-loss" element={<ProfitLossPage />} />
          <Route path="reports/cash-flow" element={<CashFlowPage />} />
          <Route path="reports/day-book" element={<DayBookPage />} />
          <Route path="reports/cancel-day-book" element={<CancelDayBookPage />} />
          <Route path="reports/cash-bank-book" element={<CashBankBookPage />} />
          <Route path="reports/ledger-statement" element={<LedgerStatementPage />} />
          <Route path="reports/ledger-groups" element={<LedgerGroupReportPage />} />
          <Route path="reports/ledgers" element={<LedgerReportPage />} />
          <Route path="reports/ledger-analysis" element={<LedgerAnalysisPage />} />
          <Route path="reports/ledger-opening" element={<LedgerOpeningReportPage />} />
          <Route path="reports/cost-centers" element={<CostCenterReportPage />} />
          <Route path="reports/bg-details" element={<BGDetailsReportPage />} />
          <Route path="reports/pdc" element={<PDCReportPage />} />
          <Route path="reports/odc" element={<ODCReportPage />} />
          <Route path="reports/bills-receivable" element={<BillsReceivableReportPage />} />
          <Route path="reports/bills-payable" element={<BillsPayableReportPage />} />
          <Route path="reports/vat-summary" element={<VATSummaryReportPage />} />
          <Route path="reports/tds-summary" element={<TDSSummaryReportPage />} />
          <Route path="reports/patient-outstanding" element={<PatientOutstandingPage />} />
          <Route path="reports/stock-summary" element={<StockSummaryPage />} />
          <Route path="reports/stock-aging" element={<StockAgingPage />} />
          <Route path="reports/product-voucher" element={<ProductVoucherPage />} />
          <Route path="reports/product-ageing" element={<ProductAgeingPage />} />
          <Route path="reports/near-expiry" element={<NearExpiryPage />} />
          <Route path="reports/stock-transfer" element={<StockTransferReportPage />} />
          <Route path="reports/pending-po" element={<PendingPOSummaryPage />} />
          <Route path="reports/sales-allotment" element={<SalesAllotmentReportPage />} />
          <Route path="reports/purchase-analysis" element={<PurchaseAnalysisProductPage />} />
          <Route path="reports/sales-analysis" element={<SalesAnalysisPage />} />
          <Route path="reports/sales-analysis-product" element={<SalesAnalysisProductPage />} />
          <Route path="reports/product-monthly-summary" element={<ProductMonthlySummaryPage />} />
          <Route path="reports/employee-summary" element={<EmployeeSummaryReportPage />} />
          <Route path="reports/service-tenure" element={<ServiceTenureReportPage />} />
          <Route path="reports/grievance-list" element={<GrievanceListReportPage />} />
          <Route path="reports/attendance-appeals" element={<AttendanceAppealsPage />} />
          <Route path="reports/loans" element={<LoanReportPage />} />
          <Route path="reports/loan-details" element={<LoanDetailsPage />} />
          <Route path="reports/loan-monthly" element={<LoanMonthlyReportPage />} />
          <Route path="reports/service-reminder" element={<ServiceReminderPage />} />
          <Route path="reports/fourth-call-log" element={<FourthCallLogPage />} />
          <Route path="reports/service-dashboard" element={<ServiceDashboardPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouter;
