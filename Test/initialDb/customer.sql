CREATE TABLE customers (
    id SERIAL PRIMARY KEY,
    name VARCHAR NOT NULL UNIQUE
);

INSERT INTO customers(name)
VALUES
    ('John'),
    ('Kirk'),
    ('Steve');

ALTER TABLE customers OWNER TO myadmin;