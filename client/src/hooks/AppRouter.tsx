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
// Dashboard - Additional
import InventorySalesDashboardPage from '../pages/dashboard/InventorySalesDashboardPage';
import SalesMetricsDashboardPage from '../pages/dashboard/SalesMetricsDashboardPage';

// Account - Additional Masters
import CurrencyListPage from '../pages/account/CurrencyListPage';
import PDCDetailsListPage from '../pages/account/PDCDetailsListPage';
import ODCDetailsListPage from '../pages/account/ODCDetailsListPage';
import BankGuaranteeListPage from '../pages/account/BankGuaranteeListPage';
import BillWiseLedgerOpeningListPage from '../pages/account/BillWiseLedgerOpeningListPage';
import TDSLedgerOpeningListPage from '../pages/account/TDSLedgerOpeningListPage';
import CostClassListPage from '../pages/account/CostClassListPage';
import LedgerCreditListPage from '../pages/account/LedgerCreditListPage';
import LedgerAuthorizedListPage from '../pages/account/LedgerAuthorizedListPage';
import TranStatusListPage from '../pages/account/TranStatusListPage';
import VoucherAuthorizedListPage from '../pages/account/VoucherAuthorizedListPage';
import ReceiptEntryListPage from '../pages/account/ReceiptEntryListPage';
import PriceListPage from '../pages/account/PriceListPage';
import BranchWiseLedgerOpeningListPage from '../pages/account/BranchWiseLedgerOpeningListPage';
import SalesManTargetListPage from '../pages/account/SalesManTargetListPage';
import LedgerTargetListPage from '../pages/account/LedgerTargetListPage';
import CostCenterLedgerOpeningListPage from '../pages/account/CostCenterLedgerOpeningListPage';
import LCDetailsListPage from '../pages/account/LCDetailsListPage';
import PendingCustomerListPage from '../pages/account/PendingCustomerListPage';
import PendingLocationListPage from '../pages/account/PendingLocationListPage';
import DeactiveCustomerListPage from '../pages/account/DeactiveCustomerListPage';
import IncomingPaymentPage from '../pages/account/IncomingPaymentPage';
import OutgoingPaymentPage from '../pages/account/OutgoingPaymentPage';
import VendorPaymentPage from '../pages/account/VendorPaymentPage';
import CashDepositePage from '../pages/account/CashDepositePage';
import CashRefundPage from '../pages/account/CashRefundPage';
import PatientDrCrPage from '../pages/account/PatientDrCrPage';
import LedgerMergePage from '../pages/account/LedgerMergePage';
import RouteMergePage from '../pages/account/RouteMergePage';
import ReVoucherNumberingPage from '../pages/account/ReVoucherNumberingPage';
import Annex10ReportPage from '../pages/account/Annex10ReportPage';
import AccountConfirmationLetterPage from '../pages/account/AccountConfirmationLetterPage';
import ExciseRegisterPage from '../pages/account/ExciseRegisterPage';
import LedgerVoucherReportPage from '../pages/account/LedgerVoucherReportPage';
import LedgerContactListPage from '../pages/account/LedgerContactListPage';
import CostClassReportPage from '../pages/account/CostClassReportPage';
import AreaReportPage from '../pages/account/AreaReportPage';
import SalesmanReportPage from '../pages/account/SalesmanReportPage';
import NarrationMasterReportPage from '../pages/account/NarrationMasterReportPage';
import CurrencyReportPage from '../pages/account/CurrencyReportPage';
import CurrencyRateReportPage from '../pages/account/CurrencyRateReportPage';
import VoucherReportPage from '../pages/account/VoucherReportPage';
import CostCenterOpeningDetailsPage from '../pages/account/CostCenterOpeningDetailsPage';
import LedgerWiseReportPage from '../pages/account/LedgerWiseReportPage';
import ProfitAndLossAsTPage from '../pages/account/ProfitAndLossAsTPage';
import BalanceSheetAsTPage from '../pages/account/BalanceSheetAsTPage';
import LedgerDailyPage from '../pages/account/LedgerDailyPage';
import CostCenterSummaryPage from '../pages/account/CostCenterSummaryPage';
import CostCenterBreakupLedgerWisePage from '../pages/account/CostCenterBreakupLedgerWisePage';
import StatisticVoucherPage from '../pages/account/StatisticVoucherPage';
import StatisticVoucherMonthlyPage from '../pages/account/StatisticVoucherMonthlyPage';
import StatisticVoucherDailyPage from '../pages/account/StatisticVoucherDailyPage';
import TDSVatReportPage from '../pages/account/TDSVatReportPage';
import LedgerCurrentStatusPage from '../pages/account/LedgerCurrentStatusPage';
import BillWiseReportPage from '../pages/account/BillWiseReportPage';
import ProductShapeListPage from '../pages/inventory/ProductShapeListPage';
import UploadProductPhotoPage from '../pages/inventory/UploadProductPhotoPage';
import PurchaseRateTypesListPage from '../pages/inventory/PurchaseRateTypesListPage';
import SalesRateTypesListPage from '../pages/inventory/SalesRateTypesListPage';
import AllAgentSalesSummaryPage from '../pages/inventory/AllAgentSalesSummaryPage';
import BankAllotmentListPage from '../pages/inventory/BankAllotmentListPage';
import BankDOListPage from '../pages/inventory/BankDOListPage';
import BankPayLetterListPage from '../pages/inventory/BankPayLetterListPage';
import BankQuotationListPage from '../pages/inventory/BankQuotationListPage';
import CannibalizeInListPage from '../pages/inventory/CannibalizeInListPage';
import CannibalizeOutListPage from '../pages/inventory/CannibalizeOutListPage';
import CounterSalesPage from '../pages/inventory/CounterSalesPage';
import CRLimitExpiredPartyPage from '../pages/inventory/CRLimitExpiredPartyPage';
import DairyPurchaseInvoicePage from '../pages/inventory/DairyPurchaseInvoicePage';
import DairySalesInvoicePage from '../pages/inventory/DairySalesInvoicePage';
import DispatchSectionListPage from '../pages/inventory/DispatchSectionListPage';
import FixedProductListPage from '../pages/inventory/FixedProductListPage';
import GRNAdditionalInvoicePage from '../pages/inventory/GRNAdditionalInvoicePage';
import InsuranceListPage from '../pages/inventory/InsuranceListPage';
import ManufacturingStockJournalPage from '../pages/inventory/ManufacturingStockJournalPage';
import MeterReadingListPage from '../pages/inventory/MeterReadingListPage';
import NamsariListPage from '../pages/inventory/NamsariListPage';
import PendingCannibalizeInPage from '../pages/inventory/PendingCannibalizeInPage';
import PendingPartsDemandPage from '../pages/inventory/PendingPartsDemandPage';
import PendingPurchaseInvoicePage from '../pages/inventory/PendingPurchaseInvoicePage';
import PendingSalesAllotmentPage from '../pages/inventory/PendingSalesAllotmentPage';
import PendingStockReceivedPage from '../pages/inventory/PendingStockReceivedPage';
import PetrolPumpDeliveryPage from '../pages/inventory/PetrolPumpDeliveryPage';
import PetrolPumpOrderPage from '../pages/inventory/PetrolPumpOrderPage';
import PetrolPumpPage from '../pages/inventory/PetrolPumpPage';
import ProductionAdditionalInvoicePage from '../pages/inventory/ProductionAdditionalInvoicePage';
import ProductRackListPage from '../pages/inventory/ProductRackListPage';
import ProductRateListPage from '../pages/inventory/ProductRateListPage';
import ProductWiseAdditionalCostPage from '../pages/inventory/ProductWiseAdditionalCostPage';
import PurchaseAdditionalInvoicePage from '../pages/inventory/PurchaseAdditionalInvoicePage';
import PurchaseCreditNoteListPage from '../pages/inventory/PurchaseCreditNoteListPage';
import PurchaseDebitNoteListPage from '../pages/inventory/PurchaseDebitNoteListPage';
import PurchaseLandedCostListPage from '../pages/inventory/PurchaseLandedCostListPage';
import ReceiptNoteReturnListPage from '../pages/inventory/ReceiptNoteReturnListPage';
import SalesAllotmentCancelListPage from '../pages/inventory/SalesAllotmentCancelListPage';
import SalesAllotmentListPage from '../pages/inventory/SalesAllotmentListPage';
import SalesAllotmentReturnListPage from '../pages/inventory/SalesAllotmentReturnListPage';
import SalesDebitNoteListPage from '../pages/inventory/SalesDebitNoteListPage';
import SalesCreditNoteListPage from '../pages/inventory/SalesCreditNoteListPage';
import CostCenterAgeingPage from '../pages/inventory/CostCenterAgeingPage';
import DairyPurchaseReportPage from '../pages/inventory/DairyPurchaseReportPage';
import DairyPurchaseSetupPage from '../pages/inventory/DairyPurchaseSetupPage';
import DairySalesReportPage from '../pages/inventory/DairySalesReportPage';
import FixedProductInOutDetailsPage from '../pages/inventory/FixedProductInOutDetailsPage';
import FixedProductReportProductWisePage from '../pages/inventory/FixedProductReportProductWisePage';
import FixedProductSellingRateReportPage from '../pages/inventory/FixedProductSellingRateReportPage';
import GodownReportPage from '../pages/inventory/GodownReportPage';
import OutStockBillWisePage from '../pages/inventory/OutStockBillWisePage';
import PartywiseProductRateReportPage from '../pages/inventory/PartywiseProductRateReportPage';
import ProductCategoriesReportPage from '../pages/inventory/ProductCategoriesReportPage';
import ProductCompanyReportPage from '../pages/inventory/ProductCompanyReportPage';
import ProductCurrentStatusPage from '../pages/inventory/ProductCurrentStatusPage';
import ProductGroupReportPage from '../pages/inventory/ProductGroupReportPage';
import ProductMergePage from '../pages/inventory/ProductMergePage';
import ProductOpeningReportPage from '../pages/inventory/ProductOpeningReportPage';
import ProductReportPage from '../pages/inventory/ProductReportPage';
import ProductSchemeReportPage from '../pages/inventory/ProductSchemeReportPage';
import ProductTypeReportPage from '../pages/inventory/ProductTypeReportPage';
import ProductwiseAnalysisPage from '../pages/inventory/ProductwiseAnalysisPage';
import SalesAllotmentDetailsPage from '../pages/inventory/SalesAllotmentDetailsPage';
import SalesCostingVoucherWisePage from '../pages/inventory/SalesCostingVoucherWisePage';
import SalesMaterializedViewPage from '../pages/inventory/SalesMaterializedViewPage';
import SalesOrderCancelListPage from '../pages/inventory/SalesOrderCancelListPage';
import StockDemandListPage from '../pages/inventory/StockDemandListPage';
import StockJournalBOMPage from '../pages/inventory/StockJournalBOMPage';
import StockSummaryIRDPage from '../pages/inventory/StockSummaryIRDPage';
import StockTransforListPage from '../pages/inventory/StockTransforListPage';
import TeaPurchaseInvoicePage from '../pages/inventory/TeaPurchaseInvoicePage';
import TraiilCenterSIPage from '../pages/inventory/TraiilCenterSIPage';
import TrailCenterCounterSIPage from '../pages/inventory/TrailCenterCounterSIPage';
import UnitReportPage from '../pages/inventory/UnitReportPage';
import SalesDeliveryNoteReturnListPage from '../pages/inventory/SalesDeliveryNoteReturnListPage';
import BedMappingPage from '../pages/hms/BedMappingPage';
import BuildingTypeListPage from '../pages/hms/BuildingTypeListPage';
import CommissionTypeListPage from '../pages/hms/CommissionTypeListPage';
import DisabilityListPage from '../pages/hms/DisabilityListPage';
import DiscountCommisionMappingPage from '../pages/hms/DiscountCommisionMappingPage';
import DonarListPage from '../pages/hms/DonarListPage';
import EthinicityListPage from '../pages/hms/EthinicityListPage';
import HmsCashDepositePage from '../pages/hms/HmsCashDepositePage';
import HmsServiceListPage from '../pages/hms/HmsServiceListPage';
import HMSVoucherPage from '../pages/hms/HMSVoucherPage';
import InPatientFormPage from '../pages/hms/InPatientFormPage';
import OutPatientFormPage from '../pages/hms/OutPatientFormPage';
import AddResultPage from '../pages/lab/AddResultPage';
import FinalReportsPage from '../pages/lab/FinalReportsPage';
import LabEntityNumberingPage from '../pages/lab/LabEntityNumberingPage';
import LabReportTemplatePage from '../pages/lab/LabReportTemplatePage';
import LabUnitsListPage from '../pages/lab/LabUnitsListPage';
import PendingLabReportsPage from '../pages/lab/PendingLabReportsPage';
import SampleCollectionPage from '../pages/lab/SampleCollectionPage';
import CloseJobCardPage from '../pages/service/CloseJobCardPage';
import ComplainInspectionListPage from '../pages/service/ComplainInspectionListPage';
import DeviceColorListPage from '../pages/service/DeviceColorListPage';
import DeviceEntryListPage from '../pages/service/DeviceEntryListPage';
import FifthCallLogPage from '../pages/service/FifthCallLogPage';
import InspectionTypeListPage from '../pages/service/InspectionTypeListPage';
import JobTypeMappingPage from '../pages/service/JobTypeMappingPage';
import ServiceJobCardFormPage from '../pages/service/ServiceJobCardFormPage';
import ServiceMembersPage from '../pages/service/ServiceMembersPage';
import SixthCallLogPage from '../pages/service/SixthCallLogPage';
import SparePartsDemandListPage from '../pages/service/SparePartsDemandListPage';
import SparePartsIssueListPage from '../pages/service/SparePartsIssueListPage';
import SparePartsReturnListPage from '../pages/service/SparePartsReturnListPage';
import TeachRptComplainInspectionListPage from '../pages/service/TeachRptComplainInspectionListPage';
import TeachRptJobCardListPage from '../pages/service/TeachRptJobCardListPage';
import TeachRptJobCardStatusPage from '../pages/service/TeachRptJobCardStatusPage';
import TeachRptServiceSparePartsPage from '../pages/service/TeachRptServiceSparePartsPage';
import TeachRptVehicleHistoryPage from '../pages/service/TeachRptVehicleHistoryPage';
import TeachRptVehicleListPage from '../pages/service/TeachRptVehicleListPage';
import AssetCodeMethodPage from '../pages/assets/AssetCodeMethodPage';
import AssetIssueStatusPage from '../pages/assets/AssetIssueStatusPage';
import AssetOpeningListPage from '../pages/assets/AssetOpeningListPage';
import AssetRequestStatusPage from '../pages/assets/AssetRequestStatusPage';
import AssetsOpeningListReportPage from '../pages/assets/AssetsOpeningListReportPage';
import AssetsUseDetailsPage from '../pages/assets/AssetsUseDetailsPage';
import AssetWiseReportPage from '../pages/assets/AssetWiseReportPage';
import EmployeeWiseAssetsDPage from '../pages/assets/EmployeeWiseAssetsDPage';
import RepairedInwardPage from '../pages/assets/RepairedInwardPage';
import VendorWiseAssetPage from '../pages/assets/VendorWiseAssetPage';
import CalculateRebatePenaltyPage from '../pages/finance/CalculateRebatePenaltyPage';
import EndLoanPage from '../pages/finance/EndLoanPage';
import FinanceConfigPage from '../pages/finance/FinanceConfigPage';
import MonthEndPage from '../pages/finance/MonthEndPage';
import RescheduleLoanPage from '../pages/finance/RescheduleLoanPage';
import VehicleDetailListPage from '../pages/finance/VehicleDetailListPage';
import AllowLeaveTypePage from '../pages/hr/AllowLeaveTypePage';
import EmployeeYearlyAttendancePage from '../pages/hr/EmployeeYearlyAttendancePage';
import ExpenseClaimPage from '../pages/hr/ExpenseClaimPage';
import ExpenseDetailPage from '../pages/hr/ExpenseDetailPage';
import HRAuditLogReportPage from '../pages/hr/HRAuditLogReportPage';
import HRBankAccountDetailsPage from '../pages/hr/HRBankAccountDetailsPage';
import HRConfigurationPage from '../pages/hr/HRConfigurationPage';
import LeaveOpeningPage from '../pages/hr/LeaveOpeningPage';
import ManualAttendancePage from '../pages/hr/ManualAttendancePage';
import MonthlyExpenseSummaryPage from '../pages/hr/MonthlyExpenseSummaryPage';
import TravelModeListPage from '../pages/hr/TravelModeListPage';
import AccountConfigurationPage from '../pages/setup/AccountConfigurationPage';
import ActiveSecurityPage from '../pages/setup/ActiveSecurityPage';
import AgentUserPage from '../pages/setup/AgentUserPage';
import AllowBackdateEntryPage from '../pages/setup/AllowBackdateEntryPage';
import AllowCategoryPage from '../pages/setup/AllowCategoryPage';
import AllowCostCategoryPage from '../pages/setup/AllowCostCategoryPage';
import AllowCostClassPage from '../pages/setup/AllowCostClassPage';
import AllowDepartmentPage from '../pages/setup/AllowDepartmentPage';
import AllowEntityEnableDisabledPage from '../pages/setup/AllowEntityEnableDisabledPage';
import AllowLedgerGroupPage from '../pages/setup/AllowLedgerGroupPage';
import AllowLevelPage from '../pages/setup/AllowLevelPage';
import AllowProductGroupPage from '../pages/setup/AllowProductGroupPage';
import AllowServiceTypePage from '../pages/setup/AllowServiceTypePage';
import AllowUserWiseGroupPage from '../pages/setup/AllowUserWiseGroupPage';
import AllowVoucherForPostPage from '../pages/setup/AllowVoucherForPostPage';
import AllowVoucherPage from '../pages/setup/AllowVoucherPage';
import ChangePasswordPage from '../pages/setup/ChangePasswordPage';
import CopyUserSecurityPage from '../pages/setup/CopyUserSecurityPage';
import DealerUserPage from '../pages/setup/DealerUserPage';
import EmployeeUserPage from '../pages/setup/EmployeeUserPage';
import EntityPropertiesPage from '../pages/setup/EntityPropertiesPage';
import GlobalActionPage from '../pages/setup/GlobalActionPage';
import ImportDataPage from '../pages/setup/ImportDataPage';
import ImportExportTranPage from '../pages/setup/ImportExportTranPage';
import InventoryConfigurationPage from '../pages/setup/InventoryConfigurationPage';
import NotApplicableLedgerPage from '../pages/setup/NotApplicableLedgerPage';
import PartsDemandGodownPage from '../pages/setup/PartsDemandGodownPage';
import ReportTemplatePage from '../pages/setup/ReportTemplatePage';
import ReportWriterDashboardDesignerPage from '../pages/setup/ReportWriterDashboardDesignerPage';
import ReportWriterDynamicAIPage from '../pages/setup/ReportWriterDynamicAIPage';
import ReportWriterNewEntityPage from '../pages/setup/ReportWriterNewEntityPage';
import ReportWriterQueryBuilderPage from '../pages/setup/ReportWriterQueryBuilderPage';
import ReportWriterRunViewerPage from '../pages/setup/ReportWriterRunViewerPage';
import ResetPasswordPage from '../pages/setup/ResetPasswordPage';
import SCTQRPage from '../pages/setup/SCTQRPage';
import SecurityConsolePage from '../pages/setup/SecurityConsolePage';
import SENTCustomPage from '../pages/setup/SENTCustomPage';
import SENTPage from '../pages/setup/SENTPage';
import SENTVoucherPage from '../pages/setup/SENTVoucherPage';
import SSFApiUserPage from '../pages/setup/SSFApiUserPage';
import TallyTranPage from '../pages/setup/TallyTranPage';
import UDFPage from '../pages/setup/UDFPage';
import UserBranchPage from '../pages/setup/UserBranchPage';
import UserWiseAutoPostPage from '../pages/setup/UserWiseAutoPostPage';
import UserWiseBranchPage from '../pages/setup/UserWiseBranchPage';
import UserWiseGodownPage from '../pages/setup/UserWiseGodownPage';
import UserWiseModulePage from '../pages/setup/UserWiseModulePage';
import VReportTemplatePage from '../pages/setup/VReportTemplatePage';
import YearClosingPage from '../pages/setup/YearClosingPage';
import MonthlyTaskPage from '../pages/task/MonthlyTaskPage';
import TaskTypePage from '../pages/task/TaskTypePage';
import ViewTaskPage from '../pages/task/ViewTaskPage';
import AcademicCalenderPage from '../pages/cms/AcademicCalenderPage';
import ServicesAndFacilitiesPage from '../pages/cms/ServicesAndFacilitiesPage';
import WeekendPage from '../pages/cms/WeekendPage';
import SupportDashboardPage from '../pages/support/SupportDashboardPage';
import SupportExecutivePage from '../pages/support/SupportExecutivePage';
import LoyaltySalesSummaryPage from '../pages/loyalty/LoyaltySalesSummaryPage';
import MembershipPointSummaryPage from '../pages/loyalty/MembershipPointSummaryPage';
import JobEntityPage from '../pages/jobs/JobEntityPage';

// Dashboard - Additional

// Account - Additional Masters

// Account - Additional Transactions

// Account - Additional Reports

// Inventory - Additional Creation

// Inventory - Additional Transactions

// Inventory - Additional Reports

// HMS - Additional

// Lab - Additional

// Service - Additional

// Assets - Additional

// Finance - Additional

// HR - Additional

// Setup - Additional

// Task - Additional

// CMS - Additional

// Support - Additional

// Loyalty - Additional

// Jobs

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

          {/* Dashboard - Additional */}
          <Route path="dashboard/inventory-sales" element={<InventorySalesDashboardPage />} />
          <Route path="dashboard/sales-metrics" element={<SalesMetricsDashboardPage />} />

          {/* Account - Additional Masters */}
          <Route path="account/currencies" element={<CurrencyListPage />} />
          <Route path="account/pdc-details" element={<PDCDetailsListPage />} />
          <Route path="account/odc-details" element={<ODCDetailsListPage />} />
          <Route path="account/bank-guarantees" element={<BankGuaranteeListPage />} />
          <Route path="account/bill-wise-ledger-opening" element={<BillWiseLedgerOpeningListPage />} />
          <Route path="account/tds-ledger-opening" element={<TDSLedgerOpeningListPage />} />
          <Route path="account/cost-classes" element={<CostClassListPage />} />
          <Route path="account/ledger-credit" element={<LedgerCreditListPage />} />
          <Route path="account/ledger-authorized" element={<LedgerAuthorizedListPage />} />
          <Route path="account/tran-status" element={<TranStatusListPage />} />
          <Route path="account/voucher-authorized" element={<VoucherAuthorizedListPage />} />
          <Route path="account/receipt-entry" element={<ReceiptEntryListPage />} />
          <Route path="account/price-list" element={<PriceListPage />} />
          <Route path="account/branch-wise-ledger-opening" element={<BranchWiseLedgerOpeningListPage />} />
          <Route path="account/salesman-targets" element={<SalesManTargetListPage />} />
          <Route path="account/ledger-targets" element={<LedgerTargetListPage />} />
          <Route path="account/cost-center-ledger-opening" element={<CostCenterLedgerOpeningListPage />} />
          <Route path="account/lc-details" element={<LCDetailsListPage />} />
          <Route path="account/pending-customers" element={<PendingCustomerListPage />} />
          <Route path="account/pending-locations" element={<PendingLocationListPage />} />
          <Route path="account/deactive-customers" element={<DeactiveCustomerListPage />} />

          {/* Account - Additional Transactions */}
          <Route path="account/incoming-payments" element={<IncomingPaymentPage />} />
          <Route path="account/outgoing-payments" element={<OutgoingPaymentPage />} />
          <Route path="account/vendor-payment" element={<VendorPaymentPage />} />
          <Route path="account/cash-deposit" element={<CashDepositePage />} />
          <Route path="account/cash-refund" element={<CashRefundPage />} />
          <Route path="account/patient-dr-cr" element={<PatientDrCrPage />} />
          <Route path="account/ledger-merge" element={<LedgerMergePage />} />
          <Route path="account/route-merge" element={<RouteMergePage />} />
          <Route path="account/re-voucher-numbering" element={<ReVoucherNumberingPage />} />

          {/* Account - Additional Reports */}
          <Route path="account/annex10" element={<Annex10ReportPage />} />
          <Route path="account/confirmation-letter" element={<AccountConfirmationLetterPage />} />
          <Route path="account/excise-register" element={<ExciseRegisterPage />} />
          <Route path="account/ledger-voucher-report" element={<LedgerVoucherReportPage />} />
          <Route path="account/ledger-contacts" element={<LedgerContactListPage />} />
          <Route path="account/cost-class-report" element={<CostClassReportPage />} />
          <Route path="account/area-report" element={<AreaReportPage />} />
          <Route path="account/salesman-report" element={<SalesmanReportPage />} />
          <Route path="account/narration-report" element={<NarrationMasterReportPage />} />
          <Route path="account/currency-report" element={<CurrencyReportPage />} />
          <Route path="account/currency-rate-report" element={<CurrencyRateReportPage />} />
          <Route path="account/voucher-report" element={<VoucherReportPage />} />
          <Route path="account/cost-center-opening-details" element={<CostCenterOpeningDetailsPage />} />
          <Route path="account/ledger-wise-report" element={<LedgerWiseReportPage />} />
          <Route path="account/profit-loss-as-t" element={<ProfitAndLossAsTPage />} />
          <Route path="account/balance-sheet-as-t" element={<BalanceSheetAsTPage />} />
          <Route path="account/ledger-daily" element={<LedgerDailyPage />} />
          <Route path="account/cost-center-summary" element={<CostCenterSummaryPage />} />
          <Route path="account/cost-center-breakup" element={<CostCenterBreakupLedgerWisePage />} />
          <Route path="account/statistic-voucher" element={<StatisticVoucherPage />} />
          <Route path="account/statistic-voucher-monthly" element={<StatisticVoucherMonthlyPage />} />
          <Route path="account/statistic-voucher-daily" element={<StatisticVoucherDailyPage />} />
          <Route path="account/tds-vat-report" element={<TDSVatReportPage />} />
          <Route path="account/ledger-current-status" element={<LedgerCurrentStatusPage />} />
          <Route path="account/bill-wise-report" element={<BillWiseReportPage />} />

          {/* Inventory - Additional Creation */}
          <Route path="inventory/product-shapes" element={<ProductShapeListPage />} />
          <Route path="inventory/upload-photos" element={<UploadProductPhotoPage />} />
          <Route path="inventory/purchase-rate-types" element={<PurchaseRateTypesListPage />} />
          <Route path="inventory/sales-rate-types" element={<SalesRateTypesListPage />} />

          {/* Inventory - Additional Transactions */}
          <Route path="inventory/all-agent-sales-summary" element={<AllAgentSalesSummaryPage />} />
          <Route path="inventory/bank-allotment" element={<BankAllotmentListPage />} />
          <Route path="inventory/bank-do" element={<BankDOListPage />} />
          <Route path="inventory/bank-pay-letter" element={<BankPayLetterListPage />} />
          <Route path="inventory/bank-quotation" element={<BankQuotationListPage />} />
          <Route path="inventory/cannibalize-in" element={<CannibalizeInListPage />} />
          <Route path="inventory/cannibalize-out" element={<CannibalizeOutListPage />} />
          <Route path="inventory/counter-sales" element={<CounterSalesPage />} />
          <Route path="inventory/cr-limit-expired" element={<CRLimitExpiredPartyPage />} />
          <Route path="inventory/dairy-purchase-invoice" element={<DairyPurchaseInvoicePage />} />
          <Route path="inventory/dairy-sales-invoice" element={<DairySalesInvoicePage />} />
          <Route path="inventory/dispatch-sections" element={<DispatchSectionListPage />} />
          <Route path="inventory/fixed-products" element={<FixedProductListPage />} />
          <Route path="inventory/grn-additional-invoice" element={<GRNAdditionalInvoicePage />} />
          <Route path="inventory/insurance" element={<InsuranceListPage />} />
          <Route path="inventory/manufacturing-stock-journal" element={<ManufacturingStockJournalPage />} />
          <Route path="inventory/meter-readings" element={<MeterReadingListPage />} />
          <Route path="inventory/namsari" element={<NamsariListPage />} />
          <Route path="inventory/pending-cannibalize-in" element={<PendingCannibalizeInPage />} />
          <Route path="inventory/pending-parts-demand" element={<PendingPartsDemandPage />} />
          <Route path="inventory/pending-purchase-invoice" element={<PendingPurchaseInvoicePage />} />
          <Route path="inventory/pending-sales-allotment" element={<PendingSalesAllotmentPage />} />
          <Route path="inventory/pending-stock-received" element={<PendingStockReceivedPage />} />
          <Route path="inventory/petrol-pump-delivery" element={<PetrolPumpDeliveryPage />} />
          <Route path="inventory/petrol-pump-order" element={<PetrolPumpOrderPage />} />
          <Route path="inventory/petrol-pump" element={<PetrolPumpPage />} />
          <Route path="inventory/production-additional-invoice" element={<ProductionAdditionalInvoicePage />} />
          <Route path="inventory/product-racks" element={<ProductRackListPage />} />
          <Route path="inventory/product-rates" element={<ProductRateListPage />} />
          <Route path="inventory/product-wise-additional-cost" element={<ProductWiseAdditionalCostPage />} />
          <Route path="inventory/purchase-additional-invoice" element={<PurchaseAdditionalInvoicePage />} />
          <Route path="inventory/purchase-credit-notes" element={<PurchaseCreditNoteListPage />} />
          <Route path="inventory/purchase-debit-notes" element={<PurchaseDebitNoteListPage />} />
          <Route path="inventory/purchase-landed-cost" element={<PurchaseLandedCostListPage />} />
          <Route path="inventory/receipt-note-returns" element={<ReceiptNoteReturnListPage />} />
          <Route path="inventory/sales-allotment-cancel" element={<SalesAllotmentCancelListPage />} />
          <Route path="inventory/sales-allotment" element={<SalesAllotmentListPage />} />
          <Route path="inventory/sales-allotment-return" element={<SalesAllotmentReturnListPage />} />
          <Route path="inventory/sales-debit-notes" element={<SalesDebitNoteListPage />} />
          <Route path="inventory/sales-credit-notes" element={<SalesCreditNoteListPage />} />

          {/* Inventory - Additional Reports */}
          <Route path="inventory/cost-center-ageing" element={<CostCenterAgeingPage />} />
          <Route path="inventory/dairy-purchase-report" element={<DairyPurchaseReportPage />} />
          <Route path="inventory/dairy-purchase-setup" element={<DairyPurchaseSetupPage />} />
          <Route path="inventory/dairy-sales-report" element={<DairySalesReportPage />} />
          <Route path="inventory/fixed-product-in-out" element={<FixedProductInOutDetailsPage />} />
          <Route path="inventory/fixed-product-report-product-wise" element={<FixedProductReportProductWisePage />} />
          <Route path="inventory/fixed-product-selling-rate" element={<FixedProductSellingRateReportPage />} />
          <Route path="inventory/godown-report" element={<GodownReportPage />} />
          <Route path="inventory/out-stock-bill-wise" element={<OutStockBillWisePage />} />
          <Route path="inventory/partywise-product-rate" element={<PartywiseProductRateReportPage />} />
          <Route path="inventory/product-categories-report" element={<ProductCategoriesReportPage />} />
          <Route path="inventory/product-company-report" element={<ProductCompanyReportPage />} />
          <Route path="inventory/product-current-status" element={<ProductCurrentStatusPage />} />
          <Route path="inventory/product-group-report" element={<ProductGroupReportPage />} />
          <Route path="inventory/product-merge" element={<ProductMergePage />} />
          <Route path="inventory/product-opening-report" element={<ProductOpeningReportPage />} />
          <Route path="inventory/product-report" element={<ProductReportPage />} />
          <Route path="inventory/product-scheme-report" element={<ProductSchemeReportPage />} />
          <Route path="inventory/product-type-report" element={<ProductTypeReportPage />} />
          <Route path="inventory/productwise-analysis" element={<ProductwiseAnalysisPage />} />
          <Route path="inventory/sales-allotment-details" element={<SalesAllotmentDetailsPage />} />
          <Route path="inventory/sales-costing-voucher-wise" element={<SalesCostingVoucherWisePage />} />
          <Route path="inventory/sales-materialized-view" element={<SalesMaterializedViewPage />} />
          <Route path="inventory/sales-order-cancel" element={<SalesOrderCancelListPage />} />
          <Route path="inventory/stock-demand" element={<StockDemandListPage />} />
          <Route path="inventory/stock-journal-bom" element={<StockJournalBOMPage />} />
          <Route path="inventory/stock-summary-ird" element={<StockSummaryIRDPage />} />
          <Route path="inventory/stock-transfor" element={<StockTransforListPage />} />
          <Route path="inventory/tea-purchase-invoice" element={<TeaPurchaseInvoicePage />} />
          <Route path="inventory/trail-center-si" element={<TraiilCenterSIPage />} />
          <Route path="inventory/trail-center-counter-si" element={<TrailCenterCounterSIPage />} />
          <Route path="inventory/unit-report" element={<UnitReportPage />} />
          <Route path="inventory/sales-delivery-note-return" element={<SalesDeliveryNoteReturnListPage />} />

          {/* HMS - Additional */}
          <Route path="hms/bed-mapping" element={<BedMappingPage />} />
          <Route path="hms/building-types" element={<BuildingTypeListPage />} />
          <Route path="hms/commission-types" element={<CommissionTypeListPage />} />
          <Route path="hms/disabilities" element={<DisabilityListPage />} />
          <Route path="hms/discount-commission-mapping" element={<DiscountCommisionMappingPage />} />
          <Route path="hms/donars" element={<DonarListPage />} />
          <Route path="hms/ethnicities" element={<EthinicityListPage />} />
          <Route path="hms/cash-deposit" element={<HmsCashDepositePage />} />
          <Route path="hms/services" element={<HmsServiceListPage />} />
          <Route path="hms/vouchers" element={<HMSVoucherPage />} />
          <Route path="hms/in-patients/new" element={<InPatientFormPage />} />
          <Route path="hms/out-patients/new" element={<OutPatientFormPage />} />

          {/* Lab - Additional */}
          <Route path="lab/add-result" element={<AddResultPage />} />
          <Route path="lab/final-reports" element={<FinalReportsPage />} />
          <Route path="lab/entity-numbering" element={<LabEntityNumberingPage />} />
          <Route path="lab/report-template" element={<LabReportTemplatePage />} />
          <Route path="lab/units" element={<LabUnitsListPage />} />
          <Route path="lab/pending-reports" element={<PendingLabReportsPage />} />
          <Route path="lab/sample-collection" element={<SampleCollectionPage />} />

          {/* Service - Additional */}
          <Route path="service/close-jobcard" element={<CloseJobCardPage />} />
          <Route path="service/complain-inspections" element={<ComplainInspectionListPage />} />
          <Route path="service/device-colors" element={<DeviceColorListPage />} />
          <Route path="service/device-entry" element={<DeviceEntryListPage />} />
          <Route path="service/fifth-call-log" element={<FifthCallLogPage />} />
          <Route path="service/inspection-types" element={<InspectionTypeListPage />} />
          <Route path="service/job-type-mapping" element={<JobTypeMappingPage />} />
          <Route path="service/jobcard-form" element={<ServiceJobCardFormPage />} />
          <Route path="service/members" element={<ServiceMembersPage />} />
          <Route path="service/sixth-call-log" element={<SixthCallLogPage />} />
          <Route path="service/spare-parts-demand" element={<SparePartsDemandListPage />} />
          <Route path="service/spare-parts-issue" element={<SparePartsIssueListPage />} />
          <Route path="service/spare-parts-return" element={<SparePartsReturnListPage />} />
          <Route path="service/teach-rpt-complain-inspection" element={<TeachRptComplainInspectionListPage />} />
          <Route path="service/teach-rpt-jobcard" element={<TeachRptJobCardListPage />} />
          <Route path="service/teach-rpt-jobcard-status" element={<TeachRptJobCardStatusPage />} />
          <Route path="service/teach-rpt-spare-parts" element={<TeachRptServiceSparePartsPage />} />
          <Route path="service/teach-rpt-vehicle-history" element={<TeachRptVehicleHistoryPage />} />
          <Route path="service/teach-rpt-vehicles" element={<TeachRptVehicleListPage />} />

          {/* Assets - Additional */}
          <Route path="assets/code-method" element={<AssetCodeMethodPage />} />
          <Route path="assets/issue-status" element={<AssetIssueStatusPage />} />
          <Route path="assets/opening" element={<AssetOpeningListPage />} />
          <Route path="assets/request-status" element={<AssetRequestStatusPage />} />
          <Route path="assets/opening-report" element={<AssetsOpeningListReportPage />} />
          <Route path="assets/use-details" element={<AssetsUseDetailsPage />} />
          <Route path="assets/asset-wise-report" element={<AssetWiseReportPage />} />
          <Route path="assets/employee-wise" element={<EmployeeWiseAssetsDPage />} />
          <Route path="assets/repaired-inward" element={<RepairedInwardPage />} />
          <Route path="assets/vendor-wise" element={<VendorWiseAssetPage />} />

          {/* Finance - Additional */}
          <Route path="finance/calculate-rebate" element={<CalculateRebatePenaltyPage />} />
          <Route path="finance/end-loan" element={<EndLoanPage />} />
          <Route path="finance/config" element={<FinanceConfigPage />} />
          <Route path="finance/month-end" element={<MonthEndPage />} />
          <Route path="finance/reschedule-loan" element={<RescheduleLoanPage />} />
          <Route path="finance/vehicle-details" element={<VehicleDetailListPage />} />

          {/* HR - Additional */}
          <Route path="hr/allow-leave-types" element={<AllowLeaveTypePage />} />
          <Route path="hr/yearly-attendance" element={<EmployeeYearlyAttendancePage />} />
          <Route path="hr/expense-claims" element={<ExpenseClaimPage />} />
          <Route path="hr/expense-details" element={<ExpenseDetailPage />} />
          <Route path="hr/audit-log" element={<HRAuditLogReportPage />} />
          <Route path="hr/bank-account-details" element={<HRBankAccountDetailsPage />} />
          <Route path="hr/configuration" element={<HRConfigurationPage />} />
          <Route path="hr/leave-opening" element={<LeaveOpeningPage />} />
          <Route path="hr/manual-attendance" element={<ManualAttendancePage />} />
          <Route path="hr/monthly-expense-summary" element={<MonthlyExpenseSummaryPage />} />
          <Route path="hr/travel-modes" element={<TravelModeListPage />} />

          {/* Setup - Additional */}
          <Route path="setup/account-configuration" element={<AccountConfigurationPage />} />
          <Route path="setup/active-security" element={<ActiveSecurityPage />} />
          <Route path="setup/agent-user" element={<AgentUserPage />} />
          <Route path="setup/allow-backdate-entry" element={<AllowBackdateEntryPage />} />
          <Route path="setup/allow-category" element={<AllowCategoryPage />} />
          <Route path="setup/allow-cost-category" element={<AllowCostCategoryPage />} />
          <Route path="setup/allow-cost-class" element={<AllowCostClassPage />} />
          <Route path="setup/allow-department" element={<AllowDepartmentPage />} />
          <Route path="setup/allow-entity-enable-disable" element={<AllowEntityEnableDisabledPage />} />
          <Route path="setup/allow-ledger-group" element={<AllowLedgerGroupPage />} />
          <Route path="setup/allow-level" element={<AllowLevelPage />} />
          <Route path="setup/allow-product-group" element={<AllowProductGroupPage />} />
          <Route path="setup/allow-service-type" element={<AllowServiceTypePage />} />
          <Route path="setup/allow-user-wise-group" element={<AllowUserWiseGroupPage />} />
          <Route path="setup/allow-voucher-for-post" element={<AllowVoucherForPostPage />} />
          <Route path="setup/allow-voucher" element={<AllowVoucherPage />} />
          <Route path="setup/change-password" element={<ChangePasswordPage />} />
          <Route path="setup/copy-user-security" element={<CopyUserSecurityPage />} />
          <Route path="setup/dealer-user" element={<DealerUserPage />} />
          <Route path="setup/employee-user" element={<EmployeeUserPage />} />
          <Route path="setup/entity-properties" element={<EntityPropertiesPage />} />
          <Route path="setup/global-action" element={<GlobalActionPage />} />
          <Route path="setup/import-data" element={<ImportDataPage />} />
          <Route path="setup/import-export-tran" element={<ImportExportTranPage />} />
          <Route path="setup/inventory-configuration" element={<InventoryConfigurationPage />} />
          <Route path="setup/not-applicable-ledger" element={<NotApplicableLedgerPage />} />
          <Route path="setup/parts-demand-godown" element={<PartsDemandGodownPage />} />
          <Route path="setup/report-template" element={<ReportTemplatePage />} />
          <Route path="setup/report-writer-dashboard" element={<ReportWriterDashboardDesignerPage />} />
          <Route path="setup/report-writer-ai" element={<ReportWriterDynamicAIPage />} />
          <Route path="setup/report-writer-entity" element={<ReportWriterNewEntityPage />} />
          <Route path="setup/report-writer-query" element={<ReportWriterQueryBuilderPage />} />
          <Route path="setup/report-writer-viewer" element={<ReportWriterRunViewerPage />} />
          <Route path="setup/reset-password" element={<ResetPasswordPage />} />
          <Route path="setup/sct-qr" element={<SCTQRPage />} />
          <Route path="setup/security-console" element={<SecurityConsolePage />} />
          <Route path="setup/sent-custom" element={<SENTCustomPage />} />
          <Route path="setup/sent" element={<SENTPage />} />
          <Route path="setup/sent-voucher" element={<SENTVoucherPage />} />
          <Route path="setup/ssf-api-user" element={<SSFApiUserPage />} />
          <Route path="setup/tally-tran" element={<TallyTranPage />} />
          <Route path="setup/udf" element={<UDFPage />} />
          <Route path="setup/user-branch" element={<UserBranchPage />} />
          <Route path="setup/user-wise-auto-post" element={<UserWiseAutoPostPage />} />
          <Route path="setup/user-wise-branch" element={<UserWiseBranchPage />} />
          <Route path="setup/user-wise-godown" element={<UserWiseGodownPage />} />
          <Route path="setup/user-wise-module" element={<UserWiseModulePage />} />
          <Route path="setup/v-report-template" element={<VReportTemplatePage />} />
          <Route path="setup/year-closing" element={<YearClosingPage />} />

          {/* Task - Additional */}
          <Route path="tasks/monthly" element={<MonthlyTaskPage />} />
          <Route path="tasks/types" element={<TaskTypePage />} />
          <Route path="tasks/:id/view" element={<ViewTaskPage />} />

          {/* CMS - Additional */}
          <Route path="cms/academic-calendar" element={<AcademicCalenderPage />} />
          <Route path="cms/services-facilities" element={<ServicesAndFacilitiesPage />} />
          <Route path="cms/weekend" element={<WeekendPage />} />

          {/* Support - Additional */}
          <Route path="support/dashboard" element={<SupportDashboardPage />} />
          <Route path="support/executives" element={<SupportExecutivePage />} />

          {/* Loyalty - Additional */}
          <Route path="loyalty/sales-summary" element={<LoyaltySalesSummaryPage />} />
          <Route path="loyalty/membership-points" element={<MembershipPointSummaryPage />} />

          {/* Jobs */}
          <Route path="jobs/entities" element={<JobEntityPage />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
};

export default AppRouter;
