import React, { useState } from 'react';
import { Layout, Menu, theme, Avatar, Dropdown, Typography } from 'antd';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import {
  DashboardOutlined, AccountBookOutlined, ShopOutlined, ShoppingCartOutlined,
  DollarOutlined, TeamOutlined, MedicineBoxOutlined, ToolOutlined,
  BarChartOutlined, MenuFoldOutlined, MenuUnfoldOutlined, LogoutOutlined, UserOutlined,
  SettingOutlined, FileTextOutlined, BuildOutlined, AppstoreOutlined,
  BankOutlined, ExperimentOutlined, ReadOutlined, CustomerServiceOutlined,
  TrophyOutlined, CheckSquareOutlined,
} from '@ant-design/icons';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../../store/slices/authSlice';
import type { RootState } from '../../store';

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

const menuItems = [
  { key: '/dashboard', icon: <DashboardOutlined />, label: 'Dashboard' },
  { key: '/account', icon: <AccountBookOutlined />, label: 'Account', children: [
    { key: '/account/ledger-groups', label: 'Ledger Groups' },
    { key: '/account/ledgers', label: 'Ledgers' },
    { key: '/account/customers', label: 'Customers' },
    { key: '/account/vendors', label: 'Vendors' },
    { key: '/account/cost-centers', label: 'Cost Centers' },
    { key: '/account/salesmen', label: 'Salesmen' },
    { key: '/account/projects', label: 'Projects' },
    { key: '/account/departments', label: 'Departments' },
    { key: '/account/areas', label: 'Areas' },
    { key: '/account/narrations', label: 'Narrations' },
    { key: '/account/vouchers', label: 'Vouchers' },
    { key: '/account/vouchers/receipt', label: 'Receipt Voucher' },
    { key: '/account/vouchers/payment', label: 'Payment Voucher' },
    { key: '/account/vouchers/journal', label: 'Journal Voucher' },
    { key: '/account/vouchers/contra', label: 'Contra Voucher' },
    { key: '/account/vouchers/debit-note', label: 'Debit Note' },
    { key: '/account/vouchers/credit-note', label: 'Credit Note' },
    { key: '/account/bank-reconciliation', label: 'Bank Reconciliation' },
    { key: '/account/payment-terms', label: 'Payment Terms' },
    { key: '/account/payment-modes', label: 'Payment Modes' },
    { key: '/account/voucher-modes', label: 'Voucher Modes' },
    { key: '/account/freight-types', label: 'Freight Types' },
  ]},
  { key: '/inventory', icon: <ShopOutlined />, label: 'Inventory', children: [
    { key: '/inventory/product-groups', label: 'Product Groups' },
    { key: '/inventory/product-categories', label: 'Categories' },
    { key: '/inventory/product-types', label: 'Product Types' },
    { key: '/inventory/product-companies', label: 'Companies' },
    { key: '/inventory/product-divisions', label: 'Divisions' },
    { key: '/inventory/products', label: 'Products' },
    { key: '/inventory/brands', label: 'Brands' },
    { key: '/inventory/units', label: 'Units' },
    { key: '/inventory/fixed-units', label: 'Fixed Units' },
    { key: '/inventory/godowns', label: 'Godowns' },
    { key: '/inventory/racks', label: 'Racks' },
    { key: '/inventory/stock', label: 'Stock' },
    { key: '/inventory/sales-prices', label: 'Sales Prices' },
    { key: '/inventory/party-wise-rates', label: 'Party Wise Rates' },
    { key: '/inventory/delivery-through', label: 'Delivery Through' },
    { key: '/inventory/indents', label: 'Indents' },
    { key: '/inventory/gate-passes', label: 'Gate Passes' },
  ]},
  { key: '/purchase', icon: <ShoppingCartOutlined />, label: 'Purchase', children: [
    { key: '/purchase/quotations', label: 'Quotations' },
    { key: '/purchase/orders', label: 'Purchase Orders' },
    { key: '/purchase/receipt-notes', label: 'Receipt Notes' },
    { key: '/purchase/invoices', label: 'Purchase Invoices' },
    { key: '/purchase/returns', label: 'Purchase Returns' },
  ]},
  { key: '/sales', icon: <DollarOutlined />, label: 'Sales', children: [
    { key: '/sales/quotations', label: 'Quotations' },
    { key: '/sales/orders', label: 'Sales Orders' },
    { key: '/sales/deliveries', label: 'Deliveries' },
    { key: '/sales/invoices', label: 'Sales Invoices' },
    { key: '/sales/returns', label: 'Sales Returns' },
  ]},
  { key: '/manufacturing', icon: <AppstoreOutlined />, label: 'Manufacturing', children: [
    { key: '/manufacturing/bom', label: 'Bill of Materials' },
    { key: '/manufacturing/production-orders', label: 'Production Orders' },
    { key: '/manufacturing/stock-journals', label: 'Stock Journals' },
    { key: '/manufacturing/consumption', label: 'Consumption' },
    { key: '/manufacturing/dispatch-orders', label: 'Dispatch Orders' },
  ]},
  { key: '/assets', icon: <BuildOutlined />, label: 'Assets', children: [
    { key: '/assets/groups', label: 'Asset Groups' },
    { key: '/assets/types', label: 'Asset Types' },
    { key: '/assets/models', label: 'Asset Models' },
    { key: '/assets/categories', label: 'Asset Categories' },
    { key: '/assets/list', label: 'Assets' },
    { key: '/assets/requests', label: 'Asset Requests' },
    { key: '/assets/inward', label: 'Asset Inward' },
    { key: '/assets/issue', label: 'Asset Issue' },
    { key: '/assets/transfer', label: 'Asset Transfer' },
    { key: '/assets/return', label: 'Asset Return' },
    { key: '/assets/damage', label: 'Asset Damage' },
    { key: '/assets/disposal', label: 'Asset Disposal' },
    { key: '/assets/transactions', label: 'Transactions' },
    { key: '/assets/stock-report', label: 'Asset Stock Report' },
  ]},
  { key: '/finance', icon: <BankOutlined />, label: 'Finance', children: [
    { key: '/finance/loans', label: 'Loans' },
  ]},
  { key: '/hr', icon: <TeamOutlined />, label: 'HR', children: [
    { key: '/hr/employees', label: 'Employees' },
    { key: '/hr/attendance', label: 'Attendance' },
    { key: '/hr/leaves', label: 'Leaves' },
    { key: '/hr/religions', label: 'Religions' },
    { key: '/hr/grievance-types', label: 'Grievance Types' },
    { key: '/hr/grievances', label: 'Grievances' },
  ]},
  { key: '/hms', icon: <MedicineBoxOutlined />, label: 'HMS', children: [
    { key: '/hms/doctors', label: 'Doctors' },
    { key: '/hms/departments', label: 'Departments' },
    { key: '/hms/designations', label: 'Designations' },
    { key: '/hms/floors', label: 'Floors' },
    { key: '/hms/rooms', label: 'Rooms' },
    { key: '/hms/wards', label: 'Wards' },
    { key: '/hms/beds', label: 'Beds' },
    { key: '/hms/diagnoses', label: 'Diagnoses' },
    { key: '/hms/vitals', label: 'Vitals' },
    { key: '/hms/admission-types', label: 'Admission Types' },
    { key: '/hms/discharge-types', label: 'Discharge Types' },
    { key: '/hms/billing-types', label: 'Billing Types' },
    { key: '/hms/discount-types', label: 'Discount Types' },
    { key: '/hms/deposit-types', label: 'Deposit Types' },
    { key: '/hms/opd-ticket-types', label: 'OPD Ticket Types' },
    { key: '/hms/opd-service-types', label: 'OPD Service Types' },
    { key: '/hms/patients', label: 'Patients' },
    { key: '/hms/opd', label: 'OPD Tickets' },
    { key: '/hms/ipd', label: 'IPD Admissions' },
  ]},
  { key: '/lab', icon: <ExperimentOutlined />, label: 'Lab', children: [
    { key: '/lab/categories', label: 'Lab Categories' },
    { key: '/lab/tests', label: 'Lab Tests' },
    { key: '/lab/packages', label: 'Lab Packages' },
    { key: '/lab/technicians', label: 'Technicians' },
    { key: '/lab/specimens', label: 'Specimens' },
    { key: '/lab/containers', label: 'Containers' },
    { key: '/lab/methods', label: 'Methods' },
    { key: '/lab/lookups', label: 'Lookups' },
    { key: '/lab/samples', label: 'Samples' },
    { key: '/lab/reports', label: 'Lab Reports' },
  ]},
  { key: '/service', icon: <ToolOutlined />, label: 'Service', children: [
    { key: '/service/ticket-for', label: 'Ticket For' },
    { key: '/service/natures', label: 'Natures' },
    { key: '/service/sources', label: 'Sources' },
    { key: '/service/job-types', label: 'Job Types' },
    { key: '/service/jobcard-types', label: 'Job Card Types' },
    { key: '/service/job-service-types', label: 'Service Types' },
    { key: '/service/device-types', label: 'Device Types' },
    { key: '/service/device-models', label: 'Device Models' },
    { key: '/service/inspection-type-groups', label: 'Inspection Types' },
    { key: '/service/complaints', label: 'Complaints' },
    { key: '/service/jobcards', label: 'Job Cards' },
    { key: '/service/appointments', label: 'Appointments' },
  ]},
  { key: '/cms', icon: <ReadOutlined />, label: 'CMS', children: [
    { key: '/cms/introduction', label: 'Introduction' },
    { key: '/cms/sliders', label: 'Sliders' },
    { key: '/cms/banners', label: 'Banners' },
    { key: '/cms/gallery', label: 'Gallery' },
    { key: '/cms/videos', label: 'Videos' },
    { key: '/cms/notices', label: 'Notices' },
    { key: '/cms/event-types', label: 'Event Types' },
    { key: '/cms/events', label: 'Events' },
    { key: '/cms/product-display', label: 'Product Display' },
  ]},
  { key: '/tasks', icon: <CheckSquareOutlined />, label: 'Tasks' },
  { key: '/support', icon: <CustomerServiceOutlined />, label: 'Support', children: [
    { key: '/support/tickets', label: 'Support Tickets' },
  ]},
  { key: '/loyalty', icon: <TrophyOutlined />, label: 'Loyalty', children: [
    { key: '/loyalty/points', label: 'Points' },
  ]},
  { key: '/reports', icon: <BarChartOutlined />, label: 'Reports', children: [
    { key: '/reports/trial-balance', label: 'Trial Balance' },
    { key: '/reports/balance-sheet', label: 'Balance Sheet' },
    { key: '/reports/profit-loss', label: 'Profit & Loss' },
    { key: '/reports/cash-flow', label: 'Cash Flow' },
    { key: '/reports/day-book', label: 'Day Book' },
    { key: '/reports/cancel-day-book', label: 'Cancel Day Book' },
    { key: '/reports/cash-bank-book', label: 'Cash/Bank Book' },
    { key: '/reports/ledger-statement', label: 'Ledger Statement' },
    { key: '/reports/ledger-groups', label: 'Ledger Group Report' },
    { key: '/reports/ledgers', label: 'Ledger Report' },
    { key: '/reports/ledger-analysis', label: 'Ledger Analysis' },
    { key: '/reports/ledger-opening', label: 'Ledger Opening' },
    { key: '/reports/cost-centers', label: 'Cost Center Report' },
    { key: '/reports/bg-details', label: 'BG Details' },
    { key: '/reports/pdc', label: 'PDC Report' },
    { key: '/reports/odc', label: 'ODC Report' },
    { key: '/reports/bills-receivable', label: 'Bills Receivable' },
    { key: '/reports/bills-payable', label: 'Bills Payable' },
    { key: '/reports/vat-summary', label: 'VAT Summary' },
    { key: '/reports/tds-summary', label: 'TDS Summary' },
    { key: '/reports/patient-outstanding', label: 'Patient Outstanding' },
    { key: '/reports/stock-summary', label: 'Stock Summary' },
    { key: '/reports/stock-aging', label: 'Stock Aging' },
    { key: '/reports/product-voucher', label: 'Product Voucher' },
    { key: '/reports/product-ageing', label: 'Product Ageing' },
    { key: '/reports/near-expiry', label: 'Near Expiry' },
    { key: '/reports/stock-transfer', label: 'Stock Transfer' },
    { key: '/reports/pending-po', label: 'Pending PO' },
    { key: '/reports/sales-allotment', label: 'Sales Allotment' },
    { key: '/reports/purchase-analysis', label: 'Purchase Analysis' },
    { key: '/reports/sales-analysis', label: 'Sales Analysis' },
    { key: '/reports/sales-analysis-product', label: 'Sales by Product' },
    { key: '/reports/product-monthly-summary', label: 'Monthly Summary' },
    { key: '/reports/employee-summary', label: 'Employee Summary' },
    { key: '/reports/service-tenure', label: 'Service Tenure' },
    { key: '/reports/grievance-list', label: 'Grievance List' },
    { key: '/reports/attendance-appeals', label: 'Attendance Appeals' },
    { key: '/reports/loans', label: 'Loan Report' },
    { key: '/reports/loan-details', label: 'Loan Details' },
    { key: '/reports/loan-monthly', label: 'Loan Monthly' },
    { key: '/reports/service-reminder', label: 'Service Reminder' },
    { key: '/reports/fourth-call-log', label: '4th Call Log' },
    { key: '/reports/service-dashboard', label: 'Service Dashboard' },
  ]},
  { key: '/setup', icon: <SettingOutlined />, label: 'Setup', children: [
    { key: '/setup/company-detail', label: 'Company Detail' },
    { key: '/setup/general-config', label: 'General Config' },
    { key: '/setup/branches', label: 'Branches' },
    { key: '/setup/sub-branches', label: 'Sub Branches' },
    { key: '/setup/users', label: 'Users' },
    { key: '/setup/user-groups', label: 'User Groups' },
    { key: '/setup/ip-restrictions', label: 'IP Restrictions' },
    { key: '/setup/modules', label: 'Modules' },
    { key: '/setup/credit-rules', label: 'Credit Rules' },
    { key: '/setup/document-types', label: 'Document Types' },
    { key: '/setup/entity-numbering', label: 'Entity Numbering' },
    { key: '/setup/ird-details', label: 'IRD Details' },
    { key: '/setup/email-setup', label: 'Email Setup' },
    { key: '/setup/onesignal', label: 'OneSignal' },
    { key: '/setup/fonepay', label: 'Fonepay' },
    { key: '/setup/payment-gateway', label: 'Payment Gateway' },
  ]},
  { key: '/logs', icon: <FileTextOutlined />, label: 'Logs', children: [
    { key: '/logs/user-wise', label: 'User Wise Log' },
    { key: '/logs/login', label: 'Login Log' },
    { key: '/logs/web-api', label: 'Web API Log' },
    { key: '/logs/ird-api', label: 'IRD API Log' },
    { key: '/logs/sms', label: 'SMS Log' },
    { key: '/logs/notifications', label: 'Notification Log' },
    { key: '/logs/email', label: 'Email Log' },
    { key: '/logs/jobs', label: 'Job Log' },
    { key: '/logs/sql-errors', label: 'SQL Error Log' },
  ]},
];

const AppLayout: React.FC = () => {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();
  const location = useLocation();
  const dispatch = useDispatch();
  const user = useSelector((s: RootState) => s.auth.user);
  const { token: { colorBgContainer, borderRadiusLG } } = theme.useToken();

  const userMenu = {
    items: [
      { key: 'logout', icon: <LogoutOutlined />, label: 'Logout', onClick: () => { dispatch(logout()); navigate('/login'); } },
    ],
  };

  return (
    <Layout style={{ minHeight: '100vh' }}>
      <Sider trigger={null} collapsible collapsed={collapsed} width={240}
        style={{ overflow: 'auto', height: '100vh', position: 'fixed', left: 0, top: 0, bottom: 0 }}>
        <div style={{ height: 48, margin: 16, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
          <Text strong style={{ color: '#fff', fontSize: collapsed ? 14 : 18 }}>
            {collapsed ? 'ERP' : 'Ultimate ERP'}
          </Text>
        </div>
        <Menu theme="dark" mode="inline" selectedKeys={[location.pathname]}
          defaultOpenKeys={['/' + location.pathname.split('/')[1]]}
          items={menuItems}
          onClick={({ key }) => navigate(key)}
        />
      </Sider>
      <Layout style={{ marginLeft: collapsed ? 80 : 240, transition: 'all 0.2s' }}>
        <Header style={{ padding: '0 24px', background: colorBgContainer, display: 'flex', alignItems: 'center', justifyContent: 'space-between' }}>
          {React.createElement(collapsed ? MenuUnfoldOutlined : MenuFoldOutlined, {
            style: { fontSize: 18, cursor: 'pointer' },
            onClick: () => setCollapsed(!collapsed),
          })}
          <Dropdown menu={userMenu}>
            <div style={{ cursor: 'pointer', display: 'flex', alignItems: 'center', gap: 8 }}>
              <Avatar icon={<UserOutlined />} />
              <Text>{user?.fullName || 'User'}</Text>
            </div>
          </Dropdown>
        </Header>
        <Content style={{ margin: 24, padding: 24, background: colorBgContainer, borderRadius: borderRadiusLG, minHeight: 280 }}>
          <Outlet />
        </Content>
      </Layout>
    </Layout>
  );
};

export default AppLayout;
