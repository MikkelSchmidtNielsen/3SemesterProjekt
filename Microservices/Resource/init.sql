CREATE DATABASE IF NOT EXISTS resourcedb;
USE resourcedb;

-- Create Resource table
CREATE TABLE IF NOT EXISTS `Resource` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Name` VARCHAR(255) NOT NULL,
    `Type` VARCHAR(255) NOT NULL,
    `BasePrice` DECIMAL(10,2) NOT NULL,
    `Location` INT NOT NULL,
    `Description` TEXT NULL,
    `IsAvailable` BOOLEAN NOT NULL DEFAULT TRUE,
    `RowVersion` BINARY(8) NOT NULL
);