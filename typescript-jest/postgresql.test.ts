import { Client } from 'ts-postgres';
import { PostgreSqlContainer, StartedPostgreSqlContainer } from '@testcontainers/postgresql';

describe("test container", () => {
    let container: StartedPostgreSqlContainer;
    let client: Client;

    beforeAll(async () => {
        container = await new PostgreSqlContainer("postgres:13.11-alpine3.17")
            .withCopyDirectoriesToContainer([{
                source: "./initial-db",
                target: "/docker-entrypoint-initdb.d"
            }])
            .start();

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
        await client.end();
        await container.stop();
    });

    it("should connect and return a query result", async () => {
        const result = await client.query("SELECT 1");
        expect(result.rows[0]).toEqual([1]);
    });

    it("should return 3 customers", async () => {
        const result = await client.query("SELECT id, name from customers");
        expect(result.rows.length).toEqual(3);
    });
});




