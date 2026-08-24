-- Parking Management System
-- MySQL 8.0+
-- Based on: Documento de Análise de Requisitos - Sistema de Gestão
-- ============================================================

CREATE DATABASE IF NOT EXISTS smartparkdb
  CHARACTER SET utf8mb4
  COLLATE utf8mb4_0900_ai_ci;

USE smartparkdb;

SET FOREIGN_KEY_CHECKS = 0;

-- ============================================================
-- 1. ORGANIZAÇÃO / MULTI-TENANT
-- ============================================================

DROP TABLE IF EXISTS integration_idempotency;
DROP TABLE IF EXISTS integration_external_references;
DROP TABLE IF EXISTS integration_systems;
DROP TABLE IF EXISTS audit_logs;
DROP TABLE IF EXISTS stock_movements;
DROP TABLE IF EXISTS stock_items;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS work_order_items;
DROP TABLE IF EXISTS work_orders;
DROP TABLE IF EXISTS appointments;
DROP TABLE IF EXISTS services;
DROP TABLE IF EXISTS fiscal_documents;
DROP TABLE IF EXISTS fiscal_configurations;
DROP TABLE IF EXISTS receivables;
DROP TABLE IF EXISTS invoices;
DROP TABLE IF EXISTS cash_movements;
DROP TABLE IF EXISTS cash_registers;
DROP TABLE IF EXISTS payments;
DROP TABLE IF EXISTS postpaid_usages;
DROP TABLE IF EXISTS postpaid_accounts;
DROP TABLE IF EXISTS agreements;
DROP TABLE IF EXISTS monthly_contract_vehicles;
DROP TABLE IF EXISTS monthly_contracts;
DROP TABLE IF EXISTS promotions;
DROP TABLE IF EXISTS discounts;
DROP TABLE IF EXISTS pricing_rules;
DROP TABLE IF EXISTS pricing_tables;
DROP TABLE IF EXISTS vehicle_inspections;
DROP TABLE IF EXISTS parking_operation_payments;
DROP TABLE IF EXISTS parking_operations;
DROP TABLE IF EXISTS tickets;
DROP TABLE IF EXISTS vehicles;
DROP TABLE IF EXISTS company_customers;
DROP TABLE IF EXISTS customers;
DROP TABLE IF EXISTS people;
DROP TABLE IF EXISTS user_permissions;
DROP TABLE IF EXISTS role_permissions;
DROP TABLE IF EXISTS user_roles;
DROP TABLE IF EXISTS permissions;
DROP TABLE IF EXISTS roles;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS parking_payment_methods;
DROP TABLE IF EXISTS payment_methods;
DROP TABLE IF EXISTS parkings;
DROP TABLE IF EXISTS establishments;
DROP TABLE IF EXISTS companies;

-- ============================================================
-- 2. COMPANIES
-- ============================================================

CREATE TABLE companies (
    id CHAR(36) NOT NULL,
    legal_name VARCHAR(200) NOT NULL,
    trade_name VARCHAR(200) NULL,
    tax_id VARCHAR(20) NOT NULL,
    email VARCHAR(255) NULL,
    phone VARCHAR(30) NULL,
    status ENUM('ACTIVE', 'INACTIVE', 'SUSPENDED') NOT NULL DEFAULT 'ACTIVE',
    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_companies_tax_id (tax_id),
    KEY idx_companies_status (status)
) ENGINE=InnoDB;

-- ============================================================
-- 3. ESTABLISHMENTS / MATRIZ / FILIAL
-- ============================================================

CREATE TABLE establishments (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parent_establishment_id CHAR(36) NULL,
    establishment_type ENUM('HEADQUARTERS', 'BRANCH') NOT NULL,
    legal_name VARCHAR(200) NOT NULL,
    trade_name VARCHAR(200) NULL,
    tax_id VARCHAR(20) NOT NULL,
    municipal_registration VARCHAR(50) NULL,
    state_registration VARCHAR(50) NULL,

    postal_code VARCHAR(12) NULL,
    street VARCHAR(200) NULL,
    number VARCHAR(30) NULL,
    complement VARCHAR(100) NULL,
    neighborhood VARCHAR(100) NULL,
    city VARCHAR(100) NULL,
    state VARCHAR(2) NULL,
    country VARCHAR(2) NOT NULL DEFAULT 'BR',

    email VARCHAR(255) NULL,
    phone VARCHAR(30) NULL,
    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_establishments_tax_id (tax_id),
    KEY idx_establishments_company (company_id),
    KEY idx_establishments_parent (parent_establishment_id),

    CONSTRAINT fk_establishments_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_establishments_parent
        FOREIGN KEY (parent_establishment_id) REFERENCES establishments(id)
) ENGINE=InnoDB;

-- ============================================================
-- 4. PARKINGS
-- ============================================================

CREATE TABLE parkings (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    establishment_id CHAR(36) NOT NULL,

    code VARCHAR(50) NOT NULL,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500) NULL,

    postal_code VARCHAR(12) NULL,
    street VARCHAR(200) NULL,
    number VARCHAR(30) NULL,
    complement VARCHAR(100) NULL,
    neighborhood VARCHAR(100) NULL,
    city VARCHAR(100) NULL,
    state VARCHAR(2) NULL,
    country VARCHAR(2) NOT NULL DEFAULT 'BR',

    capacity INT UNSIGNED NULL,
    opening_time TIME NULL,
    closing_time TIME NULL,

    status ENUM('ACTIVE', 'INACTIVE', 'MAINTENANCE') NOT NULL DEFAULT 'ACTIVE',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_parkings_company_code (company_id, code),
    KEY idx_parkings_establishment (establishment_id),
    KEY idx_parkings_company (company_id),

    CONSTRAINT fk_parkings_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_parkings_establishment
        FOREIGN KEY (establishment_id) REFERENCES establishments(id)
) ENGINE=InnoDB;

-- ============================================================
-- 5. PAYMENT METHODS
-- ============================================================

CREATE TABLE payment_methods (
    id CHAR(36) NOT NULL,
    code VARCHAR(50) NOT NULL,
    name VARCHAR(100) NOT NULL,
    type ENUM(
        'CASH',
        'PIX',
        'CREDIT_CARD',
        'DEBIT_CARD',
        'BANK_TRANSFER',
        'OTHER'
    ) NOT NULL,
    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),
    UNIQUE KEY uk_payment_methods_code (code)
) ENGINE=InnoDB;

CREATE TABLE parking_payment_methods (
    parking_id CHAR(36) NOT NULL,
    payment_method_id CHAR(36) NOT NULL,
    enabled BOOLEAN NOT NULL DEFAULT TRUE,

    PRIMARY KEY (parking_id, payment_method_id),

    CONSTRAINT fk_ppm_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_ppm_payment_method
        FOREIGN KEY (payment_method_id) REFERENCES payment_methods(id)
) ENGINE=InnoDB;

-- ============================================================
-- 6. USERS / RBAC
-- ============================================================

CREATE TABLE users (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    name VARCHAR(200) NOT NULL,
    email VARCHAR(255) NOT NULL,
    password_hash VARCHAR(500) NOT NULL,
    status ENUM('ACTIVE', 'INACTIVE', 'BLOCKED') NOT NULL DEFAULT 'ACTIVE',
    last_login_at DATETIME(6) NULL,
    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_users_company_email (company_id, email),
    KEY idx_users_company (company_id),

    CONSTRAINT fk_users_company
        FOREIGN KEY (company_id) REFERENCES companies(id)
) ENGINE=InnoDB;

CREATE TABLE roles (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    name VARCHAR(100) NOT NULL,
    description VARCHAR(500) NULL,

    PRIMARY KEY (id),
    UNIQUE KEY uk_roles_company_name (company_id, name),

    CONSTRAINT fk_roles_company
        FOREIGN KEY (company_id) REFERENCES companies(id)
) ENGINE=InnoDB;

CREATE TABLE permissions (
    id CHAR(36) NOT NULL,
    code VARCHAR(100) NOT NULL,
    description VARCHAR(500) NULL,

    PRIMARY KEY (id),
    UNIQUE KEY uk_permissions_code (code)
) ENGINE=InnoDB;

CREATE TABLE role_permissions (
    role_id CHAR(36) NOT NULL,
    permission_id CHAR(36) NOT NULL,

    PRIMARY KEY (role_id, permission_id),

    CONSTRAINT fk_role_permissions_role
        FOREIGN KEY (role_id) REFERENCES roles(id),

    CONSTRAINT fk_role_permissions_permission
        FOREIGN KEY (permission_id) REFERENCES permissions(id)
) ENGINE=InnoDB;

CREATE TABLE user_roles (
    user_id CHAR(36) NOT NULL,
    role_id CHAR(36) NOT NULL,

    PRIMARY KEY (user_id, role_id),

    CONSTRAINT fk_user_roles_user
        FOREIGN KEY (user_id) REFERENCES users(id),

    CONSTRAINT fk_user_roles_role
        FOREIGN KEY (role_id) REFERENCES roles(id)
) ENGINE=InnoDB;

-- User scope can be company-wide, establishment-specific or parking-specific.
CREATE TABLE user_permissions (
    id CHAR(36) NOT NULL,
    user_id CHAR(36) NOT NULL,
    establishment_id CHAR(36) NULL,
    parking_id CHAR(36) NULL,

    PRIMARY KEY (id),
    KEY idx_user_permissions_user (user_id),
    KEY idx_user_permissions_establishment (establishment_id),
    KEY idx_user_permissions_parking (parking_id),

    CONSTRAINT fk_user_permissions_user
        FOREIGN KEY (user_id) REFERENCES users(id),

    CONSTRAINT fk_user_permissions_establishment
        FOREIGN KEY (establishment_id) REFERENCES establishments(id),

    CONSTRAINT fk_user_permissions_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

-- ============================================================
-- 7. PEOPLE / CUSTOMERS
-- ============================================================

CREATE TABLE people (
    id CHAR(36) NOT NULL,
    person_type ENUM('INDIVIDUAL', 'LEGAL_ENTITY') NOT NULL,

    name VARCHAR(200) NOT NULL,
    tax_id VARCHAR(20) NOT NULL,
    email VARCHAR(255) NULL,
    phone VARCHAR(30) NULL,

    postal_code VARCHAR(12) NULL,
    street VARCHAR(200) NULL,
    number VARCHAR(30) NULL,
    complement VARCHAR(100) NULL,
    neighborhood VARCHAR(100) NULL,
    city VARCHAR(100) NULL,
    state VARCHAR(2) NULL,
    country VARCHAR(2) NOT NULL DEFAULT 'BR',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_people_tax_id (tax_id)
) ENGINE=InnoDB;

CREATE TABLE customers (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    person_id CHAR(36) NOT NULL,

    customer_type ENUM(
        'ROTATIVE',
        'MONTHLY',
        'AGREEMENT',
        'POSTPAID'
    ) NOT NULL DEFAULT 'ROTATIVE',

    status ENUM('ACTIVE', 'INACTIVE', 'BLOCKED') NOT NULL DEFAULT 'ACTIVE',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_customers_company_person (company_id, person_id),
    KEY idx_customers_company_type (company_id, customer_type),

    CONSTRAINT fk_customers_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_customers_person
        FOREIGN KEY (person_id) REFERENCES people(id)
) ENGINE=InnoDB;

CREATE TABLE company_customers (
    company_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NOT NULL,
    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (company_id, customer_id),

    CONSTRAINT fk_company_customers_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_company_customers_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id)
) ENGINE=InnoDB;

-- ============================================================
-- 8. VEHICLES
-- ============================================================

CREATE TABLE vehicles (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,

    plate VARCHAR(10) NOT NULL,
    brand VARCHAR(100) NULL,
    model VARCHAR(100) NULL,
    color VARCHAR(50) NULL,
    vehicle_type ENUM(
        'CAR',
        'MOTORCYCLE',
        'TRUCK',
        'VAN',
        'OTHER'
    ) NOT NULL DEFAULT 'CAR',

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_vehicles_company_plate (company_id, plate),
    KEY idx_vehicles_plate (plate),

    CONSTRAINT fk_vehicles_company
        FOREIGN KEY (company_id) REFERENCES companies(id)
) ENGINE=InnoDB;

-- Many-to-many is intentionally supported because ownership/responsibility
-- may change and a vehicle can be associated with more than one customer.
CREATE TABLE customer_vehicles (
    customer_id CHAR(36) NOT NULL,
    vehicle_id CHAR(36) NOT NULL,
    relationship_type ENUM('OWNER', 'RESPONSIBLE', 'AUTHORIZED') NOT NULL DEFAULT 'OWNER',
    valid_from DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    valid_until DATETIME(6) NULL,

    PRIMARY KEY (customer_id, vehicle_id),

    CONSTRAINT fk_customer_vehicles_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    CONSTRAINT fk_customer_vehicles_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(id)
) ENGINE=InnoDB;

-- ============================================================
-- 9. PRICING
-- ============================================================

CREATE TABLE pricing_tables (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,

    name VARCHAR(150) NOT NULL,
    operation_type ENUM(
        'ROTATIVE',
        'MONTHLY',
        'AGREEMENT',
        'POSTPAID'
    ) NOT NULL DEFAULT 'ROTATIVE',

    valid_from DATETIME(6) NOT NULL,
    valid_until DATETIME(6) NULL,

    status ENUM('DRAFT', 'ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'DRAFT',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    KEY idx_pricing_tables_parking_validity
        (parking_id, valid_from, valid_until),

    CONSTRAINT fk_pricing_tables_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_pricing_tables_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

CREATE TABLE pricing_rules (
    id CHAR(36) NOT NULL,
    pricing_table_id CHAR(36) NOT NULL,

    rule_type ENUM(
        'PER_MINUTE',
        'PER_HOUR',
        'FRACTION',
        'DAILY',
        'OVERNIGHT',
        'FIXED'
    ) NOT NULL,

    start_minute INT UNSIGNED NOT NULL DEFAULT 0,
    end_minute INT UNSIGNED NULL,
    fraction_minutes INT UNSIGNED NULL,

    amount DECIMAL(15,2) NOT NULL,
    priority INT NOT NULL DEFAULT 0,

    PRIMARY KEY (id),
    KEY idx_pricing_rules_table (pricing_table_id),

    CONSTRAINT fk_pricing_rules_table
        FOREIGN KEY (pricing_table_id) REFERENCES pricing_tables(id)
) ENGINE=InnoDB;

CREATE TABLE discounts (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,

    name VARCHAR(150) NOT NULL,
    discount_type ENUM('PERCENTAGE', 'FIXED', 'FREE') NOT NULL,
    value DECIMAL(15,4) NULL,

    valid_from DATETIME(6) NULL,
    valid_until DATETIME(6) NULL,
    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),

    CONSTRAINT fk_discounts_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_discounts_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

CREATE TABLE promotions (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,

    name VARCHAR(150) NOT NULL,
    description VARCHAR(500) NULL,

    valid_from DATETIME(6) NOT NULL,
    valid_until DATETIME(6) NULL,

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),

    CONSTRAINT fk_promotions_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_promotions_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

-- ============================================================
-- 10. TICKETS / PARKING OPERATIONS
-- ============================================================

CREATE TABLE tickets (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,

    ticket_number VARCHAR(100) NOT NULL,
    issued_at DATETIME(6) NOT NULL,
    status ENUM(
        'OPEN',
        'PAID',
        'CANCELLED',
        'CLOSED'
    ) NOT NULL DEFAULT 'OPEN',

    PRIMARY KEY (id),
    UNIQUE KEY uk_tickets_parking_number (parking_id, ticket_number),
    KEY idx_tickets_company (company_id),
    KEY idx_tickets_status (status),

    CONSTRAINT fk_tickets_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_tickets_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

CREATE TABLE parking_operations (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,
    ticket_id CHAR(36) NULL,
    vehicle_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NULL,
    pricing_table_id CHAR(36) NULL,

    operation_type ENUM(
        'ROTATIVE',
        'MONTHLY',
        'AGREEMENT',
        'POSTPAID'
    ) NOT NULL DEFAULT 'ROTATIVE',

    status ENUM(
        'OPEN',
        'IN_PARKING',
        'WAITING_PAYMENT',
        'PAID',
        'COMPLETED',
        'CANCELLED'
    ) NOT NULL DEFAULT 'OPEN',

    entry_at DATETIME(6) NOT NULL,
    exit_at DATETIME(6) NULL,

    calculated_amount DECIMAL(15,2) NOT NULL DEFAULT 0,
    discount_amount DECIMAL(15,2) NOT NULL DEFAULT 0,
    final_amount DECIMAL(15,2) NOT NULL DEFAULT 0,

    pricing_snapshot JSON NULL,

    entry_operator_id CHAR(36) NULL,
    exit_operator_id CHAR(36) NULL,

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),

    KEY idx_operations_company (company_id),
    KEY idx_operations_parking_status (parking_id, status),
    KEY idx_operations_vehicle (vehicle_id),
    KEY idx_operations_customer (customer_id),
    KEY idx_operations_entry (entry_at),
    KEY idx_operations_exit (exit_at),

    CONSTRAINT fk_operations_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_operations_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_operations_ticket
        FOREIGN KEY (ticket_id) REFERENCES tickets(id),

    CONSTRAINT fk_operations_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(id),

    CONSTRAINT fk_operations_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    CONSTRAINT fk_operations_pricing_table
        FOREIGN KEY (pricing_table_id) REFERENCES pricing_tables(id),

    CONSTRAINT fk_operations_entry_operator
        FOREIGN KEY (entry_operator_id) REFERENCES users(id),

    CONSTRAINT fk_operations_exit_operator
        FOREIGN KEY (exit_operator_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- ============================================================
-- 11. VEHICLE INSPECTION / DAMAGE CHECKLIST
-- ============================================================

CREATE TABLE vehicle_inspections (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_operation_id CHAR(36) NOT NULL,

    inspection_type ENUM('ENTRY', 'EXIT') NOT NULL,
    notes TEXT NULL,
    photos JSON NULL,

    performed_by CHAR(36) NULL,
    performed_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    KEY idx_inspections_operation (parking_operation_id),

    CONSTRAINT fk_inspections_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_inspections_operation
        FOREIGN KEY (parking_operation_id) REFERENCES parking_operations(id),

    CONSTRAINT fk_inspections_user
        FOREIGN KEY (performed_by) REFERENCES users(id)
) ENGINE=InnoDB;

-- ============================================================
-- 12. AGREEMENTS / CONVENIOS
-- ============================================================

CREATE TABLE agreements (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NULL,

    name VARCHAR(200) NOT NULL,
    partner_name VARCHAR(200) NOT NULL,

    discount_type ENUM('PERCENTAGE', 'FIXED', 'FREE') NOT NULL,
    discount_value DECIMAL(15,4) NULL,

    valid_from DATETIME(6) NOT NULL,
    valid_until DATETIME(6) NULL,

    status ENUM('ACTIVE', 'INACTIVE', 'EXPIRED') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),

    CONSTRAINT fk_agreements_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_agreements_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_agreements_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id)
) ENGINE=InnoDB;

-- ============================================================
-- 13. MONTHLY CONTRACTS
-- ============================================================

CREATE TABLE monthly_contracts (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NOT NULL,

    contract_number VARCHAR(100) NOT NULL,

    valid_from DATE NOT NULL,
    valid_until DATE NULL,

    monthly_amount DECIMAL(15,2) NOT NULL,

    allowed_start_time TIME NULL,
    allowed_end_time TIME NULL,

    days_of_week JSON NULL,

    status ENUM(
        'DRAFT',
        'ACTIVE',
        'SUSPENDED',
        'EXPIRED',
        'CANCELLED'
    ) NOT NULL DEFAULT 'DRAFT',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    updated_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6)
        ON UPDATE CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_monthly_contracts_company_number
        (company_id, contract_number),

    CONSTRAINT fk_monthly_contracts_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_monthly_contracts_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_monthly_contracts_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id)
) ENGINE=InnoDB;

CREATE TABLE monthly_contract_vehicles (
    monthly_contract_id CHAR(36) NOT NULL,
    vehicle_id CHAR(36) NOT NULL,

    PRIMARY KEY (monthly_contract_id, vehicle_id),

    CONSTRAINT fk_mcv_contract
        FOREIGN KEY (monthly_contract_id) REFERENCES monthly_contracts(id),

    CONSTRAINT fk_mcv_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(id)
) ENGINE=InnoDB;

-- ============================================================
-- 14. POSTPAID
-- ============================================================

CREATE TABLE postpaid_accounts (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NOT NULL,

    account_number VARCHAR(100) NOT NULL,
    credit_limit DECIMAL(15,2) NULL,
    billing_day TINYINT UNSIGNED NULL,
    due_day TINYINT UNSIGNED NULL,

    status ENUM('ACTIVE', 'BLOCKED', 'CLOSED') NOT NULL DEFAULT 'ACTIVE',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),
    UNIQUE KEY uk_postpaid_accounts_company_number
        (company_id, account_number),

    CONSTRAINT fk_postpaid_accounts_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_postpaid_accounts_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id)
) ENGINE=InnoDB;

CREATE TABLE postpaid_usages (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    postpaid_account_id CHAR(36) NOT NULL,
    parking_operation_id CHAR(36) NOT NULL,

    amount DECIMAL(15,2) NOT NULL,
    usage_at DATETIME(6) NOT NULL,

    invoiced BOOLEAN NOT NULL DEFAULT FALSE,

    PRIMARY KEY (id),
    UNIQUE KEY uk_postpaid_usage_operation (parking_operation_id),

    CONSTRAINT fk_postpaid_usages_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_postpaid_usages_account
        FOREIGN KEY (postpaid_account_id) REFERENCES postpaid_accounts(id),

    CONSTRAINT fk_postpaid_usages_operation
        FOREIGN KEY (parking_operation_id) REFERENCES parking_operations(id)
) ENGINE=InnoDB;

-- ============================================================
-- 15. PAYMENTS
-- ============================================================

CREATE TABLE payments (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    payment_method_id CHAR(36) NOT NULL,

    amount DECIMAL(15,2) NOT NULL,
    paid_at DATETIME(6) NOT NULL,

    status ENUM(
        'PENDING',
        'COMPLETED',
        'CANCELLED',
        'REFUNDED'
    ) NOT NULL DEFAULT 'PENDING',

    external_reference VARCHAR(200) NULL,
    metadata JSON NULL,

    created_by CHAR(36) NULL,

    PRIMARY KEY (id),
    KEY idx_payments_company_date (company_id, paid_at),
    KEY idx_payments_status (status),

    CONSTRAINT fk_payments_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_payments_method
        FOREIGN KEY (payment_method_id) REFERENCES payment_methods(id),

    CONSTRAINT fk_payments_user
        FOREIGN KEY (created_by) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE parking_operation_payments (
    parking_operation_id CHAR(36) NOT NULL,
    payment_id CHAR(36) NOT NULL,
    amount DECIMAL(15,2) NOT NULL,

    PRIMARY KEY (parking_operation_id, payment_id),

    CONSTRAINT fk_operation_payments_operation
        FOREIGN KEY (parking_operation_id) REFERENCES parking_operations(id),

    CONSTRAINT fk_operation_payments_payment
        FOREIGN KEY (payment_id) REFERENCES payments(id)
) ENGINE=InnoDB;

-- ============================================================
-- 16. CASH REGISTER
-- ============================================================

CREATE TABLE cash_registers (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NOT NULL,
    opened_by CHAR(36) NOT NULL,

    opened_at DATETIME(6) NOT NULL,
    opening_amount DECIMAL(15,2) NOT NULL DEFAULT 0,

    closed_by CHAR(36) NULL,
    closed_at DATETIME(6) NULL,

    expected_amount DECIMAL(15,2) NULL,
    closing_amount DECIMAL(15,2) NULL,
    difference_amount DECIMAL(15,2) NULL,

    status ENUM('OPEN', 'CLOSED', 'CANCELLED') NOT NULL DEFAULT 'OPEN',

    PRIMARY KEY (id),

    KEY idx_cash_registers_parking_status (parking_id, status),

    CONSTRAINT fk_cash_registers_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_cash_registers_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_cash_registers_opened_by
        FOREIGN KEY (opened_by) REFERENCES users(id),

    CONSTRAINT fk_cash_registers_closed_by
        FOREIGN KEY (closed_by) REFERENCES users(id)
) ENGINE=InnoDB;

CREATE TABLE cash_movements (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    cash_register_id CHAR(36) NOT NULL,
    payment_id CHAR(36) NULL,

    movement_type ENUM(
        'OPENING',
        'PAYMENT',
        'WITHDRAWAL',
        'SUPPLY',
        'ADJUSTMENT',
        'REFUND'
    ) NOT NULL,

    amount DECIMAL(15,2) NOT NULL,
    occurred_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    description VARCHAR(500) NULL,
    created_by CHAR(36) NULL,

    PRIMARY KEY (id),

    KEY idx_cash_movements_register (cash_register_id),
    KEY idx_cash_movements_date (occurred_at),

    CONSTRAINT fk_cash_movements_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_cash_movements_register
        FOREIGN KEY (cash_register_id) REFERENCES cash_registers(id),

    CONSTRAINT fk_cash_movements_payment
        FOREIGN KEY (payment_id) REFERENCES payments(id),

    CONSTRAINT fk_cash_movements_user
        FOREIGN KEY (created_by) REFERENCES users(id)
) ENGINE=InnoDB;

-- ============================================================
-- 17. BILLING / RECEIVABLES
-- ============================================================

CREATE TABLE invoices (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    customer_id CHAR(36) NOT NULL,
    postpaid_account_id CHAR(36) NULL,

    invoice_number VARCHAR(100) NOT NULL,

    issue_date DATE NOT NULL,
    due_date DATE NOT NULL,

    subtotal DECIMAL(15,2) NOT NULL,
    discount_amount DECIMAL(15,2) NOT NULL DEFAULT 0,
    total_amount DECIMAL(15,2) NOT NULL,

    status ENUM(
        'DRAFT',
        'ISSUED',
        'OPEN',
        'PAID',
        'OVERDUE',
        'CANCELLED'
    ) NOT NULL DEFAULT 'DRAFT',

    PRIMARY KEY (id),
    UNIQUE KEY uk_invoices_company_number (company_id, invoice_number),

    CONSTRAINT fk_invoices_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_invoices_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    CONSTRAINT fk_invoices_postpaid
        FOREIGN KEY (postpaid_account_id) REFERENCES postpaid_accounts(id)
) ENGINE=InnoDB;

CREATE TABLE receivables (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    invoice_id CHAR(36) NOT NULL,

    due_date DATE NOT NULL,
    original_amount DECIMAL(15,2) NOT NULL,
    paid_amount DECIMAL(15,2) NOT NULL DEFAULT 0,

    status ENUM(
        'OPEN',
        'PARTIALLY_PAID',
        'PAID',
        'OVERDUE',
        'CANCELLED'
    ) NOT NULL DEFAULT 'OPEN',

    PRIMARY KEY (id),
    UNIQUE KEY uk_receivables_invoice (invoice_id),

    CONSTRAINT fk_receivables_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_receivables_invoice
        FOREIGN KEY (invoice_id) REFERENCES invoices(id)
) ENGINE=InnoDB;

-- ============================================================
-- 18. FISCAL
-- ============================================================

CREATE TABLE fiscal_configurations (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    establishment_id CHAR(36) NOT NULL,

    municipal_registration VARCHAR(50) NULL,
    tax_regime VARCHAR(50) NULL,
    service_code VARCHAR(50) NULL,

    provider VARCHAR(100) NULL,
    configuration JSON NULL,

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),

    CONSTRAINT fk_fiscal_config_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_fiscal_config_establishment
        FOREIGN KEY (establishment_id) REFERENCES establishments(id)
) ENGINE=InnoDB;

CREATE TABLE fiscal_documents (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    establishment_id CHAR(36) NOT NULL,
    invoice_id CHAR(36) NULL,

    document_type ENUM('NFS_E', 'OTHER') NOT NULL,

    document_number VARCHAR(100) NULL,
    external_id VARCHAR(200) NULL,

    issue_date DATETIME(6) NULL,

    status ENUM(
        'PENDING',
        'PROCESSING',
        'AUTHORIZED',
        'REJECTED',
        'CANCELLED'
    ) NOT NULL DEFAULT 'PENDING',

    payload JSON NULL,
    response JSON NULL,

    PRIMARY KEY (id),

    CONSTRAINT fk_fiscal_documents_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_fiscal_documents_establishment
        FOREIGN KEY (establishment_id) REFERENCES establishments(id),

    CONSTRAINT fk_fiscal_documents_invoice
        FOREIGN KEY (invoice_id) REFERENCES invoices(id)
) ENGINE=InnoDB;

-- ============================================================
-- 19. SERVICES / APPOINTMENTS / WORK ORDERS
-- ============================================================

CREATE TABLE services (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,

    name VARCHAR(150) NOT NULL,
    description VARCHAR(500) NULL,
    amount DECIMAL(15,2) NOT NULL DEFAULT 0,

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),

    CONSTRAINT fk_services_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_services_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id)
) ENGINE=InnoDB;

CREATE TABLE appointments (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,
    customer_id CHAR(36) NOT NULL,
    vehicle_id CHAR(36) NOT NULL,
    service_id CHAR(36) NOT NULL,

    scheduled_at DATETIME(6) NOT NULL,

    status ENUM(
        'SCHEDULED',
        'CONFIRMED',
        'IN_PROGRESS',
        'COMPLETED',
        'CANCELLED',
        'NO_SHOW'
    ) NOT NULL DEFAULT 'SCHEDULED',

    notes VARCHAR(1000) NULL,

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),

    KEY idx_appointments_date (scheduled_at),

    CONSTRAINT fk_appointments_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_appointments_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_appointments_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    CONSTRAINT fk_appointments_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(id),

    CONSTRAINT fk_appointments_service
        FOREIGN KEY (service_id) REFERENCES services(id)
) ENGINE=InnoDB;

CREATE TABLE work_orders (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,
    customer_id CHAR(36) NOT NULL,
    vehicle_id CHAR(36) NOT NULL,

    order_number VARCHAR(100) NOT NULL,

    status ENUM(
        'OPEN',
        'IN_PROGRESS',
        'COMPLETED',
        'CANCELLED'
    ) NOT NULL DEFAULT 'OPEN',

    opened_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    completed_at DATETIME(6) NULL,

    notes VARCHAR(1000) NULL,

    PRIMARY KEY (id),
    UNIQUE KEY uk_work_orders_company_number (company_id, order_number),

    CONSTRAINT fk_work_orders_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_work_orders_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_work_orders_customer
        FOREIGN KEY (customer_id) REFERENCES customers(id),

    CONSTRAINT fk_work_orders_vehicle
        FOREIGN KEY (vehicle_id) REFERENCES vehicles(id)
) ENGINE=InnoDB;

CREATE TABLE work_order_items (
    id CHAR(36) NOT NULL,
    work_order_id CHAR(36) NOT NULL,
    service_id CHAR(36) NOT NULL,

    quantity DECIMAL(15,3) NOT NULL DEFAULT 1,
    unit_amount DECIMAL(15,2) NOT NULL,
    total_amount DECIMAL(15,2) NOT NULL,

    PRIMARY KEY (id),

    CONSTRAINT fk_work_order_items_order
        FOREIGN KEY (work_order_id) REFERENCES work_orders(id),

    CONSTRAINT fk_work_order_items_service
        FOREIGN KEY (service_id) REFERENCES services(id)
) ENGINE=InnoDB;

-- ============================================================
-- 20. STOCK
-- ============================================================

CREATE TABLE products (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,

    sku VARCHAR(100) NOT NULL,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500) NULL,

    unit VARCHAR(20) NOT NULL DEFAULT 'UN',
    cost_amount DECIMAL(15,2) NOT NULL DEFAULT 0,
    sale_amount DECIMAL(15,2) NOT NULL DEFAULT 0,

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',

    PRIMARY KEY (id),
    UNIQUE KEY uk_products_company_sku (company_id, sku),

    CONSTRAINT fk_products_company
        FOREIGN KEY (company_id) REFERENCES companies(id)
) ENGINE=InnoDB;

CREATE TABLE stock_items (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    parking_id CHAR(36) NULL,
    product_id CHAR(36) NOT NULL,

    quantity DECIMAL(15,3) NOT NULL DEFAULT 0,

    PRIMARY KEY (id),
    UNIQUE KEY uk_stock_items_parking_product (parking_id, product_id),

    CONSTRAINT fk_stock_items_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_stock_items_parking
        FOREIGN KEY (parking_id) REFERENCES parkings(id),

    CONSTRAINT fk_stock_items_product
        FOREIGN KEY (product_id) REFERENCES products(id)
) ENGINE=InnoDB;

CREATE TABLE stock_movements (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    stock_item_id CHAR(36) NOT NULL,

    movement_type ENUM(
        'ENTRY',
        'EXIT',
        'ADJUSTMENT',
        'INVENTORY'
    ) NOT NULL,

    quantity DECIMAL(15,3) NOT NULL,
    occurred_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    reference_type VARCHAR(100) NULL,
    reference_id CHAR(36) NULL,
    notes VARCHAR(500) NULL,

    created_by CHAR(36) NULL,

    PRIMARY KEY (id),

    CONSTRAINT fk_stock_movements_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_stock_movements_stock
        FOREIGN KEY (stock_item_id) REFERENCES stock_items(id),

    CONSTRAINT fk_stock_movements_user
        FOREIGN KEY (created_by) REFERENCES users(id)
) ENGINE=InnoDB;

-- ============================================================
-- 21. AUDIT
-- ============================================================

CREATE TABLE audit_logs (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    user_id CHAR(36) NULL,

    entity_type VARCHAR(100) NOT NULL,
    entity_id CHAR(36) NOT NULL,
    action VARCHAR(50) NOT NULL,

    old_values JSON NULL,
    new_values JSON NULL,

    ip_address VARCHAR(45) NULL,
    user_agent VARCHAR(500) NULL,

    occurred_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),

    KEY idx_audit_entity (entity_type, entity_id),
    KEY idx_audit_company_date (company_id, occurred_at),

    CONSTRAINT fk_audit_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_audit_user
        FOREIGN KEY (user_id) REFERENCES users(id)
) ENGINE=InnoDB;

-- ============================================================
-- 22. INTEGRATIONS
-- ============================================================

CREATE TABLE integration_systems (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,

    name VARCHAR(150) NOT NULL,
    system_type VARCHAR(100) NOT NULL,
    base_url VARCHAR(500) NULL,

    status ENUM('ACTIVE', 'INACTIVE') NOT NULL DEFAULT 'ACTIVE',
    configuration JSON NULL,

    PRIMARY KEY (id),

    CONSTRAINT fk_integration_systems_company
        FOREIGN KEY (company_id) REFERENCES companies(id)
) ENGINE=InnoDB;

CREATE TABLE integration_external_references (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    integration_system_id CHAR(36) NOT NULL,

    entity_type VARCHAR(100) NOT NULL,
    internal_id CHAR(36) NOT NULL,
    external_id VARCHAR(200) NOT NULL,

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),

    PRIMARY KEY (id),

    UNIQUE KEY uk_external_reference (
        integration_system_id,
        entity_type,
        external_id
    ),

    UNIQUE KEY uk_internal_reference (
        integration_system_id,
        entity_type,
        internal_id
    ),

    CONSTRAINT fk_external_references_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_external_references_system
        FOREIGN KEY (integration_system_id) REFERENCES integration_systems(id)
) ENGINE=InnoDB;

CREATE TABLE integration_idempotency (
    id CHAR(36) NOT NULL,
    company_id CHAR(36) NOT NULL,
    integration_system_id CHAR(36) NOT NULL,

    idempotency_key VARCHAR(255) NOT NULL,
    operation_type VARCHAR(100) NOT NULL,

    request_hash VARCHAR(128) NULL,
    response JSON NULL,

    status ENUM('PROCESSING', 'PROCESSED', 'FAILED') NOT NULL DEFAULT 'PROCESSING',

    created_at DATETIME(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    processed_at DATETIME(6) NULL,

    PRIMARY KEY (id),

    UNIQUE KEY uk_idempotency_key (
        integration_system_id,
        idempotency_key
    ),

    CONSTRAINT fk_idempotency_company
        FOREIGN KEY (company_id) REFERENCES companies(id),

    CONSTRAINT fk_idempotency_system
        FOREIGN KEY (integration_system_id) REFERENCES integration_systems(id)
) ENGINE=InnoDB;

SET FOREIGN_KEY_CHECKS = 1;

-- ============================================================
-- 23. INITIAL PERMISSIONS
-- ============================================================

INSERT INTO permissions (id, code, description) VALUES
(UUID(), 'COMPANY_READ', 'Visualizar empresa'),
(UUID(), 'COMPANY_WRITE', 'Alterar empresa'),
(UUID(), 'ESTABLISHMENT_READ', 'Visualizar estabelecimentos'),
(UUID(), 'ESTABLISHMENT_WRITE', 'Alterar estabelecimentos'),
(UUID(), 'PARKING_READ', 'Visualizar estacionamentos'),
(UUID(), 'PARKING_WRITE', 'Alterar estacionamentos'),
(UUID(), 'USER_READ', 'Visualizar usuários'),
(UUID(), 'USER_WRITE', 'Alterar usuários'),
(UUID(), 'CUSTOMER_READ', 'Visualizar clientes'),
(UUID(), 'CUSTOMER_WRITE', 'Alterar clientes'),
(UUID(), 'VEHICLE_READ', 'Visualizar veículos'),
(UUID(), 'VEHICLE_WRITE', 'Alterar veículos'),
(UUID(), 'PARKING_OPERATION_READ', 'Visualizar operações'),
(UUID(), 'PARKING_OPERATION_WRITE', 'Operar entrada e saída'),
(UUID(), 'PRICING_READ', 'Visualizar tarifas'),
(UUID(), 'PRICING_WRITE', 'Alterar tarifas'),
(UUID(), 'PAYMENT_READ', 'Visualizar pagamentos'),
(UUID(), 'PAYMENT_WRITE', 'Registrar pagamentos'),
(UUID(), 'CASH_READ', 'Visualizar caixa'),
(UUID(), 'CASH_WRITE', 'Operar caixa'),
(UUID(), 'REPORT_READ', 'Visualizar relatórios'),
(UUID(), 'AUDIT_READ', 'Visualizar auditoria');

-- ============================================================
-- 24. NOTES
-- ============================================================

-- IMPORTANTE:
-- 1. Este script representa o MODELO FÍSICO INICIAL.
-- 2. Ele não deve ser considerado definitivo antes da validação
--    do domínio e das regras de negócio.
-- 3. O modelo utiliza multi-tenancy lógico por company_id.
-- 4. O sistema externo deverá ser integrado por adapters/mappings.
-- 5. pricing_snapshot foi incluído para preservar a regra utilizada
--    no cálculo de uma operação e evitar dependência retroativa da
--    tabela de preços.
-- 6. UUIDs são utilizados como identificadores internos.
-- 7. O modelo deve ser revisado antes de produção, principalmente:
--      - regras de tarifação;
--      - mensalistas;
--      - convênios;
--      - pós-pago;
--      - fiscal;
--      - operação offline;
--      - equipamentos;
--      - integração com o sistema legado.
