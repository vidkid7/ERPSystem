# Implementation Plan: Ultimate ERP System

## Overview

This implementation plan breaks down the comprehensive Ultimate ERP System into manageable development phases. The system encompasses 16+ modules, 426 data entities, 97 API endpoints, and 721 pages using an ASP.NET Core 10 backend with a React + TypeScript frontend. The approach follows a phased implementation strategy starting with core infrastructure and progressively building specialized modules.

## Technology Stack

- **Backend**: ASP.NET Core 10, C#, Entity Framework Core, Dapper, CQRS + MediatR, FluentValidation, AutoMapper, Hangfire
- **Database & Storage**: PostgreSQL 16+ (primary), Redis (cache), Elasticsearch (search + logs), ClickHouse (analytics)
- **API Layer**: REST (ASP.NET Core), GraphQL (Hot Chocolate), SignalR (real-time), gRPC (inter-service)
- **Messaging**: MassTransit (event bus), RabbitMQ (message broker)
- **Frontend**: React + TypeScript, Ant Design, Redux Toolkit, Apache ECharts
- **Security**: Keycloak (IAM), ASP.NET Core Policies (authorization), JWT + Refresh Tokens
- **DevOps**: Docker, Kubernetes, Microsoft Azure or AWS, GitHub Actions, Terraform
- **Monitoring**: Serilog → Elasticsearch, Prometheus, Grafana, OpenTelemetry
- **Testing**: xUnit, Moq, Entity Framework InMemory provider

## Tasks

- [ ] 1. Project Infrastructure and Core Setup
  - Set up solution structure with separate projects for API, Business Logic, Data Access, and Tests
  - Configure Entity Framework Core with PostgreSQL
  - Set up CQRS + MediatR pipeline with FluentValidation and AutoMapper
  - Configure dependency injection container
  - Configure logging with Serilog → Elasticsearch and OpenTelemetry tracing
  - Set up API project with Swagger documentation
  - Configure Docker and Docker Compose for local development
  - Set up GitHub Actions CI/CD pipeline skeleton
  - _Requirements: 1.1, 1.2, 1.3, 1.4, 1.5, 1.13, 1.14, 1.15_

- [ ] 2. Database Foundation and Core Entities
  - [ ] 2.1 Create base entity classes and audit infrastructure
    - Implement BaseEntity with Id, CreatedDate, ModifiedDate, IsActive, IsDeleted
    - Create IAuditableEntity interface
    - Implement audit interceptor for automatic audit field population
    - _Requirements: 20.8, 20.9_

  - [ ] 2.2 Write unit tests for base entity infrastructure
    - Test audit field population
    - Test soft delete functionality
    - _Requirements: 21.1_

  - [ ] 2.3 Create Entity Framework DbContext and configuration
    - Implement ERP DbContext with all entity configurations
    - Configure entity relationships and constraints
    - Set up PostgreSQL connection string and Dapper for high-performance read queries
    - _Requirements: 20.1, 20.2, 20.3_

  - [ ] 2.4 Create initial database migration
    - Generate Code First migration for core entities
    - Include indexes, foreign keys, and constraints
    - _Requirements: 20.4, 20.5, 20.6_

- [ ] 3. Authentication and Security Module
  - [ ] 3.1 Implement user authentication system
    - Create User, UserGroup, and related security entities
    - Integrate Keycloak for authentication and IAM
    - Implement JWT + Refresh Token issuance and validation middleware
    - Create login/logout API endpoints delegating to Keycloak
    - _Requirements: 2.1, 2.2, 2.3, 14.1, 14.2_

  - [ ] 3.2 Implement role-based and policy-based authorization
    - Create permission system with EntityPermission, ModuleAccess entities
    - Implement ASP.NET Core Policies and authorization attributes
    - Create permission checking services
    - _Requirements: 2.4, 2.7, 14.3, 14.4, 14.5_

  - [ ] 3.3 Write unit tests for authentication and authorization
    - Test login validation
    - Test permission checking logic
    - Test authorization middleware
    - _Requirements: 21.5_

  - [ ] 3.4 Implement branch and access restrictions
    - Create BranchAccess, GodownAccess entities
    - Implement branch-wise and godown-wise filtering
    - _Requirements: 2.8, 14.6, 14.7, 14.8_

- [ ] 4. Setup Module - System Configuration
  - [ ] 4.1 Create configuration entities and services
    - Implement Branch, CostClass, DocumentType entities
    - Create configuration management services
    - Implement entity numbering system
    - _Requirements: 15.1, 15.2, 15.3, 15.4, 15.10_

  - [ ] 4.2 Create setup API endpoints
    - Implement CRUD APIs for all setup entities
    - Add validation and business rules
    - _Requirements: 18.1, 18.2, 18.3, 18.4_

  - [ ] 4.3 Write integration tests for setup APIs
    - Test CRUD operations for all setup entities
    - Test validation rules
    - _Requirements: 21.2_

- [ ] 5. Account Module - Core Financial Infrastructure
  - [ ] 5.1 Implement ledger and ledger group entities
    - Create Ledger, LedgerGroup entities with hierarchical structure
    - Implement ledger group tree operations
    - Add ledger validation and business rules
    - _Requirements: 3.1, 3.2, 3.3, 3.4, 3.5_

  - [ ] 5.2 Create voucher system infrastructure
    - Implement Voucher, VoucherDetail entities
    - Create voucher numbering service
    - Implement debit/credit balance validation
    - _Requirements: 4.1, 4.2, 4.3, 4.4, 4.5_

  - [ ] 5.3 Write unit tests for voucher validation
    - Test debit/credit balance validation
    - Test voucher numbering logic
    - Test business rule enforcement
    - _Requirements: 17.4, 17.8, 17.9, 21.1_

  - [ ] 5.4 Implement customer and vendor management
    - Create Customer, Vendor entities linked to Ledger
    - Implement customer/vendor CRUD operations
    - Add credit limit and payment terms validation
    - _Requirements: 5.1, 5.2, 5.3, 5.4, 5.5_

  - [ ] 5.5 Create account module API endpoints
    - Implement ledger CRUD APIs
    - Implement voucher CRUD APIs
    - Implement customer/vendor CRUD APIs
    - Add proper error handling and validation
    - _Requirements: 18.1, 18.2, 18.3, 18.4, 18.5_

- [ ] 6. Checkpoint - Core Foundation Complete
  - Ensure all tests pass, verify database schema is correct
  - Test authentication and basic CRUD operations
  - Ask the user if questions arise.

- [ ] 7. Inventory Module - Product and Stock Management
  - [ ] 7.1 Implement product management entities
    - Create Product, ProductGroup, Brand, Category entities
    - Implement hierarchical product groups
    - Add product validation and business rules
    - _Requirements: 6.1, 6.2, 6.3, 6.4, 6.5_

  - [ ] 7.2 Implement godown and stock management
    - Create Godown, Rack, Stock entities
    - Implement stock tracking by product, godown, and batch
    - Add stock validation and costing methods
    - _Requirements: 7.1, 7.2, 7.3, 7.4, 7.5_

  - [ ] 7.3 Write unit tests for stock calculations
    - Test FIFO, LIFO, weighted average costing
    - Test stock availability validation
    - Test negative stock prevention
    - _Requirements: 21.1_

  - [ ] 7.4 Create inventory API endpoints
    - Implement product CRUD APIs
    - Implement godown and stock APIs
    - Add stock inquiry and valuation APIs
    - _Requirements: 18.1, 18.2, 18.3_

- [ ] 8. Purchase and Sales Transaction Processing
  - [ ] 8.1 Implement purchase transaction entities
    - Create PurchaseInvoice, PurchaseInvoiceDetail entities
    - Implement purchase order and quotation entities
    - Add purchase validation and stock update logic
    - _Requirements: 8.1, 8.2, 8.3, 8.4, 8.5_

  - [ ] 8.2 Implement sales transaction entities
    - Create SalesInvoice, SalesInvoiceDetail entities
    - Implement sales order, quotation, and delivery entities
    - Add sales validation and stock reduction logic
    - _Requirements: 9.1, 9.2, 9.3, 9.4, 9.5, 9.6_

  - [ ] 8.3 Write integration tests for purchase/sales flow
    - Test complete purchase invoice to stock update flow
    - Test complete sales invoice to stock and accounting flow
    - Test stock availability validation
    - _Requirements: 17.5, 17.6, 17.10, 21.2_

  - [ ] 8.4 Create purchase and sales API endpoints
    - Implement purchase transaction APIs
    - Implement sales transaction APIs
    - Add transaction posting and approval workflows
    - _Requirements: 18.1, 18.2, 18.3, 18.4_

- [ ] 9. Financial Reporting and Account Integration
  - [ ] 9.1 Implement voucher posting service
    - Create service to post inventory transactions to accounting
    - Implement automatic voucher generation for sales/purchase
    - Add cost center allocation logic
    - _Requirements: 4.6, 4.7, 4.8_

  - [ ] 9.2 Create basic financial reports
    - Implement ledger balance calculation
    - Create trial balance report
    - Create day book report
    - _Requirements: 16.7_

  - [ ] 9.3 Write unit tests for financial calculations
    - Test ledger balance calculations
    - Test trial balance accuracy
    - Test voucher posting logic
    - _Requirements: 21.1_

- [ ] 10. HR Module Implementation
  - [ ] 10.1 Create HR entities and services
    - Implement Employee, Attendance, Leave entities
    - Create employee management services
    - Add attendance and leave calculation logic
    - _Requirements: 10.1, 10.2, 10.3, 10.4_

  - [ ] 10.2 Create HR API endpoints
    - Implement employee CRUD APIs
    - Implement attendance and leave APIs
    - Add HR reporting endpoints
    - _Requirements: 18.1, 18.2_

  - [ ] 10.3 Write unit tests for HR calculations
    - Test leave balance calculations
    - Test attendance calculations
    - _Requirements: 21.1_

- [ ] 11. Assets Module Implementation
  - [ ] 11.1 Create assets entities and services
    - Implement AssetMaster, AssetIssue, AssetTransfer entities
    - Create asset tracking services
    - Add asset lifecycle management
    - _Requirements: 11.1, 11.2, 11.3, 11.4, 11.5_

  - [ ] 11.2 Create assets API endpoints
    - Implement asset CRUD APIs
    - Implement asset issue/return APIs
    - Add asset reporting endpoints
    - _Requirements: 18.1, 18.2_

- [ ] 12. Checkpoint - Core Modules Complete
  - Ensure all core modules (Account, Inventory, HR, Assets) are working
  - Run integration tests across modules
  - Ask the user if questions arise.

- [ ] 13. Frontend Infrastructure Setup
  - [ ] 13.1 Set up React + TypeScript project structure
    - Scaffold Vite + React + TypeScript app
    - Set up Ant Design theme and global layout (Sider, Header, Content)
    - Configure Redux Toolkit store and slices
    - Set up React Router for all module routes
    - _Requirements: 19.1, 19.2_

  - [ ] 13.2 Create common frontend components
    - Implement base page templates (list, form, detail) using Ant Design
    - Create reusable form controls (Input, InputNumber, DatePicker, Select)
    - Implement Ant Design Table grid component for line items
    - _Requirements: 19.3, 19.4_

  - [ ] 13.3 Implement authentication UI
    - Create Keycloak-integrated login page with JWT handling
    - Implement Refresh Token rotation and silent refresh
    - Add logout functionality via Keycloak
    - _Requirements: 19.12, 19.13, 19.14_

- [ ] 14. Account Module Frontend
  - [ ] 14.1 Create ledger management pages
    - Implement ledger list page with search and filters
    - Create ledger form for add/edit operations
    - Add ledger group hierarchy display
    - _Requirements: 19.1, 19.3, 19.4_

  - [ ] 14.2 Create voucher entry pages
    - Implement voucher list page with filters
    - Create voucher entry form with line items grid
    - Add debit/credit validation and auto-balancing
    - _Requirements: 19.3, 19.4, 19.5_

  - [ ] 14.3 Create customer and vendor pages
    - Implement customer/vendor list pages
    - Create customer/vendor forms
    - Add contact management functionality
    - _Requirements: 19.1, 19.3_

- [ ] 15. Inventory Module Frontend
  - [ ] 15.1 Create product management pages
    - Implement product list page with advanced search
    - Create product form with all attributes
    - Add product image upload functionality
    - _Requirements: 19.1, 19.3, 19.10_

  - [ ] 15.2 Create purchase transaction pages
    - Implement purchase invoice list and form
    - Create purchase order and quotation pages
    - Add vendor lookup and product selection
    - _Requirements: 19.1, 19.3, 19.4_

  - [ ] 15.3 Create sales transaction pages
    - Implement sales invoice list and form
    - Create sales order and quotation pages
    - Add customer lookup and stock availability check
    - _Requirements: 19.1, 19.3, 19.4_

- [ ] 16. Dashboard and Reporting Frontend
  - [ ] 16.1 Create main dashboard
    - Implement dashboard with key metrics widgets
    - Add charts for income/expenses, cash flow
    - Create top products and sales analysis widgets
    - _Requirements: 13.1, 13.2, 13.3, 13.4_

  - [ ] 16.2 Create report pages
    - Implement report parameter forms
    - Create report viewer with export options
    - Add financial reports (trial balance, day book, ledger)
    - _Requirements: 16.1, 16.2, 16.3, 16.4_

  - [ ] 16.3 Create specialized module frontend pages
    - Build HMS frontend: patient registration form, OPD/IPD ticket pages, bed management grid, doctor lookup
    - Create Service module UI: complaint ticket form, job card form with technician assignment, device tracking pages, service appointment calendar
    - Build Finance/Loan frontend: loan creation form, EMI schedule view, rebate/penalty entry, loan closure page
    - Implement Task module UI: task creation form, task dashboard with status cards, monthly calendar view, assignment pages
    - Create AppCMS frontend: slider/banner management pages, notice board, gallery/video upload pages, event list management
    - Build Support module UI: ticket creation form, assignment dashboard, resolution tracking, escalation views
    - Create Loyalty module UI: membership point ledger, sales summary with points earned, redemption entry page
    - Implement Lab frontend: sample collection form, result entry page, pending reports list, final report viewer
    - Add industry-specific pages: dairy purchase/sales invoice forms with Fat%/SNF% fields, tea purchase form, petrol pump transaction and meter reading forms
    - _Requirements: 19.1, 26.1, 27.1, 28.1, 29.1, 30.1, 31.1, 32.1, 12.1, 37.1, 37.4, 37.7_

- [ ] 17. Checkpoint - Basic UI Complete
  - Test all major UI workflows
  - Verify responsive design on different screen sizes
  - Ask the user if questions arise.

- [ ] 18. Advanced Account Features
  - [ ] 18.1 Implement PDC/ODC management
    - Create PDC, ODC entities and services
    - Implement cheque tracking and status updates
    - Add PDC/ODC reports
    - _Requirements: 39.1, 39.2, 39.5_

  - [ ] 18.2 Implement bank guarantee and LC
    - Create BankGuarantee, LetterOfCredit entities
    - Add BG/LC tracking and expiry alerts
    - _Requirements: 39.3, 39.4, 39.6_

  - [ ] 18.3 Create advanced account API endpoints
    - Implement PDC/ODC APIs
    - Implement BG/LC APIs
    - Add bank reconciliation APIs
    - _Requirements: 18.1, 18.2_

- [ ] 19. Multi-Branch and Multi-Currency Support
  - [ ] 19.1 Implement multi-branch infrastructure
    - Add branch filtering to all entities
    - Implement inter-branch transactions
    - Create consolidated reporting across branches
    - _Requirements: 25.1, 25.2, 25.3, 25.4_

  - [ ] 19.2 Implement multi-currency support
    - Create Currency, ExchangeRate entities
    - Add currency conversion services
    - Implement foreign currency accounting
    - _Requirements: 25.5, 25.6, 25.7, 25.8_

  - [ ] 19.3 Write unit tests for multi-currency calculations
    - Test exchange rate conversions
    - Test currency gain/loss calculations
    - _Requirements: 21.1_

- [ ] 20. Nepal-Specific Compliance Features
  - [ ] 20.1 Implement Bikram Sambat calendar support
    - Create BS date conversion utilities
    - Add dual calendar display throughout system
    - Store both AD and BS dates for all transactions
    - _Requirements: 38.1, 38.2_

  - [ ] 20.2 Implement IRD integration infrastructure
    - Create IRD API client and data mapping services
    - Implement IRD-compliant data structures
    - Add IRD API logging
    - _Requirements: 38.3, 38.4, 38.5_

  - [ ] 20.3 Create Nepal compliance reports
    - Implement Annex 10 VAT report
    - Create excise register report
    - Add one lakh above sales/purchase reports
    - _Requirements: 38.6, 38.7, 38.8_

- [ ] 21. Lab Module Implementation
  - [ ] 21.1 Create lab entities and services
    - Implement SampleCollection, LabReport entities
    - Create lab workflow services
    - Add report template management
    - _Requirements: 12.1, 12.2, 12.3, 12.4_

  - [ ] 21.2 Create lab API endpoints and UI
    - Implement sample collection APIs
    - Create lab report generation APIs
    - Build lab management UI pages
    - _Requirements: 18.1, 19.1_

- [ ] 22. HMS (Hospital Management) Module
  - [ ] 22.1 Create HMS core entities
    - Implement Patient, OPDTicket, IPDAdmission entities
    - Create Bed, Ward, Room management entities
    - Add HMS-specific master data entities
    - _Requirements: 26.1, 26.2, 26.3, 26.4, 26.5, 26.6_

  - [ ] 22.2 Implement HMS workflows
    - Create OPD and IPD workflow services
    - Implement bed assignment and patient billing
    - Add HMS voucher integration with accounting
    - _Requirements: 26.7, 26.8, 26.9, 26.10_

  - [ ] 22.3 Create HMS API endpoints and UI
    - Implement patient management APIs
    - Create OPD/IPD transaction APIs
    - Build HMS management UI pages
    - _Requirements: 18.1, 19.1_

- [ ] 23. Service Management Module
  - [ ] 23.1 Create service entities and workflows
    - Implement ComplaintTicket, JobCard entities
    - Create service workflow and assignment logic
    - Add spare parts demand and tracking
    - _Requirements: 27.1, 27.2, 27.3, 27.4, 27.5_

  - [ ] 23.2 Create service API endpoints and UI
    - Implement complaint and job card APIs
    - Create service dashboard and reports
    - Build service management UI pages
    - _Requirements: 18.1, 19.1, 27.10, 27.11, 27.13_

- [ ] 24. Task Management Module
  - [ ] 24.1 Create task entities and services
    - Implement Task entity with assignment logic
    - Create task workflow and status tracking
    - Add task scheduling and calendar features
    - _Requirements: 28.1, 28.2, 28.3, 28.4, 28.5_

  - [ ] 24.2 Create task API endpoints and UI
    - Implement task CRUD APIs
    - Create task dashboard and calendar view
    - Build task management UI pages
    - _Requirements: 18.1, 19.1_

- [ ] 25. Finance/Loan Management Module
  - [ ] 25.1 Create loan entities and services
    - Implement Loan, LoanEMI, VehicleDetail entities
    - Create EMI calculation and scheduling services
    - Add loan workflow and closure logic
    - _Requirements: 29.1, 29.2, 29.3, 29.4, 29.5, 29.6, 29.7_

  - [ ] 25.2 Create finance API endpoints and UI
    - Implement loan management APIs
    - Create EMI processing and reporting APIs
    - Build loan management UI pages
    - _Requirements: 18.1, 19.1, 29.8_

  - [ ] 25.3 Implement banking and vehicle finance transactions
    - Create BankQuotation, BankDeliveryOrder, BankAllotment, Namsari, BankPaymentLetter, and InsuranceRecord entities
    - Implement workflows for bank-linked quotation, DO, allotment, ownership transfer, payment letters, and insurance updates
    - Add validation and status tracking integration with financed inventory and loan lifecycle
    - _Requirements: 35.1, 35.2, 35.3, 35.4, 35.5, 35.6_

  - [ ] 25.4 Create banking and vehicle finance APIs and UI
    - Implement CRUD APIs for banking/vehicle finance transaction entities
    - Build UI pages for bank quotation, bank DO, bank allotment, namsari, payment letter, and insurance records
    - Add reports for financed inventory movement and bank transaction status
    - _Requirements: 18.1, 19.1, 35.1, 35.2, 35.3, 35.4, 35.5, 35.6_

- [ ] 26. Checkpoint - Specialized Modules Complete
  - Test all specialized modules (Lab, HMS, Service, Task, Finance)
  - Verify integration with core accounting and inventory
  - Ask the user if questions arise.

- [ ] 27. AppCMS and Support Modules
  - [ ] 27.1 Create AppCMS entities and services
    - Implement Slider, Banner, Notice entities
    - Create content management services
    - Add gallery and video management
    - _Requirements: 30.1, 30.2, 30.3, 30.4, 30.5_

  - [ ] 27.2 Create Support module
    - Implement SupportTicket entity and workflow
    - Create support dashboard and assignment logic
    - _Requirements: 31.1, 31.2, 31.3, 31.4_

  - [ ] 27.3 Create AppCMS and Support APIs and UI
    - Implement content management APIs
    - Create support ticket APIs
    - Build CMS and support UI pages
    - _Requirements: 18.1, 19.1_

- [ ] 28. Loyalty and Manufacturing Modules
  - [ ] 28.1 Create loyalty module
    - Implement MembershipPoint entity and services
    - Create point accumulation and redemption logic
    - _Requirements: 32.1, 32.2, 32.3, 32.4_

  - [ ] 28.2 Create manufacturing features
    - Implement BOM, ProductionOrder entities
    - Create production workflow and stock journals
    - Add raw material consumption tracking
    - _Requirements: 33.1, 33.2, 33.3, 33.4, 33.5_

  - [ ] 28.3 Create loyalty and manufacturing APIs and UI
    - Implement loyalty program APIs
    - Create production management APIs
    - Build loyalty and manufacturing UI pages
    - _Requirements: 18.1, 19.1_

- [ ] 29. Advanced Inventory Features
  - [ ] 29.1 Implement dispatch management
    - Create DispatchOrder, DispatchSection entities
    - Add gate pass and delivery tracking
    - _Requirements: 34.1, 34.2, 34.3, 34.4_

  - [ ] 29.2 Implement fixed/serialized products
    - Add fixed product configuration and tracking
    - Implement serial number and chassis tracking
    - Create fixed product reports
    - _Requirements: 36.1, 36.2, 36.3, 36.4, 36.5_

  - [ ] 29.3 Implement industry-specific transactions
    - Create dairy purchase/sales with fat% and SNF%
    - Add tea purchase and petrol pump transactions
    - Implement meter reading for fuel dispensing
    - _Requirements: 37.1, 37.2, 37.3, 37.4, 37.5, 37.6, 37.7, 37.8_

- [ ] 30. External Integrations
  - [ ] 30.1 Implement SMS gateway integration
    - Create SMS provider abstraction and implementation
    - Add SENT configuration for automated SMS
    - Implement SMS logging and tracking
    - _Requirements: 40.1, 40.2, 40.3, 40.5_

  - [ ] 30.2 Implement payment gateway integration
    - Create payment gateway abstraction
    - Implement FonePay and SCTQR providers
    - Add payment logging and reconciliation
    - _Requirements: 40.7, 40.8, 40.9, 40.10_

  - [ ] 30.3 Implement push notification integration
    - Create OneSignal integration service
    - Add notification logging and tracking
    - _Requirements: 40.4, 40.6_

  - [ ] 30.4 Implement SSF (Social Security Fund) integration
    - Create SSF API user configuration at `/Setup/Security/SSFApiUser`
    - Implement SSF employee registration and contribution API calls
    - Add SSF contribution amount calculation per employee (employer + employee shares)
    - Implement SSF API logging and error handling with retry logic
    - Create SSF submission reports for HR department
    - _Requirements: 43.1, 43.2, 43.3_

- [ ] 31. Data Import/Export and Tally Integration
  - [ ] 31.1 Implement data import/export services
    - Create Excel import/export services
    - Add CSV and PDF export capabilities
    - Implement bulk data operations
    - _Requirements: 24.1, 24.2, 24.3, 24.4, 24.5, 24.6, 24.7, 24.8_

  - [ ] 31.2 Implement Tally integration
    - Create Tally XML parser and exporter
    - Implement master data and transaction import from Tally
    - Add Tally export functionality
    - _Requirements: 41.1, 41.2, 41.3, 41.4, 41.5_

- [ ] 32. Advanced Features and Utilities
  - [ ] 32.1 Implement KYC management
    - Create KYC form and document upload
    - Add customer verification workflow
    - _Requirements: 42.1, 42.2, 42.3, 42.4_

  - [ ] 32.2 Implement data maintenance tools
    - Create ledger merge functionality
    - Implement voucher re-numbering
    - Add product merge capabilities
    - _Requirements: 44.1, 44.2, 44.3, 44.4, 44.5, 44.6_

  - [ ] 32.3 Implement year-end closing
    - Create year-end closing process
    - Add opening balance carry-forward logic
    - Implement fiscal year locking
    - _Requirements: 45.1, 45.2, 45.3, 45.4, 45.5_

  - [ ] 32.4 Implement scheduled jobs and automation
    - Set up Hangfire job scheduler with PostgreSQL persistence and dashboard
    - Implement IRD data sync job for automated submission of sales/purchase to IRD API
    - Add SMS reminder jobs: overdue payment alerts, PDC maturity alerts, EMI due date notifications
    - Create stock reorder alert job: notify when stock falls below reorder level
    - Implement system backup automation job with configurable schedule
    - Add background job for fiscal year period status checks
    - Create job execution log viewer at `/Setup/Security/JobLog`
    - Implement job enable/disable and manual trigger from admin panel
    - _Requirements: 46.1, 46.2, 46.3, 46.4, 46.5_

- [ ] 33. Reporting and Analytics Enhancement
  - [ ] 33.1 Implement advanced financial reports
    - Create balance sheet and P&L reports
    - Add cash flow and fund flow reports
    - Implement VAT and TDS reports
    - _Requirements: 16.7, 16.11_

  - [ ] 33.2 Implement inventory and sales reports
    - Create stock aging and movement reports
    - Add sales analysis and agent performance reports
    - Implement purchase analysis reports
    - _Requirements: 16.8, 16.9, 16.10_

  - [ ] 33.3 Create report designer and query builder
    - Implement visual report designer with column/grouping/sorting configuration at `/Setup/ReportWriter/*`
    - Create ad-hoc query builder using `GetNewEntityMenu` API to list queryable entities
    - Add custom dashboard designer with drag-and-drop widget placement
    - Implement report sharing (share with all users vs. private)
    - Add PDF and Excel export for all custom reports
    - _Requirements: 47.1, 47.2, 47.4_

  - [ ] 33.4 Implement DynamicAI report generation
    - Integrate AI-powered natural language to SQL query translation
    - Implement DynamicAI dashboard widget types (chart, table, KPI card)
    - Create report generation queue for long-running queries with progress indicator
    - Build report versioning (save snapshots of generated reports)
    - Add AI-assisted report suggestion based on user role and frequently used reports
    - _Requirements: 47.3, 47.5_

  - [ ] 33.5 Implement advanced additional reports
    - Build specialized report set: Cancel Day Book, Cost Center Analysis/Summary/Breakup, Bills Receivable/Payable aging
    - Add Statistic Voucher (standard/monthly/daily), TDS-VAT register, Ledger Current Status/Daily/Ledger Analysis variants
    - Implement operational analytics reports: CR limit expired party, Cost Center Ageing, Pending PO Summary, Out-of-Stock Bill-Wise
    - Add performance and commercial reports: All Agent Sales Summary, Product Monthly Summary, Product-Wise Additional Cost, Sales Allotment Details
    - Include costing and pending-flow reports: Sales/Purchase costing voucher-wise, Pending Cannibalize-In, Sales Materialized View, Party-Wise Product Rate list
    - _Requirements: 48.1, 48.2, 48.3, 48.4, 48.5, 48.6, 48.7, 48.8, 48.9, 48.10, 48.11, 48.12, 48.13, 48.14, 48.15, 48.16, 48.17, 48.18, 48.19, 48.20_

- [ ] 34. Performance Optimization and Caching
  - [ ] 34.1 Implement caching strategy
    - Add Redis caching for reference data
    - Implement query result caching
    - Add application-level caching
    - _Requirements: 23.6_

  - [ ] 34.2 Optimize database queries
    - Add database indexes for performance
    - Implement query optimization
    - Add database connection pooling
    - _Requirements: 23.5, 23.10_

  - [ ] 34.3 Write performance tests
    - Create load tests for API endpoints
    - Test concurrent user scenarios
    - Validate performance targets
    - _Requirements: 21.6, 23.1, 23.2, 23.3, 23.4_

- [ ] 35. Error Handling and Logging Enhancement
  - [ ] 35.1 Implement comprehensive error handling
    - Add global exception handling middleware
    - Create user-friendly error messages
    - Implement error logging and tracking
    - _Requirements: 22.1, 22.2, 22.3, 22.4_

  - [ ] 35.2 Implement audit logging
    - Create comprehensive audit trail
    - Add user activity logging
    - Implement security event logging
    - _Requirements: 22.5, 22.6, 22.7, 22.8, 22.9_

- [ ] 36. Security Hardening
  - [ ] 36.1 Implement security best practices
    - Add input validation and sanitization
    - Implement CSRF protection
    - Add SQL injection prevention
    - _Requirements: 21.5_

  - [ ] 36.2 Implement advanced security features
    - Add IP address restrictions
    - Implement session timeout
    - Add password policy enforcement
    - _Requirements: 14.12, 14.13, 14.14_

- [ ] 37. Testing and Quality Assurance
  - [ ] 37.1 Complete unit test coverage
    - Achieve 80% code coverage for business logic
    - Test all calculation functions
    - Test all validation rules
    - _Requirements: 21.1_

  - [ ] 37.2 Complete integration test suite
    - Test all API endpoints
    - Test database operations
    - Test external service integrations (mocked)
    - _Requirements: 21.2_

  - [ ] 37.3 Implement end-to-end test scenarios
    - Test critical user workflows
    - Test multi-module interactions
    - Test UI functionality
    - _Requirements: 21.3_

- [ ] 38. Deployment and DevOps Setup
  - [ ] 38.1 Set up CI/CD pipeline
    - Configure build automation
    - Set up automated testing
    - Implement deployment automation
    - _Requirements: 21.1, 21.2, 21.3_

  - [ ] 38.2 Configure production environment
    - Set up production database
    - Configure application settings
    - Set up monitoring and logging
    - _Requirements: 22.7, 22.8, 22.9_

  - [ ] 38.3 Implement backup and recovery
    - Set up database backup strategy
    - Implement disaster recovery procedures
    - Test backup and restore processes
    - _Requirements: 23.9_

- [ ] 39. Final Integration and System Testing
  - [ ] 39.1 Complete system integration testing
    - Test all modules working together
    - Verify data consistency across modules
    - Test all external integrations
    - _Requirements: 21.2_

  - [ ] 39.2 Perform user acceptance testing preparation
    - Create test data sets
    - Prepare user training materials
    - Document system configuration
    - _Requirements: 21.4_

  - [ ] 39.3 Final performance and security testing
    - Conduct load testing with realistic data volumes
    - Perform security penetration testing
    - Validate all performance targets
    - _Requirements: 21.6, 23.1, 23.2, 23.3, 23.4_

- [ ] 40. Final Checkpoint - System Ready for Deployment
  - Ensure all modules are fully functional and integrated
  - Verify all tests pass and performance targets are met
  - Confirm system is ready for production deployment
  - Ask the user if questions arise.

## Notes

- All tasks are required for complete system implementation
- Each task references specific requirements for traceability
- The implementation follows a phased approach building from core infrastructure to specialized modules
- ASP.NET Core 10 (C#) technology stack is used throughout with Entity Framework Core and Dapper for data access
- React + TypeScript with Ant Design provides consistent enterprise UI/UX across all 721 pages
- Comprehensive testing strategy covers unit, integration, and end-to-end testing
- Performance optimization and security hardening are included as dedicated phases
- External integrations (IRD, SMS, Payment Gateway, SSF API) are implemented with proper abstraction
- Multi-branch and multi-currency support is built into the core architecture
- Nepal-specific compliance features (BS calendar, IRD integration, SSF) are included
- The system supports all 16+ modules with 426 entities and 97 API endpoints as specified in requirements
- Specialized module frontend pages (HMS, Service, Finance, Task, AppCMS, Support, Loyalty, Lab) are covered in Task 16.3
- Scheduled job infrastructure (Hangfire) is included in Task 32.4
- DynamicAI report generation is a dedicated sub-task (33.4) separate from the report designer (33.3)
- Industry-specific frontends (Dairy, Tea, Petrol Pump) are part of Task 16.3 frontend work

## Implementation Approach

This implementation plan follows a systematic approach:

1. **Foundation First**: Core infrastructure, authentication, and database setup
2. **Core Modules**: Account and Inventory modules as the backbone
3. **Supporting Modules**: HR, Assets, Lab modules
4. **Frontend Development**: Complete React + TypeScript SPA with Ant Design
5. **Specialized Modules**: HMS, Service, Task, Finance modules
6. **Advanced Features**: Multi-branch, multi-currency, Nepal compliance
7. **External Integrations**: SMS, Payment Gateway, IRD, Tally
8. **Quality Assurance**: Comprehensive testing and performance optimization
9. **Deployment**: Production setup and final system testing

Each phase includes checkpoints to ensure quality and allow for user feedback before proceeding to the next phase.