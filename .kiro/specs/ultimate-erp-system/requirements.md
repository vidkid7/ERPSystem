# Requirements Document

## Introduction

This document specifies the requirements for building a comprehensive Enterprise Resource Planning (ERP) system based on the Dynamic ERP 2.0 analysis. The system encompasses 721 pages, 97 API endpoints, 426 data entities across 16+ major modules: Account, Inventory, HR, Assets Management, Lab, Dashboard, Security, Hospital (HMS), Service, Task, Finance/Loan, AppCMS, Support, Loyalty, Manufacturing/Production, and Setup. The frontend uses React + TypeScript with Ant Design, Redux Toolkit, and Apache ECharts; the backend is ASP.NET Core 10 exposing REST, GraphQL (Hot Chocolate), SignalR, and gRPC APIs secured via Keycloak. The system is IRD-certified for Nepal tax compliance and supports Bikram Sambat (BS) calendar alongside Gregorian dates.

### Source Baseline Reconciliation

- Canonical planning baseline for acceptance criteria is `ultimate_erp_report.md` metrics: 721 pages, 97 API endpoints, 426 entities, 1441 forms, 2026 tables, 18937 buttons, and 3002 modals.
- `ERP_Scrape_Report.md` contains minor crawl variance (for example, 722 pages and 18900 buttons).
- Unless explicitly overridden by a newer approved snapshot, numeric requirement targets in this document follow the canonical baseline above.

## Glossary

- **ERP_System**: The complete Enterprise Resource Planning application
- **Account_Module**: Financial accounting and ledger management subsystem
- **Inventory_Module**: Stock, product, and warehouse management subsystem
- **HR_Module**: Human resources and employee management subsystem
- **Assets_Module**: Fixed assets tracking and management subsystem
- **Lab_Module**: Laboratory sample collection and reporting subsystem
- **Dashboard_Module**: Analytics and visualization subsystem
- **Security_Module**: Authentication, authorization, and access control subsystem
- **HMS_Module**: Hospital Management System subsystem for OPD/IPD operations
- **Service_Module**: After-sales service and job card management subsystem
- **Task_Module**: Task creation, assignment, and tracking subsystem
- **Finance_Module**: Loan and vehicle finance management subsystem
- **AppCMS_Module**: Mobile application content management subsystem
- **Support_Module**: Internal and customer support ticket management subsystem
- **Loyalty_Module**: Customer loyalty points and membership management subsystem
- **API_Backend**: ASP.NET Core 10 server exposing REST, GraphQL (Hot Chocolate), SignalR, and gRPC endpoints
- **Frontend_UI**: Web-based user interface built with React + TypeScript and Ant Design component library
- **Data_Entity**: Database table or domain object
- **CRUD_Operation**: Create, Read, Update, Delete operations
- **Voucher**: Financial transaction document
- **Ledger**: Account book recording financial transactions
- **Godown**: Warehouse or storage location
- **Cost_Center**: Department or division for cost allocation
- **User_Session**: Authenticated user context represented by JWT + Refresh Token pair issued by Keycloak
- **BS_Date**: Bikram Sambat (Nepali calendar) date, also referred to as Miti
- **IRD**: Inland Revenue Department (Nepal tax authority)
- **PDC**: Post-Dated Cheque
- **ODC**: Open-Dated Cheque
- **BOM**: Bill of Materials for manufactured products
- **KYC**: Know Your Customer — regulatory identity verification process

## Requirements

### Requirement 1: System Architecture and Technology Stack

**User Story:** As a system architect, I want a well-structured multi-tier architecture, so that the system is maintainable, scalable, and follows best practices.

#### Acceptance Criteria

1. THE Frontend_UI SHALL use React + TypeScript with Ant Design, Redux Toolkit, and Apache ECharts
2. THE API_Backend SHALL provide REST endpoints (ASP.NET Core 10) following REST principles
3. THE ERP_System SHALL separate presentation, business logic, and data access layers
4. THE ERP_System SHALL use PostgreSQL as the primary relational database
5. THE API_Backend SHALL return JSON responses with consistent structure containing Data, TotalCount, IsSuccess, and ResponseMSG fields
6. THE API_Backend SHALL expose GraphQL (Hot Chocolate) for flexible data queries
7. THE API_Backend SHALL use SignalR for real-time push notifications and live data updates
8. THE API_Backend SHALL use gRPC for internal inter-service communication
9. THE ERP_System SHALL use MassTransit over RabbitMQ for asynchronous event-driven messaging
10. THE ERP_System SHALL use Keycloak for authentication and identity & access management with JWT + Refresh Tokens
11. THE ERP_System SHALL use Redis for distributed caching and Elasticsearch for full-text search and log aggregation
12. THE ERP_System SHALL use ClickHouse for high-volume analytics queries
13. THE ERP_System SHALL be containerized with Docker and orchestrated with Kubernetes on Microsoft Azure or AWS
14. THE ERP_System SHALL use GitHub Actions for CI/CD and Terraform for infrastructure as code
15. THE ERP_System SHALL use Serilog → Elasticsearch for structured logging, Prometheus + Grafana for metrics, and OpenTelemetry for distributed tracing

### Requirement 2: Authentication and Authorization System

**User Story:** As a system administrator, I want secure user authentication and role-based access control, so that only authorized users can access specific features.

#### Acceptance Criteria

1. WHEN a user submits valid credentials, THE Security_Module SHALL authenticate via Keycloak and issue JWT + Refresh Tokens
2. WHEN a user submits invalid credentials, THE Security_Module SHALL reject the login attempt and return an error message
3. THE Security_Module SHALL maintain JWT tokens and securely refresh them using Refresh Tokens
4. THE Security_Module SHALL enforce role-based and policy-based permissions (ASP.NET Core Policies) for all entities and operations
5. WHEN a JWT expires and no valid Refresh Token is present, THE Security_Module SHALL require re-authentication via Keycloak
6. THE Security_Module SHALL support user-wise module access restrictions
7. THE Security_Module SHALL support entity-level permission controls (view, create, update, delete)
8. THE Security_Module SHALL support branch-wise and godown-wise user restrictions

### Requirement 3: Account Module - Ledger Management

**User Story:** As an accountant, I want to manage ledgers with hierarchical groups, so that I can organize accounts properly.

#### Acceptance Criteria

1. THE Account_Module SHALL support creating ledgers with name, alias, code, group, and opening balance
2. THE Account_Module SHALL support hierarchical ledger groups with parent-child relationships
3. WHEN a ledger is created, THE Account_Module SHALL validate that the ledger group exists
4. THE Account_Module SHALL support ledger types: debtor, creditor, bank, cash, and general
5. THE Account_Module SHALL track ledger opening balances, debit amounts, credit amounts, and closing balances
6. THE Account_Module SHALL support ledger categories and channels for classification
7. THE Account_Module SHALL support credit limit amounts and credit limit days for debtors
8. THE Account_Module SHALL support PAN/VAT numbers, addresses, and contact information for ledgers
9. THE Account_Module SHALL support bill-wise adjustment for debtors and creditors
10. THE Account_Module SHALL support cost center allocation for ledgers

### Requirement 4: Account Module - Voucher System

**User Story:** As an accountant, I want to record financial transactions using vouchers, so that all transactions are properly documented.

#### Acceptance Criteria

1. THE Account_Module SHALL support voucher types: payment, receipt, journal, contra, sales, purchase, debit note, and credit note
2. WHEN a voucher is created, THE Account_Module SHALL generate a unique voucher number based on numbering method configuration
3. THE Account_Module SHALL support manual and automatic voucher numbering with prefix, suffix, and numerical width
4. THE Account_Module SHALL validate that debit and credit amounts balance in journal vouchers
5. THE Account_Module SHALL support voucher date, effective date, and reference number
6. THE Account_Module SHALL support common narration and line-item narration
7. THE Account_Module SHALL support cost center allocation at voucher line level
8. THE Account_Module SHALL support multi-currency transactions with exchange rates
9. THE Account_Module SHALL prevent duplicate voucher numbers when configured
10. THE Account_Module SHALL support voucher authorization workflow when enabled

### Requirement 5: Account Module - Customer and Vendor Management

**User Story:** As a sales manager, I want to manage customer and vendor information, so that I can track business relationships.

#### Acceptance Criteria

1. THE Account_Module SHALL support creating customers with name, code, address, contact details, and debtor type
2. THE Account_Module SHALL support creating vendors with name, code, address, contact details, and creditor type
3. THE Account_Module SHALL support customer routes for sales territory management
4. THE Account_Module SHALL support assigning sales agents to customers
5. THE Account_Module SHALL support customer credit limits and payment terms
6. THE Account_Module SHALL support customer pricing groups
7. THE Account_Module SHALL support customer activation and deactivation
8. THE Account_Module SHALL support pending customer approval workflow
9. THE Account_Module SHALL support customer area and cluster assignment
10. THE Account_Module SHALL support customer contact lists with multiple contacts per customer

### Requirement 6: Inventory Module - Product Management

**User Story:** As an inventory manager, I want to manage products with detailed attributes, so that I can track inventory accurately.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support creating products with name, code, product type, product group, and unit of measure
2. THE Inventory_Module SHALL support product attributes: brand, company, division, category, color, flavor, and shape
3. THE Inventory_Module SHALL support product costing methods: FIFO, LIFO, weighted average, and standard cost
4. THE Inventory_Module SHALL support multiple units of measure per product with conversion factors
5. THE Inventory_Module SHALL support product opening stock with batch and expiry date
6. THE Inventory_Module SHALL support product rack location assignment within godowns
7. THE Inventory_Module SHALL support product images and photo uploads
8. THE Inventory_Module SHALL support product pricing with multiple rate types (MRP, wholesale, retail)
9. THE Inventory_Module SHALL support product schemes and promotional pricing
10. THE Inventory_Module SHALL support Bill of Materials (BOM) for manufactured products

### Requirement 7: Inventory Module - Godown and Stock Management

**User Story:** As a warehouse manager, I want to manage multiple godowns and track stock movements, so that I can maintain accurate inventory levels.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support creating godowns with name, code, address, and parent godown for hierarchy
2. THE Inventory_Module SHALL support rack management within godowns for product location tracking
3. THE Inventory_Module SHALL support user-wise godown access restrictions
4. THE Inventory_Module SHALL track stock quantities by product, godown, and batch
5. THE Inventory_Module SHALL support stock transfer between godowns
6. THE Inventory_Module SHALL support stock journal entries for adjustments
7. THE Inventory_Module SHALL calculate stock valuation using configured costing method
8. THE Inventory_Module SHALL support stock aging analysis by product and batch
9. THE Inventory_Module SHALL support near-expiry product alerts
10. THE Inventory_Module SHALL support negative stock prevention when configured

### Requirement 8: Inventory Module - Purchase Operations

**User Story:** As a purchase officer, I want to manage the complete purchase cycle, so that I can procure goods efficiently.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support purchase quotation creation with multiple vendors
2. THE Inventory_Module SHALL support purchase order creation from quotations or directly
3. THE Inventory_Module SHALL support purchase invoice creation from purchase orders or directly
4. WHEN a purchase invoice is saved, THE Inventory_Module SHALL update stock quantities in the specified godown
5. THE Inventory_Module SHALL support purchase return with reference to original purchase invoice
6. THE Inventory_Module SHALL support purchase debit note and credit note
7. THE Inventory_Module SHALL support landed cost allocation for additional purchase costs
8. THE Inventory_Module SHALL support GRN (Goods Receipt Note) additional invoice processing
9. THE Inventory_Module SHALL track pending purchase orders and pending receipts
10. THE Inventory_Module SHALL support purchase analysis reports by vendor, product, and period

### Requirement 9: Inventory Module - Sales Operations

**User Story:** As a sales officer, I want to manage the complete sales cycle, so that I can process customer orders efficiently.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support sales quotation creation for customers
2. THE Inventory_Module SHALL support sales order creation from quotations or directly
3. THE Inventory_Module SHALL support sales allotment for order fulfillment
4. THE Inventory_Module SHALL support sales delivery note creation from allotments
5. THE Inventory_Module SHALL support sales invoice creation from delivery notes or directly
6. WHEN a sales invoice is saved, THE Inventory_Module SHALL reduce stock quantities in the specified godown
7. THE Inventory_Module SHALL support sales return with reference to original sales invoice
8. THE Inventory_Module SHALL support sales debit note and credit note
9. THE Inventory_Module SHALL support counter sales for direct cash sales
10. THE Inventory_Module SHALL track pending sales orders, pending allotments, and pending deliveries
11. THE Inventory_Module SHALL support sales analysis reports by customer, product, agent, and period

### Requirement 10: HR Module - Employee Management

**User Story:** As an HR manager, I want to manage employee information and attendance, so that I can track workforce data.

#### Acceptance Criteria

1. THE HR_Module SHALL support creating employee records with personal details, designation, and department
2. THE HR_Module SHALL support employee bank account details for salary processing
3. THE HR_Module SHALL support employee leave opening balances and leave types
4. THE HR_Module SHALL support manual attendance entry and attendance appeals
5. THE HR_Module SHALL support employee expense claims with approval workflow
6. THE HR_Module SHALL support employee grievance management with grievance types
7. THE HR_Module SHALL generate employee summary reports
8. THE HR_Module SHALL generate employee yearly attendance reports
9. THE HR_Module SHALL generate service tenure reports
10. THE HR_Module SHALL support travel mode configuration for expense claims

### Requirement 11: Assets Module - Fixed Assets Management

**User Story:** As an assets manager, I want to track fixed assets throughout their lifecycle, so that I can manage company assets properly.

#### Acceptance Criteria

1. THE Assets_Module SHALL support creating asset masters with code, name, asset type, asset group, and asset category
2. THE Assets_Module SHALL support asset opening balances with purchase date and purchase value
3. THE Assets_Module SHALL support asset inward for new asset acquisitions
4. THE Assets_Module SHALL support asset issue to employees with issue date
5. THE Assets_Module SHALL support asset return from employees
6. THE Assets_Module SHALL support asset transfer between employees or locations
7. THE Assets_Module SHALL support asset damage recording
8. THE Assets_Module SHALL support asset repair tracking with repaired inward
9. THE Assets_Module SHALL generate asset stock reports by type, group, and category
10. THE Assets_Module SHALL generate employee-wise asset details reports
11. THE Assets_Module SHALL generate vendor-wise asset reports
12. THE Assets_Module SHALL track asset request and approval workflow

### Requirement 12: Lab Module - Sample Collection and Reporting

**User Story:** As a lab technician, I want to manage sample collection and generate lab reports, so that I can process lab tests efficiently.

#### Acceptance Criteria

1. THE Lab_Module SHALL support sample collection entry with patient details and test parameters
2. THE Lab_Module SHALL support lab report template configuration with header and footer
3. THE Lab_Module SHALL generate pending reports list for tests awaiting results
4. THE Lab_Module SHALL generate final reports with test results and reference ranges
5. THE Lab_Module SHALL support entity numbering system for sample IDs
6. THE Lab_Module SHALL support multiple test parameters per sample
7. THE Lab_Module SHALL support report printing with configured templates

### Requirement 13: Dashboard Module - Analytics and Visualization

**User Story:** As a business manager, I want to view key performance indicators on dashboards, so that I can monitor business performance.

#### Acceptance Criteria

1. THE Dashboard_Module SHALL display income and expenses by month for the current fiscal year
2. THE Dashboard_Module SHALL display cash flow by month
3. THE Dashboard_Module SHALL display top-selling products
4. THE Dashboard_Module SHALL display sales metrics by agent and territory
5. THE Dashboard_Module SHALL display inventory aging analysis
6. THE Dashboard_Module SHALL display party aging for receivables and payables
7. THE Dashboard_Module SHALL support product brand summary visualization
8. THE Dashboard_Module SHALL support non-moving items analysis
9. THE Dashboard_Module SHALL support custom dashboard designer for user-defined dashboards
10. THE Dashboard_Module SHALL refresh dashboard data in real-time or on-demand

### Requirement 14: Security Module - User and Permission Management

**User Story:** As a system administrator, I want to manage users and their permissions, so that I can control system access.

#### Acceptance Criteria

1. THE Security_Module SHALL support creating users with username, password, user type, and branch assignment
2. THE Security_Module SHALL support user groups for role-based access control
3. THE Security_Module SHALL support module-wise access control per user
4. THE Security_Module SHALL support entity-wise permission control (view, create, update, delete, report)
5. THE Security_Module SHALL support voucher-wise access control for accounting vouchers
6. THE Security_Module SHALL support branch-wise user restrictions
7. THE Security_Module SHALL support godown-wise user restrictions
8. THE Security_Module SHALL support cost center-wise user restrictions
9. THE Security_Module SHALL support ledger group-wise user restrictions
10. THE Security_Module SHALL support product group-wise user restrictions
11. THE Security_Module SHALL support copying security settings from one user to another
12. THE Security_Module SHALL support IP address restrictions for user access
13. THE Security_Module SHALL log all user login attempts and activities
14. THE Security_Module SHALL support password change and password reset functionality

### Requirement 15: Setup Module - System Configuration

**User Story:** As a system administrator, I want to configure system settings, so that the system behaves according to business requirements.

#### Acceptance Criteria

1. THE ERP_System SHALL support account configuration with fiscal year, currency, and accounting preferences
2. THE ERP_System SHALL support inventory configuration with costing method, negative stock control, and batch tracking
3. THE ERP_System SHALL support general configuration with date format, number format, and company details
4. THE ERP_System SHALL support branch management with multiple branches
5. THE ERP_System SHALL support document type configuration for voucher categorization
6. THE ERP_System SHALL support email setup for system notifications
7. THE ERP_System SHALL support payment gateway integration configuration
8. THE ERP_System SHALL support IRD (tax authority) integration configuration
9. THE ERP_System SHALL support custom field (UDF) definition for entities
10. THE ERP_System SHALL support entity numbering system configuration with prefix, suffix, and auto-numbering

### Requirement 16: Reporting System

**User Story:** As a business user, I want to generate comprehensive reports, so that I can analyze business data.

#### Acceptance Criteria

1. THE ERP_System SHALL support report template designer for custom report layouts
2. THE ERP_System SHALL support query builder for ad-hoc report creation
3. THE ERP_System SHALL support report parameters for filtering data
4. THE ERP_System SHALL support report export to PDF, Excel, and CSV formats
5. THE ERP_System SHALL support report scheduling for automated generation
6. THE ERP_System SHALL support report email delivery
7. THE ERP_System SHALL generate financial reports: balance sheet, profit and loss, trial balance, day book, ledger voucher
8. THE ERP_System SHALL generate inventory reports: stock summary, product voucher, stock aging, near expiry
9. THE ERP_System SHALL generate sales reports: sales analysis, sales by agent, sales by product, pending orders
10. THE ERP_System SHALL generate purchase reports: purchase analysis, purchase by vendor, purchase by product, pending orders
11. THE ERP_System SHALL generate VAT/tax reports: purchase VAT register, sales VAT register, VAT summary, TDS summary

### Requirement 17: Data Validation and Business Rules

**User Story:** As a system user, I want the system to validate data entry, so that data integrity is maintained.

#### Acceptance Criteria

1. WHEN a user creates an entity, THE ERP_System SHALL validate that all required fields are provided
2. WHEN a user enters a code, THE ERP_System SHALL validate that the code is unique within the entity type
3. WHEN a user selects a date, THE ERP_System SHALL validate that the date is within the active cost class period
4. WHEN a user creates a voucher, THE ERP_System SHALL validate that the voucher date is not a backdated entry if backdate entry is disabled
5. WHEN a user saves a sales invoice, THE ERP_System SHALL validate that sufficient stock is available if negative stock is disabled
6. WHEN a user saves a transaction, THE ERP_System SHALL validate that the user has permission for the selected branch and godown
7. WHEN a user enters a credit transaction for a customer, THE ERP_System SHALL validate credit limit if credit rules are enforced
8. WHEN a user enters a numeric value, THE ERP_System SHALL validate that the value is within the configured decimal places
9. WHEN a user selects a ledger, THE ERP_System SHALL validate that the ledger is active and not blocked
10. WHEN a user deletes a record, THE ERP_System SHALL validate that the record is not referenced by other transactions

### Requirement 18: API Endpoint Implementation

**User Story:** As a frontend developer, I want well-documented RESTful APIs, so that I can integrate the frontend with the backend.

#### Acceptance Criteria

1. THE API_Backend SHALL implement all 97 discovered API endpoints with consistent URL patterns
2. THE API_Backend SHALL support GET requests for retrieving data lists and single records
3. THE API_Backend SHALL support POST requests for creating records and complex queries
4. THE API_Backend SHALL support PUT or POST requests for updating records
5. THE API_Backend SHALL support DELETE requests for removing records
6. THE API_Backend SHALL return HTTP 200 for successful requests
7. THE API_Backend SHALL return HTTP 400 for validation errors with descriptive error messages
8. THE API_Backend SHALL return HTTP 401 for unauthorized requests
9. THE API_Backend SHALL return HTTP 403 for forbidden requests (insufficient permissions)
10. THE API_Backend SHALL return HTTP 500 for server errors with error logging
11. THE API_Backend SHALL support pagination for list endpoints with page number and page size parameters
12. THE API_Backend SHALL support sorting for list endpoints with sort field and sort direction parameters
13. THE API_Backend SHALL support filtering for list endpoints with filter criteria parameters

### Requirement 19: Frontend UI Implementation

**User Story:** As an end user, I want an intuitive and responsive user interface, so that I can use the system efficiently.

#### Acceptance Criteria

1. THE Frontend_UI SHALL implement all 721 pages identified in the analysis
2. THE Frontend_UI SHALL use Ant Design Layout with sidebar navigation and top navbar
3. THE Frontend_UI SHALL implement all 1441 forms with proper field types and validation (Ant Design Form + FluentValidation-backed API)
4. THE Frontend_UI SHALL implement all 2026 tables with sorting, filtering, and pagination (Ant Design Table)
5. THE Frontend_UI SHALL implement all 18937 buttons with appropriate actions
6. THE Frontend_UI SHALL implement all 3002 modals for dialogs and popups (Ant Design Modal)
7. THE Frontend_UI SHALL use Ant Design Select for dropdown fields with search capability
8. THE Frontend_UI SHALL use Ant Design Modal for confirmation dialogs and Ant Design notification for success/error toasts
9. THE Frontend_UI SHALL use date-fns or dayjs for date formatting and manipulation
10. THE Frontend_UI SHALL use Ant Design Input with masking for formatted input fields (phone, currency, date)
11. THE Frontend_UI SHALL be responsive and work on desktop, tablet, and mobile devices
12. THE Frontend_UI SHALL display loading indicators during API calls
13. THE Frontend_UI SHALL display error messages for failed operations
14. THE Frontend_UI SHALL display success messages for completed operations
15. THE Frontend_UI SHALL use Apache ECharts for all charts and data visualizations on dashboard and report pages
16. THE Frontend_UI SHALL use Redux Toolkit for global state management across all modules

### Requirement 20: Database Schema Implementation

**User Story:** As a database administrator, I want a well-designed database schema, so that data is stored efficiently and relationships are maintained.

#### Acceptance Criteria

1. THE ERP_System SHALL implement database tables for all 426 identified data entities
2. THE ERP_System SHALL implement primary keys for all tables
3. THE ERP_System SHALL implement foreign keys for all relationships between tables
4. THE ERP_System SHALL implement indexes on frequently queried columns
5. THE ERP_System SHALL implement unique constraints on code fields
6. THE ERP_System SHALL implement check constraints for data validation
7. THE ERP_System SHALL implement default values for common fields (IsActive, CreatedDate)
8. THE ERP_System SHALL implement audit fields (CreatedBy, CreatedDate, ModifiedBy, ModifiedDate) on all transactional tables
9. THE ERP_System SHALL implement soft delete using IsDeleted flag instead of physical deletion
10. THE ERP_System SHALL implement database views for complex queries and reporting

### Requirement 21: Testing Strategy

**User Story:** As a quality assurance engineer, I want comprehensive testing coverage, so that the system is reliable and bug-free.

#### Acceptance Criteria

1. THE ERP_System SHALL have unit tests for all business logic functions with minimum 80% code coverage
2. THE ERP_System SHALL have integration tests for all API endpoints
3. THE ERP_System SHALL have end-to-end tests for critical user workflows (login, create voucher, create invoice)
4. THE ERP_System SHALL have validation tests for all form inputs
5. THE ERP_System SHALL have security tests for authentication and authorization
6. THE ERP_System SHALL have performance tests for high-volume operations (bulk import, report generation)
7. THE ERP_System SHALL have database tests for data integrity and constraints
8. THE ERP_System SHALL have UI tests for all pages and user interactions
9. THE ERP_System SHALL have API contract tests to ensure frontend-backend compatibility
10. THE ERP_System SHALL have regression tests to prevent breaking existing functionality

### Requirement 22: Error Handling and Logging

**User Story:** As a system administrator, I want comprehensive error handling and logging, so that I can troubleshoot issues quickly.

#### Acceptance Criteria

1. WHEN an error occurs, THE ERP_System SHALL log the error with timestamp, user, module, and stack trace
2. WHEN a validation error occurs, THE ERP_System SHALL return a user-friendly error message
3. WHEN a database error occurs, THE ERP_System SHALL log the SQL error and return a generic error message to the user
4. WHEN an API call fails, THE ERP_System SHALL log the request and response details
5. THE ERP_System SHALL log all user login attempts with success or failure status
6. THE ERP_System SHALL log all data modification operations (create, update, delete) with before and after values
7. THE ERP_System SHALL provide an error log viewer in the admin interface
8. THE ERP_System SHALL provide a login log viewer in the admin interface
9. THE ERP_System SHALL provide an audit log viewer for tracking data changes
10. THE ERP_System SHALL support log retention policies with automatic archival of old logs

### Requirement 23: Performance and Scalability

**User Story:** As a system architect, I want the system to perform well under load, so that users have a responsive experience.

#### Acceptance Criteria

1. WHEN a user loads a list page, THE ERP_System SHALL return results within 2 seconds for up to 10,000 records
2. WHEN a user saves a transaction, THE ERP_System SHALL complete the save operation within 1 second
3. WHEN a user generates a report, THE ERP_System SHALL generate the report within 5 seconds for up to 100,000 records
4. THE ERP_System SHALL support at least 100 concurrent users without performance degradation
5. THE ERP_System SHALL implement database query optimization with proper indexes
6. THE ERP_System SHALL implement caching for frequently accessed reference data
7. THE ERP_System SHALL implement lazy loading for large data sets
8. THE ERP_System SHALL implement pagination for all list views
9. THE ERP_System SHALL implement asynchronous processing for long-running operations
10. THE ERP_System SHALL implement database connection pooling for efficient resource usage

### Requirement 24: Data Import and Export

**User Story:** As a data administrator, I want to import and export data, so that I can migrate data and integrate with other systems.

#### Acceptance Criteria

1. THE ERP_System SHALL support importing master data (ledgers, products, customers) from Excel files
2. THE ERP_System SHALL support importing opening balances from Excel files
3. THE ERP_System SHALL support importing transactions from Excel files
4. THE ERP_System SHALL validate imported data and report errors with row numbers
5. THE ERP_System SHALL support exporting data to Excel format
6. THE ERP_System SHALL support exporting data to CSV format
7. THE ERP_System SHALL support exporting data to PDF format
8. THE ERP_System SHALL support bulk data operations for efficient processing
9. THE ERP_System SHALL support data templates for import operations
10. THE ERP_System SHALL log all import and export operations with user and timestamp

### Requirement 25: Multi-Branch and Multi-Currency Support

**User Story:** As a business owner, I want to manage multiple branches and currencies, so that I can operate in different locations and markets.

#### Acceptance Criteria

1. THE ERP_System SHALL support multiple branches with separate inventory and accounting
2. THE ERP_System SHALL support inter-branch stock transfers
3. THE ERP_System SHALL support inter-branch financial transactions
4. THE ERP_System SHALL support consolidated reporting across all branches
5. THE ERP_System SHALL support multiple currencies with exchange rate management
6. THE ERP_System SHALL support currency-wise transactions and reporting
7. THE ERP_System SHALL support base currency and foreign currency accounting
8. THE ERP_System SHALL support exchange rate gain/loss calculation
9. THE ERP_System SHALL support branch-wise user access restrictions
10. THE ERP_System SHALL support branch-wise configuration settings

### Requirement 26: Hospital Management Module (HMS)

**User Story:** As a hospital administrator, I want to manage in-patient and out-patient operations, so that I can run hospital workflows efficiently.

#### Acceptance Criteria

1. THE HMS_Module SHALL support master data for OPD Ticket Types and OPD Service Types
2. THE HMS_Module SHALL support department, designation, admission type, discharge type, and billing type masters
3. THE HMS_Module SHALL support doctor, diagnosis, vital signs, and service masters
4. THE HMS_Module SHALL support discount types, deposit types, commission types, and discount-commission mappings
5. THE HMS_Module SHALL support hospital infrastructure masters: building type, floor, room, ward, bed, and bed mapping
6. THE HMS_Module SHALL support patient demographic masters: ethnicity, disability, and donor records
7. THE HMS_Module SHALL support OPD (Out-Patient Department) transactions with patient registration, ticketing, and billing
8. THE HMS_Module SHALL support IPD (In-Patient Department) transactions with admission, bed assignment, and discharge
9. THE HMS_Module SHALL support HMS-specific voucher configuration
10. THE HMS_Module SHALL support cash deposit for patient advance payments
11. THE HMS_Module SHALL support patient Dr/Cr (debit/credit) adjustments in accounts
12. THE HMS_Module SHALL generate patient outstanding reports

### Requirement 27: Service Management Module

**User Story:** As a service manager, I want to manage customer complaints, job cards, and after-sales service, so that I can track and resolve service requests efficiently.

#### Acceptance Criteria

1. THE Service_Module SHALL support complaint ticket creation and tracking
2. THE Service_Module SHALL support job card creation, assignment, and closure for technicians
3. THE Service_Module SHALL support device entry with device type, model, and color masters
4. THE Service_Module SHALL support inspection types and inspection type groups for complaint inspection
5. THE Service_Module SHALL support spare parts demand, issuance, and return against job cards
6. THE Service_Module SHALL support job type, job card type, job service type, and job type mapping masters
7. THE Service_Module SHALL support service appointment scheduling
8. THE Service_Module SHALL support service member management and assignment
9. THE Service_Module SHALL support ticket categorization masters: ticket-for, nature, source
10. THE Service_Module SHALL generate reports: job card list, vehicle list, service spare parts, complain inspection list, vehicle history, and job card status
11. THE Service_Module SHALL generate call log reports (fourth, fifth, and sixth call follow-up logs)
12. THE Service_Module SHALL generate service reminder reports for pending follow-ups
13. THE Service_Module SHALL provide a dedicated service dashboard with key metrics

### Requirement 28: Task Management Module

**User Story:** As a team manager, I want to create, assign, and track tasks, so that I can monitor team progress and workload.

#### Acceptance Criteria

1. THE Task_Module SHALL support task type configuration for categorizing tasks
2. THE Task_Module SHALL support creating tasks with title, description, assignee, due date, and task type
3. THE Task_Module SHALL support viewing all tasks with status filters (pending, in-progress, completed)
4. THE Task_Module SHALL support monthly task scheduling and calendar view
5. THE Task_Module SHALL support task status updates and progress tracking

### Requirement 29: Finance and Loan Management Module

**User Story:** As a finance officer, I want to manage loans, vehicle finance, and EMI schedules, so that I can track lending and repayment efficiently.

#### Acceptance Criteria

1. THE Finance_Module SHALL support finance configuration for interest rate and loan product setup
2. THE Finance_Module SHALL support vehicle detail registration for vehicle-linked loans
3. THE Finance_Module SHALL support loan creation with borrower, principal, interest rate, tenure, and disbursement date
4. THE Finance_Module SHALL support month-end processing to generate EMI schedules
5. THE Finance_Module SHALL support rebate and penalty calculation for early/late payments
6. THE Finance_Module SHALL support loan rescheduling for restructured repayment plans
7. THE Finance_Module SHALL support loan closure with final settlement calculation
8. THE Finance_Module SHALL generate loan creation, loan details, and monthly loan reports

### Requirement 30: Mobile App CMS Module

**User Story:** As a marketing manager, I want to manage content displayed on the mobile app, so that customers see relevant products, promotions, and announcements.

#### Acceptance Criteria

1. THE AppCMS_Module SHALL support product display configuration for e-commerce app listing
2. THE AppCMS_Module SHALL support slider management for home screen promotional banners
3. THE AppCMS_Module SHALL support banner and notice publishing with effective dates
4. THE AppCMS_Module SHALL support gallery and video content management
5. THE AppCMS_Module SHALL support company introduction and services-and-facilities content
6. THE AppCMS_Module SHALL support academic calendar management for educational institutions
7. THE AppCMS_Module SHALL support event type and event list management
8. THE AppCMS_Module SHALL support weekend configuration for scheduling and calendar features

### Requirement 31: Customer Support Module

**User Story:** As a support executive, I want to manage internal and customer support tickets, so that issues are tracked and resolved in a timely manner.

#### Acceptance Criteria

1. THE Support_Module SHALL support creating support tickets with subject, description, priority, and assignee
2. THE Support_Module SHALL support managing support executives and their assignments
3. THE Support_Module SHALL provide a support dashboard showing open, in-progress, and closed tickets
4. THE Support_Module SHALL track ticket history and resolution notes

### Requirement 32: Loyalty and Membership Module

**User Story:** As a loyalty program manager, I want to track customer loyalty points and membership, so that I can reward repeat customers.

#### Acceptance Criteria

1. THE Loyalty_Module SHALL support membership point accumulation based on sales transactions
2. THE Loyalty_Module SHALL support point redemption against future purchases
3. THE Loyalty_Module SHALL generate sales summary reports for loyalty program analysis
4. THE Loyalty_Module SHALL generate membership point summary reports per customer

### Requirement 33: Manufacturing and Production Module

**User Story:** As a production manager, I want to manage production orders, raw material consumption, and stock movements, so that I can track manufacturing operations end-to-end.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support production order creation linked to BOM with planned and actual quantities
2. THE Inventory_Module SHALL support stock journal BOM for BOM-driven stock adjustments
3. THE Inventory_Module SHALL support manufacturing stock journal for recording production outputs
4. THE Inventory_Module SHALL support raw material consumption entries against production orders
5. THE Inventory_Module SHALL support indent (material requisition) creation and approval
6. THE Inventory_Module SHALL support stock demand entries for inter-department material requests
7. THE Inventory_Module SHALL support CannibalizeIn and CannibalizeOut transactions for component harvesting from end-of-life items
8. THE Inventory_Module SHALL support GRN additional invoice, purchase additional invoice, and production additional invoice for supplementary cost entries
9. THE Inventory_Module SHALL generate production order reports and pending production order lists
10. THE Inventory_Module SHALL generate consumption list and pending cannibalize-in reports

### Requirement 34: Dispatch Management Module

**User Story:** As a dispatch officer, I want to manage outgoing dispatch orders and dispatch sections, so that I can track goods delivery to customers.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support dispatch order creation referencing sales orders or delivery notes
2. THE Inventory_Module SHALL support dispatch section management for organizing shipments by route or vehicle
3. THE Inventory_Module SHALL support gate pass generation for goods leaving the premises
4. THE Inventory_Module SHALL track pending dispatch orders and delivery status

### Requirement 35: Banking and Vehicle Finance Transactions

**User Story:** As a banking operations officer, I want to process bank-linked quotations, delivery orders, allotments, and insurance, so that I can manage vehicle financing workflows.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support bank quotation creation for vehicle or asset financing
2. THE Inventory_Module SHALL support bank delivery order (DO) processing
3. THE Inventory_Module SHALL support bank allotment for linking financed inventory to customers
4. THE Inventory_Module SHALL support Namsari (ownership transfer) documentation
5. THE Inventory_Module SHALL support bank payment letter generation
6. THE Inventory_Module SHALL support insurance record management for financed assets

### Requirement 36: Fixed / Serialized Product Management

**User Story:** As an inventory manager, I want to track serialized and high-value products (vehicles, equipment) with unique identifiers, so that I can maintain accurate individual product records.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support fixed product configuration with configurable display fields: Registration No, Chassis No, Engine No, Model, Type, Color, Key No, Code No, and MFG Year
2. THE Inventory_Module SHALL track fixed products individually by serial/chassis number
3. THE Inventory_Module SHALL generate fixed product list reports, fixed product report by product voucher, and fixed product in/out details
4. THE Inventory_Module SHALL generate fixed product selling rate reports
5. THE Inventory_Module SHALL support fixed unit configuration for standardized fixed product units

### Requirement 37: Industry-Specific Transaction Modules

**User Story:** As a business owner in a specialized industry, I want industry-specific transaction workflows, so that I can process transactions suited to my business type.

#### Acceptance Criteria

1. THE Inventory_Module SHALL support Dairy Purchase Invoice for milk and dairy product procurement with quantity, fat%, SNF%, and rate per litre
2. THE Inventory_Module SHALL support Dairy Sales Invoice for dairy product sales
3. THE Inventory_Module SHALL support Dairy Purchase Setup for configuring dairy-specific rates and parameters
4. THE Inventory_Module SHALL support Tea Purchase Invoice for tea procurement
5. THE Inventory_Module SHALL support Petrol Pump Order and Petrol Pump Delivery transactions
6. THE Inventory_Module SHALL support Petrol Pump (POS) transaction for fueling
7. THE Inventory_Module SHALL support Meter Reading entry for tracking fuel dispensed per pump nozzle
8. THE Inventory_Module SHALL generate Dairy Purchase Report and Dairy Sales Report

### Requirement 38: Nepali Calendar and Nepal Regulatory Compliance

**User Story:** As a user in Nepal, I want the system to support Bikram Sambat (BS) calendar and Nepal tax authority compliance, so that all transactions, dates, and reports meet local regulatory requirements.

#### Acceptance Criteria

1. THE ERP_System SHALL support dual calendar display: Gregorian (AD) and Bikram Sambat (BS/Miti) for all date fields
2. THE ERP_System SHALL store both AD and BS dates for all transactional records
3. THE ERP_System SHALL be IRD (Inland Revenue Department, Nepal) certified and comply with IRD real-time reporting requirements
4. THE ERP_System SHALL support IRD API integration for submitting sales/purchase data to the IRD portal
5. THE ERP_System SHALL log all IRD API calls and responses in the IRD API log
6. THE ERP_System SHALL generate Annex 10 report for VAT compliance filing
7. THE ERP_System SHALL generate Excise Register for excise duty tracking
8. THE ERP_System SHALL generate One Lakh Above Sales and One Lakh Above Purchase reports for transactions exceeding NPR 100,000
9. THE ERP_System SHALL support SSF (Social Security Fund) API user configuration for SSF integration
10. THE ERP_System SHALL generate IRD-compliant Stock Summary report

### Requirement 39: Cheque, PDC, ODC, Bank Guarantee, and Letter of Credit

**User Story:** As an accounts manager, I want to manage post-dated cheques, open-dated cheques, bank guarantees, and letters of credit, so that I can track all instrument-based financial obligations.

#### Acceptance Criteria

1. THE Account_Module SHALL support PDC (Post-Dated Cheque) details entry with cheque number, date, bank, and amount
2. THE Account_Module SHALL support ODC (Open-Dated Cheque) details entry
3. THE Account_Module SHALL support Bank Guarantee (BG) details with guarantee amount, validity, and issuing bank
4. THE Account_Module SHALL support Letter of Credit (LC) details with LC number, value, expiry, and linked vendor
5. THE Account_Module SHALL generate Post-Dated Cheque report and Open-Dated Cheque report
6. THE Account_Module SHALL generate Bank Guarantee Details report
7. THE Account_Module SHALL support Bank Reconciliation for matching bank statements with ledger entries
8. THE Account_Module SHALL display BG amount, PDC amount, and ODC amount on the ledger detail summary

### Requirement 40: SMS, Push Notification, and Payment Gateway Integration

**User Story:** As a system administrator, I want to configure SMS, push notification, and payment gateway integrations, so that customers and staff receive automated notifications and payments are processed electronically.

#### Acceptance Criteria

1. THE ERP_System SHALL support SENT (SMS notification) configuration for automated SMS alerts on transactions
2. THE ERP_System SHALL support SENT Voucher configuration to trigger SMS on specific voucher types
3. THE ERP_System SHALL support SENT Custom rules for custom SMS triggers
4. THE ERP_System SHALL support OneSignal push notification setup for mobile app notifications
5. THE ERP_System SHALL log all SMS API calls in the SMS API log
6. THE ERP_System SHALL log all push notification events in the Notification log
7. THE ERP_System SHALL support FonePay QR code payment integration for digital payments
8. THE ERP_System SHALL support SCTQR payment integration
9. THE ERP_System SHALL support general payment gateway configuration for third-party payment processors
10. THE ERP_System SHALL log all Web API integration calls in the Web API log

### Requirement 41: Tally Integration

**User Story:** As a data migration specialist, I want to import data from Tally ERP and export data to Tally, so that businesses switching from or integrating with Tally can migrate smoothly.

#### Acceptance Criteria

1. THE ERP_System SHALL support importing master data (ledgers, products, parties) from Tally XML format
2. THE ERP_System SHALL support importing transactions from Tally XML format
3. THE ERP_System SHALL support exporting data in Tally-compatible format via TallyTran module
4. THE ERP_System SHALL support Import/Export Transaction (ImportExportTran) for flexible data interchange
5. THE ERP_System SHALL validate and report import errors with record-level details

### Requirement 42: KYC (Know Your Customer) Management

**User Story:** As a compliance officer, I want to collect and manage KYC documents for customers, so that the business meets regulatory due diligence requirements.

#### Acceptance Criteria

1. THE Account_Module SHALL support KYC form entry with customer personal details and document uploads
2. THE KYC form SHALL capture: name, address, contact, identification type, identification number, and document scans (photo, ID front, ID back, supporting documents)
3. THE Account_Module SHALL support customer deactivation and pending customer approval workflows
4. THE Account_Module SHALL support pending location management for customers awaiting location verification

### Requirement 43: Advanced Account Master Features

**User Story:** As an accounts manager, I want to manage salesman targets, ledger targets, advanced opening balances, and authorized transaction controls, so that I can plan and control financial performance.

#### Acceptance Criteria

1. THE Account_Module SHALL support salesman target configuration by period with target amount and product
2. THE Account_Module SHALL support ledger target configuration by period
3. THE Account_Module SHALL support bill-wise ledger opening balance entry for outstanding receivables/payables
4. THE Account_Module SHALL support TDS ledger opening balance entry for TDS credits carried forward
5. THE Account_Module SHALL support branch-wise ledger opening balance for multi-branch deployments
6. THE Account_Module SHALL support cost center ledger opening balance entry
7. THE Account_Module SHALL support receipt entry master for configuring receipt transaction defaults
8. THE Account_Module SHALL support Ledger Authorized and Voucher Authorized configuration to restrict which users can post to specific ledgers or vouchers
9. THE Account_Module SHALL support TranStatus master for transaction status workflow management
10. THE Account_Module SHALL support incoming payment, outgoing payment, and vendor payment transaction types in addition to standard receipt and payment

### Requirement 44: Data Maintenance and Re-organization

**User Story:** As a database administrator, I want tools to merge duplicate records and re-number vouchers, so that data consistency and voucher sequencing can be maintained.

#### Acceptance Criteria

1. THE Account_Module SHALL support Ledger Merge to combine duplicate ledger accounts into a single ledger
2. THE Account_Module SHALL support Route Merge to combine duplicate debtor routes
3. THE Inventory_Module SHALL support Product Merge to combine duplicate product records
4. THE Account_Module SHALL support Re-Voucher Numbering to reassign sequential voucher numbers within a period
5. WHEN a merge operation is performed, THE ERP_System SHALL transfer all transaction history to the target record and deactivate the source record
6. WHEN re-numbering vouchers, THE ERP_System SHALL update all references to old voucher numbers

### Requirement 45: Year-End Closing

**User Story:** As an accountant, I want to perform year-end closing, so that the fiscal year is properly closed and opening balances are carried forward to the new year.

#### Acceptance Criteria

1. THE ERP_System SHALL support year-end closing process that calculates and posts closing entries
2. WHEN year closing is executed, THE ERP_System SHALL carry forward ledger closing balances as opening balances for the new fiscal year
3. WHEN year closing is executed, THE ERP_System SHALL carry forward stock opening quantities and values for the new year
4. THE ERP_System SHALL prevent new transaction posting in closed fiscal years unless explicitly unlocked
5. THE ERP_System SHALL support partial year closing for specific ledger groups or modules

### Requirement 46: Scheduled Jobs and Background Automation

**User Story:** As a system administrator, I want to configure and monitor automated background jobs, so that recurring tasks run without manual intervention.

#### Acceptance Criteria

1. THE ERP_System SHALL support job entity configuration defining automated tasks and their schedules
2. THE ERP_System SHALL execute scheduled jobs at configured intervals (daily, weekly, monthly)
3. THE ERP_System SHALL log all job executions with status, start time, end time, and result in the Job Log
4. WHEN a job fails, THE ERP_System SHALL log the error and optionally send an alert notification
5. THE ERP_System SHALL support manual job triggers for on-demand execution

### Requirement 47: DynamicAI and Report Writer

**User Story:** As a business analyst, I want AI-powered report generation and a visual query builder, so that I can create custom reports without writing SQL.

#### Acceptance Criteria

1. THE ERP_System SHALL support a Report Writer module for creating custom report templates with field selection, grouping, sorting, and filtering
2. THE ERP_System SHALL support a Query Builder for constructing ad-hoc data queries visually
3. THE ERP_System SHALL support a Dynamic AI feature for generating reports or queries using natural language inputs
4. THE ERP_System SHALL support a Dashboard Designer for creating user-defined dashboards with custom charts and KPIs
5. THE ERP_System SHALL support a New Entity module for defining custom data entities without code changes
6. THE ERP_System SHALL support running report viewer with transaction ID parameter for previewing generated reports
7. THE ERP_System SHALL support Global Action configuration for system-wide custom actions and workflows

### Requirement 48: Advanced Reporting (Additional Reports)

**User Story:** As a business user, I want access to all specialized reports, so that I can analyze every dimension of business performance.

#### Acceptance Criteria

1. THE ERP_System SHALL generate Cancel Day Book report for cancelled transaction tracking
2. THE ERP_System SHALL generate Cost Center Analysis (voucher-wise) report
3. THE ERP_System SHALL generate Cost Center Summary and Cost Center Breakup Ledger-Wise reports
4. THE ERP_System SHALL generate Bills Receivable and Bills Payable aging reports
5. THE ERP_System SHALL generate Statistic Voucher, Statistic Voucher Monthly, and Statistic Voucher Daily reports
6. THE ERP_System SHALL generate TDS-VAT combined register report
7. THE ERP_System SHALL generate Ledger Current Status and Ledger Daily reports
8. THE ERP_System SHALL generate Ledger Wise and Ledger Analysis reports
9. THE ERP_System SHALL generate CR (Credit) Limit Expired Party report for overdue credit reviews
10. THE ERP_System SHALL generate Cost Center Ageing report
11. THE ERP_System SHALL generate Pending PO Summary report for overdue purchase orders
12. THE ERP_System SHALL generate Out-of-Stock Bill-Wise report for unfulfilled sales orders
13. THE ERP_System SHALL generate All Agent Sales Summary report
14. THE ERP_System SHALL generate Product Monthly Summary report
15. THE ERP_System SHALL generate Product-Wise Additional Cost report
16. THE ERP_System SHALL generate Sales Allotment Details report
17. THE ERP_System SHALL generate Sales Costing Voucher-Wise and Purchase Costing Voucher-Wise reports
18. THE ERP_System SHALL generate Pending Cannibalize-In report
19. THE ERP_System SHALL generate Sales Materialized View report
20. THE ERP_System SHALL generate Party-Wise Product Rate list report

---

## Implementation Notes

This requirements document covers the complete ERP system with 426 entities across all discovered modules. The system requires:

- **Backend**: RESTful API with 97+ endpoints, business logic layer, data access layer
- **Frontend**: 721+ pages, 1441 forms, 2026 tables, 18937 buttons, 3002 modals
- **Database**: 426+ tables with proper relationships, indexes, and constraints
- **Testing**: Unit tests, integration tests, E2E tests for all modules
- **Security**: Authentication, authorization, role-based access control, audit logging

The implementation should follow a phased approach:
1. **Phase 1**: Core infrastructure (authentication, database, API framework)
2. **Phase 2**: Account module (ledgers, vouchers, customers, vendors, cheques, PDC/ODC)
3. **Phase 3**: Inventory module (products, godowns, purchase, sales, manufacturing, dispatch)
4. **Phase 4**: HR, Assets, Lab modules
5. **Phase 5**: Dashboard, reporting, analytics, and DynamicAI
6. **Phase 6**: Hospital module, Service module, Finance/Loan module
7. **Phase 7**: Task, Support, Loyalty, AppCMS modules
8. **Phase 8**: Advanced features (multi-branch, multi-currency, Tally integration, Nepal compliance)
9. **Phase 9**: Industry-specific modules (Petrol Pump, Dairy, Tea, Banking/Vehicle Finance)

Each phase should include complete testing before moving to the next phase.
