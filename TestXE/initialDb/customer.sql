ALTER SESSION SET CONTAINER=XEPDB1;

CREATE TABLE customers (
    id NUMBER GENERATED ALWAYS as IDENTITY(START with 1 INCREMENT by 1),
    name VARCHAR2(50) not null
);

INSERT INTO customers(name) VALUES('John');
INSERT INTO customers(name) VALUES('Kirk');
INSERT INTO customers(name) VALUES('Steve');

COMMIT;

-- GRANT CONNECT, RESOURCE TO appuser;
-- GRANT ALL ON customers TO SYSTEM;


SELECT * from customers;