package com.example.testmysqlcontainer.repo;

import com.example.testmysqlcontainer.entity.Country;
import org.springframework.data.jpa.repository.JpaRepository;

public interface CountryRepository extends JpaRepository<Country, Long> {
}
