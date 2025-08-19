package com.example.testmysqlcontainer.repo;

import com.example.testmysqlcontainer.entity.Country;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.jdbc.AutoConfigureTestDatabase;
import org.springframework.boot.test.autoconfigure.orm.jpa.DataJpaTest;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.testcontainers.service.connection.ServiceConnection;
import org.testcontainers.containers.MySQLContainer;
import org.testcontainers.junit.jupiter.Container;
import org.testcontainers.junit.jupiter.Testcontainers;
import org.testcontainers.utility.MountableFile;

import static org.junit.jupiter.api.Assertions.*;

@DataJpaTest()
@Testcontainers
@AutoConfigureTestDatabase(replace = AutoConfigureTestDatabase.Replace.NONE)
class CountryRepositoryTest {

    @Container
    @ServiceConnection
    private static final MySQLContainer mySQLContainer;

    static {
        mySQLContainer = new MySQLContainer<>("mysql:8.3.0");
        mySQLContainer.withCopyFileToContainer(MountableFile.forClasspathResource("db/init.sql"), "/docker-entrypoint-initdb.d/");
    }

    @Autowired
    private CountryRepository countryRepository;

    @Test
    public void InsertCountry() {
        Country country = new Country();
        country.setName("Thailand");
        country.setPopulation(60000000);

        Country savedCountry = countryRepository.save(country);

        assertNotNull(savedCountry.getId());
        assertEquals(1, savedCountry.getId());
        assertEquals(country.getName(), savedCountry.getName());
        assertEquals(country.getPopulation(), savedCountry.getPopulation());
    }
}