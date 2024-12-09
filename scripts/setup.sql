CREATE DATABASE Referly;
GO

CREATE SCHEMA Job;
GO

USE Referly;
GO

CREATE TABLE Job.Auth (
    id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL UNIQUE, 
    PasswordKey VARBINARY(255),          
    PasswordSalt VARBINARY(255)        
);
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