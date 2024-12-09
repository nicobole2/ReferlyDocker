-- Wait for SQL Server to start up
WAITFOR DELAY '00:00:30';
GO

-- Create database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'Referly')
BEGIN
    CREATE DATABASE Referly;
END
GO

USE Referly;
GO

-- Create Job schema if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'Job')
BEGIN
    EXEC('CREATE SCHEMA Job');
END
GO

CREATE TABLE Job.Auth (
    id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL UNIQUE, 
    PasswordKey VARBINARY(255),          
    PasswordSalt VARBINARY(255)        
);
GO

CREATE OR ALTER PROCEDURE ManageAuth
    @Email NVARCHAR(255),
    @PasswordKey VARBINARY(255),
    @PasswordSalt VARBINARY(255),
AS
BEGIN
    SET NOCOUNT ON;

    -- Verifica si ya existe un registro con el email proporcionado
    IF EXISTS (SELECT 1 FROM Job.Auth WHERE Email = @Email)
    BEGIN
        -- Actualiza el registro existente
        UPDATE Job.Auth
        SET 
            PasswordKey = @PasswordKey,
            PasswordSalt = @PasswordSalt
        WHERE Email = @Email;

        PRINT 'Registro actualizado con éxito.';
    END
    ELSE
    BEGIN
        -- Inserta un nuevo registro
        INSERT INTO Job.Auth (
            Email, PasswordKey, PasswordSalt
        )
        VALUES (
            @Email, @PasswordKey, @PasswordSalt
        );

        PRINT 'Nuevo registro creado con éxito.';
    END

    -- Retorna el ID del registro procesado
    SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

CREATE TABLE Job.Company (
    id INT PRIMARY KEY IDENTITY(1,1),
    registrationDate DATETIME,
    status NVARCHAR(50),
    name NVARCHAR(100),
    taxId NVARCHAR(20)
);
GO

CREATE TABLE Job.CompanyContact (
    id INT PRIMARY KEY IDENTITY(1,1),
    registrationDate DATETIME,
    firstName NVARCHAR(50),
    lastName NVARCHAR(50),
    identificationNumber NVARCHAR(20),
    birthDate DATETIME,
    gender NVARCHAR(10),
    email NVARCHAR(100),
    password NVARCHAR(100),
    countryOfResidence NVARCHAR(50),
    cityOfResidence NVARCHAR(50),
    companyId INT,
    FOREIGN KEY (companyId) REFERENCES Company(id)
);
GO



CREATE TABLE Job.Responsibilities (
    id INT PRIMARY KEY IDENTITY(1,1),
    jobReferenceId INT,
    description NVARCHAR(MAX),
    FOREIGN KEY (jobReferenceId) REFERENCES Job.JobSearch(referenceId)
);
GO

CREATE TABLE Job.Requirements (
    id INT PRIMARY KEY IDENTITY(1,1),
    jobReferenceId INT,
    description NVARCHAR(MAX),
    FOREIGN KEY (jobReferenceId) REFERENCES Job.JobSearch(referenceId)
);
GO

CREATE TABLE Job.JobSearch (
    id INT PRIMARY KEY IDENTITY(1,1),
    published DATETIME,
    positionName NVARCHAR(100),
    category NVARCHAR(50),
    subcategory NVARCHAR(50),
    referenceId INT,
    description NVARCHAR(MAX),
    companyContactId INT,
    share DECIMAL(10, 2),
    state NVARCHAR(50),
    workMode NVARCHAR(50),
    area NVARCHAR(50),
    vacancies INT NOT NULL,
    city NVARCHAR(50),
    level NVARCHAR(50),
    FOREIGN KEY (companyContactId) REFERENCES CompanyContact(id)
);
GO

CREATE TABLE Job.Hunters (
    id INT PRIMARY KEY IDENTITY(1,1),
    email NVARCHAR(100),
    registrationDate DATETIME,
    firstName NVARCHAR(50),
    lastName NVARCHAR(50),
    --identificationNumber NVARCHAR(20),
    birthDate DATETIME,
    --gender NVARCHAR(10),
    countryOfResidence NVARCHAR(50),
    cityOfResidence NVARCHAR(50),
    linkedinProfile NVARCHAR(200),
    status NVARCHAR(50),
    shortSummary NVARCHAR(MAX),
    rating FLOAT,
    --profilePicture NVARCHAR(200),
    bankAccount NVARCHAR(50),
    --tags NVARCHAR(200),
    phone NVARCHAR(20)
);
GO

CREATE OR ALTER PROCEDURE ManageHunter
    @Email NVARCHAR(100),
   -- @RegistrationDate DATETIME,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
   -- @BirthDate DATETIME,
   -- @CountryOfResidence NVARCHAR(50),
   -- @CityOfResidence NVARCHAR(50),
    @LinkedinProfile NVARCHAR(200),
    --@Status NVARCHAR(50),
    @ShortSummary NVARCHAR(MAX),
   -- @Rating FLOAT,
   -- @BankAccount NVARCHAR(50),
    @Phone NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    -- Verifica si el email ya existe
    IF EXISTS (SELECT 1 FROM Job.Hunters WHERE email = @Email)
    BEGIN
        -- Actualiza los valores del registro existente
        UPDATE Job.Hunters
        SET 
            registrationDate =GETDATE(),
            firstName = @FirstName,
            lastName = @LastName,
            -- birthDate = @BirthDate,
           -- countryOfResidence = @CountryOfResidence,
           -- cityOfResidence = @CityOfResidence,
            linkedinProfile = @LinkedinProfile,
           -- status = @Status,
            status = 'ACTIVE',
            shortSummary = @ShortSummary,
           -- rating = @Rating,
            rating = 0,
           -- bankAccount = @BankAccount,
            bankAccount = 0,
            phone = @Phone
        WHERE email = @Email;
        PRINT 'Registro actualizado con éxito.';
    END
    ELSE
    BEGIN
        -- Inserta un nuevo registro
        INSERT INTO Job.Hunters (
            email, registrationDate, firstName, lastName, --birthDate, 
           -- countryOfResidence, cityOfResidence,
             linkedinProfile, 
            status, shortSummary, rating, bankAccount, phone
        )
        VALUES (
            @Email, GETDATE(), @FirstName, @LastName,-- @BirthDate, 
           -- @CountryOfResidence, @CityOfResidence,
             @LinkedinProfile, 
            'ACTIVE', @ShortSummary, 0, 0, @Phone
        );

        PRINT 'Nuevo registro creado con éxito.';
    END

    -- Retorna el userID
     SELECT SCOPE_IDENTITY() AS NewId;
END;
GO

CREATE TABLE Job.JobSearch (
    id INT PRIMARY KEY IDENTITY(1,1),
    published DATETIME,
    positionName NVARCHAR(100),
    category NVARCHAR(50),
    subcategory NVARCHAR(50),
    referenceId INT UNIQUE, -- Restringe valores únicos
    description NVARCHAR(MAX),
    companyContactId INT,
    share DECIMAL(10, 2),
    state NVARCHAR(50),
    workMode NVARCHAR(50),
    area NVARCHAR(50),
    vacancies INT NOT NULL,
    city NVARCHAR(50),
    level NVARCHAR(50),
    FOREIGN KEY (companyContactId) REFERENCES CompanyContact(id)
);
GO

CREATE TABLE Job.Referrals (
    [id]             INT             IDENTITY (1, 1) NOT NULL,
    [email]          NVARCHAR (100)  NOT NULL,
    [jobSearch]      INT             NOT NULL,
    [hunter]         INT             NOT NULL,
    [cvLink]         NVARCHAR (200)  NULL,
    [description]    NVARCHAR (MAX)  NULL,
    [status]         NVARCHAR (50)   NULL,
    [lastStatus]     NVARCHAR (50)   NULL,
    [cvRating]       FLOAT (53)      NULL,
    [firstName]      NVARCHAR (50)   NOT NULL,
    [lastName]       NVARCHAR (50)   NOT NULL,
    [pdfFile]        VARBINARY (MAX) NOT NULL,
    [pdfFileName]    NVARCHAR (255)  NOT NULL,
    [pdfContentType] NVARCHAR (50)   NOT NULL,
    [creationDate]   DATE            NULL,
    PRIMARY KEY CLUSTERED ([id] ASC)
);

CREATE TABLE Job.ReferralFiles (
    id INT PRIMARY KEY IDENTITY(1,1),
    referralEmail NVARCHAR(100),
    pdfFile VARBINARY(MAX),
    pdfFileName NVARCHAR(255),
    pdfContentType NVARCHAR(50),
);
GO

CREATE Table Job.Contact(
    id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(255),
    email NVARCHAR(255),
    subject NVARCHAR(255),
    message NVARCHAR(255)
)
GO


GRANT CONNECT TO [sa];
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::Job TO [sa];
GO