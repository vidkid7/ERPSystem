import React, { useState } from 'react';
import { Layout, Menu, theme, Avatar, Dropdown, Typography } from 'antd';
import { Outlet, useNavigate, useLocation } from 'react-router-dom';
import {
  DashboardOutlined, AccountBookOutlined, ShopOutlined, ShoppingCartOutlined,
  DollarOutlined, TeamOutlined, MedicineBoxOutlined, ToolOutlined,
  BarChartOutlined, MenuFoldOutlined, MenuUnfoldOutlined, LogoutOutlined, UserOutlined,
} from '@ant-design/icons';
import { useDispatch, useSelector } from 'react-redux';
import { logout } from '../../store/slices/authSlice';
import type { RootState } from '../../store';

const { Header, Sider, Content } = Layout;
const { Text } = Typography;

const menuItems = [
  { key: '/dashboard', icon: <DashboardOutlined />, label: 'Dashboard' },
  { key: '/account', icon: <AccountBookOutlined />, label: 'Account', children: [
    { key: '/account/ledgers', label: 'Ledgers' },
    { key: '/account/vouchers', label: 'Vouchers' },
    { key: '/account/customers', label: 'Customers' },
    { key: '/account/vendors', label: 'Vendors' },
  ]},
  { key: '/inventory', icon: <ShopOutlined />, label: 'Inventory', children: [
    { key: '/inventory/products', label: 'Products' },
    { key: '/inventory/godowns', label: 'Godowns' },
    { key: '/inventory/stock', label: 'Stock' },
  ]},
  { key: '/purchase', icon: <ShoppingCartOutlined />, label: 'Purchase', children: [
    { key: '/purchase/invoices', label: 'Purchase Invoices' },
  ]},
  { key: '/sales', icon: <DollarOutlined />, label: 'Sales', children: [
    { key: '/sales/invoices', label: 'Sales Invoices' },
  ]},
  { key: '/hr', icon: <TeamOutlined />, label: 'HR', children: [
    { key: '/hr/employees', label: 'Employees' },
    { key: '/hr/attendance', label: 'Attendance' },
    { key: '/hr/leaves', label: 'Leaves' },
  ]},
  { key: '/hms', icon: <MedicineBoxOutlined />, label: 'HMS', children: [
    { key: '/hms/patients', label: 'Patients' },
    { key: '/hms/opd', label: 'OPD Tickets' },
    { key: '/hms/ipd', label: 'IPD Admissions' },
    { key: '/hms/beds', label: 'Beds' },
  ]},
  { key: '/service', icon: <ToolOutlined />, label: 'Service', children: [
    { key: '/service/complaints', label: 'Complaints' },
    { key: '/service/jobcards', label: 'Job Cards' },
    { key: '/service/appointments', label: 'Appointments' },
  ]},
  { key: '/reports', icon: <BarChartOutlined />, label: 'Reports', children: [
    { key: '/reports/trial-balance', label: 'Trial Balance' },
    { key: '/reports/day-book', label: 'Day Book' },
    { key: '/reports/ledger-statement', label: 'Ledger Statement' },
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
