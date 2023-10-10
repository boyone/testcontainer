# README

0. Setup TypeScript Project

   ```sh
   mkdir testcontainer
   ```

   ```sh
   npm init
   yarn init
   ```

   ```sh
   npm install --save-dev jest typescript ts-jest @types/jest
   yarn add --dev jest typescript ts-jest @types/jest
   ```

   ```json
   "scripts": {
      "test": "jest"
   },
   ```

   ```sh
   npx ts-jest config:init
   yarn ts-jest config:init
   # jest.config.js
   ```

   ```sh
   npx tsc --init
   # tsconfig.json
   ```

   ```sh
   npm install ts-postgres
   npm install --save-dev @testcontainers/postgresql

   yarn install ts-postgres
   yarn install --dev @testcontainers/postgresql
   ```

1. Pull Target Container

   ```sh
   docker pull postgres:13.11-alpine3.17
   ```

2. Create Unit Test File Call `postgresql.test.js` to Test Connection with PostgreSql

   ```ts
   # postgresql.test.js
   import { Client } from 'ts-postgres';
   import {
     PostgreSqlContainer,
     StartedPostgreSqlContainer,
   } from '@testcontainers/postgresql';

   describe('test container', () => {
     let container: StartedPostgreSqlContainer;
     let client: Client;

     beforeAll(async () => {
       // new startedContainer instance
       container = await new PostgreSqlContainer(
         'postgres:13.11-alpine3.17'
       ).start();

       // new postgresql client instance
       client = new Client({
         host: container.getHost(),
         port: container.getPort(),
         database: container.getDatabase(),
         user: container.getUsername(),
         password: container.getPassword(),
       });
       await client.connect();
     });

     afterAll(async () => {
       // stop postgresql client instance
       await client.end();
       // stop startedContainer instance
       await container.stop();
     });

     it('should connect and return a query result', async () => {
       const result = await client.query('SELECT 1');
       expect(result.rows[0]).toEqual([1]);
     });
   });
   ```

3. Run Test

   ```sh
   npm test
   ```

4. Database Schema

   ```sql
   # initial-db/customer.sql
   CREATE TABLE customers (
       id SERIAL PRIMARY KEY,
       name VARCHAR NOT NULL UNIQUE
   );

   INSERT INTO customers(name)
   VALUES
       ('John'),
       ('Kirk'),
       ('Steve');
   ```

5. Add Database Schema to PostgreSql During Start Database Instance

   ```ts
   .withCopyDirectoriesToContainer([{
               source: "./initialDb",
               target: "/docker-entrypoint-initdb.d"
           }])
   ```

   - eg.

     ```ts
     container = await new PostgreSqlContainer('postgres:13.11-alpine3.17')
       .withCopyDirectoriesToContainer([
         {
           source: './initial-db',
           target: '/docker-entrypoint-initdb.d',
         },
       ])
       .start();
     ```

6. Add New Test to Check Total Number Customers in Database

   ```sh
   it("should return 3 customers", async () => {
        const result = await client.query("SELECT id, name from customers");
        expect(result.rows.length).toEqual(3);
    });
   ```

7. Run Test

   ```sh
   npm test
   ```
