CREATE DATABASE IF NOT EXISTS authdb;
USE authdb;

-- Create user tabel
CREATE TABLE IF NOT EXISTS `User` (
    `Id` INT AUTO_INCREMENT PRIMARY KEY,
    `Email` VARCHAR(255) NOT NULL,
    'Otp' INT,
    'OtpExpiryTime' DATETIME
);