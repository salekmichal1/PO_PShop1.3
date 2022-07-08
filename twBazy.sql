USE master
GO
--Tworzenie bazy
CREATE DATABASE rtvDatabase;
GO
--Tworzenie tabel:
USE rtvDatabase;
CREATE TABLE Product_category(
id INT IDENTITY(1, 1) CONSTRAINT category_id
PRIMARY KEY NOT NULL, category_name NVARCHAR(50) NOT NULL
);
CREATE TABLE Producers(
id INT IDENTITY(1,1) CONSTRAINT producer_id PRIMARY
KEY NOT NULL, producer_name NVARCHAR(50) NOT NULL
);
CREATE TABLE Customers(
id INT IDENTITY(1, 1) CONSTRAINT customer_id PRIMARY
KEY NOT NULL, customer_name NVARCHAR(50) NOT NULL,
surname NVARCHAR(50) NOT NULL, company_name NVARCHAR(50), company_number
INT, street NVARCHAR(50) NOT NULL, street_number
SMALLINT NOT NULL, flat_number SMALLINT, post_code VARCHAR(6) NOT NULL,
city NVARCHAR(50) NOT NULL, phone_number CHAR(12) NOT NULL, mail VARCHAR(50)
NOT
NULL UNIQUE CHECK(((LEN(mail) - LEN(REPLACE(mail, '@', ''))) = 1)),
login NVARCHAR(50) UNIQUE, password CHAR(60)
);
CREATE TABLE Products(
id INT IDENTITY(1, 1) CONSTRAINT product_id PRIMARY KEY
NOT NULL, product_name NVARCHAR(50) NOT NULL,
category_id int CONSTRAINT id_category FOREIGN KEY REFERENCES
Product_category(id) NOT NULL,
producer_id int CONSTRAINT id_producer FOREIGN KEY REFERENCES Producers(id) NOT
NULL, available_pieces SMALLINT NOT NULL,
net_catalog_price DECIMAL(6,2) NOT NULL, net_selling_price DECIMAL(6,2) NOT
NULL, net_selling_warehouse DECIMAL(6,2) NOT NULL, VAT_rate TINYINT NOT NULL
);
CREATE TABLE Employees(
id INT IDENTITY(1, 1) CONSTRAINT employee_id PRIMARY
KEY NOT NULL, employee_name NVARCHAR(50) NOT NULL,
employee_surname NVARCHAR(50) NOT NULL, mail VARCHAR(50) NOT NULL UNIQUE
CHECK(((LEN(mail) - LEN(REPLACE(mail, '@', ''))) = 1)),
login NVARCHAR(50) UNIQUE NOT NULL, password CHAR(60) NOT NULL, employee_bonus
DECIMAL (6,2) DEFAULT 200.00
);
CREATE TABLE Invoices(
id INT IDENTITY(1, 1) CONSTRAINT invoice_id PRIMARY KEY
NOT NULL, invoice_name VARCHAR(12) UNIQUE NOT NULL,
date_of_invoice DATETIME NOT NULL
);
CREATE TABLE Orders(
id INT IDENTITY(1, 1) CONSTRAINT order_id PRIMARY KEY NOT
NULL,
customer_id int CONSTRAINT id_customer FOREIGN KEY REFERENCES Customers(id) NOT
NULL,
date_of_placing_the_order DATETIME NOT NULL, order_realization_date DATETIME,
whether_the_order_fulfilled BIT, shipping_date DATETIME,
employee_id int CONSTRAINT id_employee FOREIGN KEY REFERENCES Employees(id) NOT
NULL,
invoice_id int CONSTRAINT id_invoice FOREIGN KEY REFERENCES Invoices(id) NOT
NULL
);
CREATE TABLE Ordered_products(
id INT IDENTITY(1, 1) CONSTRAINT
orderd_products_id PRIMARY KEY NOT NULL,
order_id int CONSTRAINT id_order FOREIGN KEY REFERENCES Orders(id) NOT NULL,
product_id int CONSTRAINT id_product FOREIGN KEY REFERENCES Products(id) NOT
NULL, quantity INT NOT NULL
);
--Tworzenie rekordów:
INSERT INTO Customers( customer_name, surname, street, street_number,
flat_number, post_code,
city, phone_number, Mail, login, password)
VALUES
('Michal', 'Salek', 'Tetmaiera', 12, null, '31-333', 'Kraków',
'123456789', 'michal@o2.pl', 'michal123', 'qwerty1'),
('Julia', 'Maly', 'Rolna', 34, null, '31-322', 'Kraków', '232145621',
'julia@o2.pl', 'julia123', 'qwerty121'),
('Jan', 'Kowalski', 'Polna', 3, 4, '31-321', 'Kraków', '232124213',
'jan@o2.pl', 'jan123', 'qwerty123'),
('Tomasz', 'Nowak', 'Szkolna', 44, null, '31-123', 'Kraków',
'987654321', 'tomasz@o2.pl', 'tomek123', 'qwerty1234'),
('Pawel', 'Duzy', 'Wolna', 22, null, '31-111', 'Kraków', '321452332',
'pawel@o2.pl', 'pawel123', 'qwerty141');
INSERT INTO Product_category(category_name)
VALUES ('Telewizor'), ('Glosniki'), ('Mikrofalowka'), ('Laptop'),
('Klawiatura');
INSERT INTO Producers(producer_name) VALUES
('Sony'),('Logitech'), ('Lg'), ('Acer'), ('Dell');
INSERT INTO Products( product_name, category_id, producer_id, available_pieces,
net_catalog_price,
net_selling_price, net_selling_warehouse, VAT_rate)
VALUES
('Sony KD-65XF9005', 1, 1, 1, 5500.00, 5300.00, 4900.00, 23),
('Dell Inspiron 3583', 4, 5, 2, 2199.00, 1949.00, 1799.00, 23),
('LG microvawe', 3, 3, 299.00, 2, 299.00, 199.00, 23),
('Logitech K906', 2, 2, 1149, 10, 999.00, 850.00, 23),
('Logitech K120', 5, 2, 89.00, 8, 79.00, 58.00, 23),
('Logitech G502', 2, 2, 249.00, 12, 199.00, 149.00, 23);
INSERT INTO Employees ( employee_name, employee_surname, mail, login, password,
employee_bonus)
VALUES
('W³adys³aw', '£okietek', 'lokietek@gmail.com', 'wlalo123', 'zaq1@WSX',
DEFAULT),
('W³adys³aw', 'Jagie³o', 'jakieglo@gmail.com', 'wladzia22', 'zaq1@WSX',
DEFAULT),
('Jan', 'Sobieski', 'sobjan@gmail.com', 'sebek77', 'zaq1@WSX', DEFAULT),
('Jacek', 'Soplica', 'soplicajacek@gmail.com', 'sopjacek99', 'zaq1@WSX',
DEFAULT);
INSERT INTO Invoices( invoice_name, date_of_invoice)
VALUES
('KK2021000001', '20180602'),
('KK2021000002', '20200210'),
('KK2021000003', '20200210'),
('KK2021000004', '20190325'),
('KK2021000005', '20200131');
INSERT INTO Orders ( customer_id, employee_id, invoice_id,
date_of_placing_the_order, order_realization_date, whether_the_order_fulfilled,
shipping_date)
VALUES
(1, 2, 1, '20180530', '20180602', 'true', '20180531'),
(2, 2, 2, '20200202', '20200210', 'false', '20200203'),
(2, 4, 3, '20200202', '20200210', 'false', '20200203'),
(3, 1, 4, '20190322', '20190325', 'true', '20190323'),
(5, 3, 5, '20200129', '20200131', 'true', '20200129');
INSERT INTO Ordered_products (order_id, product_id, quantity)
VALUES
(1, 3, 1),
(2, 3, 2),
(3, 2, 1),
(4, 5, 1),
(5, 6, 2);