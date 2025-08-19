package com.example.testmysqlcontainer;

import org.junit.jupiter.api.Disabled;
import org.springframework.boot.SpringApplication;

public class TestTestmysqlcontainerApplication {

	public static void main(String[] args) {
		SpringApplication.from(TestmysqlcontainerApplication::main).with(TestcontainersConfiguration.class).run(args);
	}

}
