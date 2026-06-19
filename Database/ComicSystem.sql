IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'ComicSystem')
BEGIN
    CREATE DATABASE ComicSystem;
END
GO

USE ComicSystem;
GO

IF OBJECT_ID(N'dbo.Customers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Customers
    (
        CustomerID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        FullName NVARCHAR(255) NOT NULL,
        PhoneNumber NVARCHAR(15) NOT NULL,
        RegistrationDate DATETIME NOT NULL
    );
END
GO

IF OBJECT_ID(N'dbo.ComicBooks', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.ComicBooks
    (
        ComicBookID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Title NVARCHAR(255) NOT NULL,
        Author NVARCHAR(255) NOT NULL,
        PricePerDay DECIMAL(10, 2) NOT NULL
    );
END
GO

IF OBJECT_ID(N'dbo.Rentals', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Rentals
    (
        RentalID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        CustomerID INT NOT NULL,
        RentalDate DATETIME NOT NULL,
        ReturnDate DATETIME NOT NULL,
        Status NVARCHAR(50) NOT NULL,
        CONSTRAINT FK_Rentals_Customers FOREIGN KEY (CustomerID) REFERENCES dbo.Customers(CustomerID)
    );
END
GO

IF OBJECT_ID(N'dbo.RentalDetails', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.RentalDetails
    (
        RentalDetailID INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        RentalID INT NOT NULL,
        ComicBookID INT NOT NULL,
        Quantity INT NOT NULL,
        PricePerDay DECIMAL(10, 2) NOT NULL,
        CONSTRAINT FK_RentalDetails_Rentals FOREIGN KEY (RentalID) REFERENCES dbo.Rentals(RentalID),
        CONSTRAINT FK_RentalDetails_ComicBooks FOREIGN KEY (ComicBookID) REFERENCES dbo.ComicBooks(ComicBookID)
    );
END
GO
