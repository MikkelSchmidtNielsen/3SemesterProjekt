USE authdb;

-- Create user tabel
CREATE TABLE [User] (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Email VARCHAR(50) NOT NULL,
);