export interface ApiResponse<T> {
  data: T;
  totalCount: number;
  isSuccess: boolean;
  responseMSG: string;
}

export interface LoginRequest {
  username: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  refreshToken: string;
  userId: number;
  userName: string;
  fullName: string;
  branchId: number;
  branchName: string;
}

export interface PaginationParams {
  page: number;
  pageSize: number;
  search?: string;
}

// Account
export interface LedgerGroup {
  id: number;
  code: string;
  name: string;
  parentId: number | null;
  children: LedgerGroup[];
}

export interface Ledger {
  id: number;
  code: string;
  name: string;
  ledgerGroupId: number;
  groupName: string;
  openingBalance: number;
  currentBalance: number;
}

export interface Voucher {
  id: number;
  voucherNumber: string;
  voucherDate: string;
  totalDebit: number;
  totalCredit: number;
  isPosted: boolean;
  details: VoucherDetail[];
}

export interface VoucherDetail {
  id: number;
  ledgerId: number;
  ledgerName: string;
  debitAmount: number;
  creditAmount: number;
  narration: string;
}

export interface Customer {
  id: number;
  name: string;
  code: string;
  phone: string;
  email: string;
  address: string;
}

export interface Vendor {
  id: number;
  name: string;
  code: string;
  phone: string;
  email: string;
  address: string;
}

// Inventory
export interface Product {
  id: number;
  code: string;
  name: string;
  productGroupId: number;
  groupName: string;
  unit: string;
  sellingPrice: number;
  costPrice: number;
}

export interface ProductGroup {
  id: number;
  code: string;
  name: string;
  parentId: number | null;
  children: ProductGroup[];
}

export interface Godown {
  id: number;
  code: string;
  name: string;
  address: string;
}

export interface Stock {
  id: number;
  productId: number;
  productName: string;
  godownId: number;
  godownName: string;
  quantity: number;
  rate: number;
}

// Purchase & Sales
export interface PurchaseInvoice {
  id: number;
  invoiceNumber: string;
  invoiceDate: string;
  vendorId: number;
  vendorName: string;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  totalAmount: number;
  status: string;
  details: PurchaseInvoiceDetail[];
}

export interface PurchaseInvoiceDetail {
  productId: number;
  productName: string;
  quantity: number;
  rate: number;
  amount: number;
}

export interface SalesInvoice {
  id: number;
  invoiceNumber: string;
  invoiceDate: string;
  customerId: number;
  customerName: string;
  subTotal: number;
  discountAmount: number;
  taxAmount: number;
  totalAmount: number;
  status: string;
  details: SalesInvoiceDetail[];
}

export interface SalesInvoiceDetail {
  productId: number;
  productName: string;
  quantity: number;
  rate: number;
  amount: number;
}

// HR
export interface Employee {
  id: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  fullName: string;
  phone: string;
  email: string;
  branchName: string;
  joiningDate: string;
}

export interface Attendance {
  id: number;
  employeeId: number;
  employeeName: string;
  attendanceDate: string;
  checkInTime: string;
  checkOutTime: string;
  workingHours: number;
  status: string;
}

export interface LeaveRequest {
  id: number;
  employeeId: number;
  employeeName: string;
  fromDate: string;
  toDate: string;
  totalDays: number;
  reason: string;
  status: string;
}

// HMS
export interface Patient {
  id: number;
  patientNumber: string;
  firstName: string;
  lastName: string;
  fullName: string;
  dateOfBirth: string;
  gender: string;
  phone: string;
}

export interface OPDTicket {
  id: number;
  ticketNumber: string;
  ticketDate: string;
  patientName: string;
  symptoms: string;
  diagnosis: string;
  amount: number;
  status: string;
}

export interface IPDAdmission {
  id: number;
  admissionNumber: string;
  admissionDate: string;
  patientName: string;
  bedNumber: string;
  status: string;
  dischargeDate: string;
}

// Service
export interface ComplaintTicket {
  id: number;
  ticketNumber: string;
  ticketDate: string;
  customerName: string;
  complaintDescription: string;
  priority: string;
  status: string;
}

export interface JobCard {
  id: number;
  jobCardNumber: string;
  jobCardDate: string;
  assignedToName: string;
  estimatedCost: number;
  actualCost: number;
  status: string;
}

// Reports
export interface TrialBalance {
  asOfDate: string;
  ledgers: LedgerBalance[];
  totalDebit: number;
  totalCredit: number;
  isBalanced: boolean;
}

export interface LedgerBalance {
  ledgerId: number;
  ledgerName: string;
  ledgerCode: string;
  debitTotal: number;
  creditTotal: number;
  balance: number;
  balanceType: string;
}

export interface DayBookEntry {
  voucherId: number;
  voucherNumber: string;
  voucherDate: string;
  narration: string;
  details: { ledgerName: string; debitAmount: number; creditAmount: number }[];
}
