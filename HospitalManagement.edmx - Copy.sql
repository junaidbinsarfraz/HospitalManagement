
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 01/17/2017 17:19:58
-- Generated from EDMX file: C:\Users\Junaid\Downloads\HospitalManagament\HospitalManagement.edmx
-- --------------------------------------------------

USE master
GO

--Create a database
IF EXISTS(SELECT name FROM sys.databases
    WHERE name = 'HospitalManagement')
    DROP DATABASE HospitalManagement
GO

CREATE DATABASE HospitalManagement
GO

SET QUOTED_IDENTIFIER OFF;
GO
USE [HospitalManagement];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

ALTER DATABASE [HospitalManagement] SET COMPATIBILITY_LEVEL = 100
GO

ALTER DATABASE [HospitalManagement] SET  ENABLE_BROKER 
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Password] nvarchar(max)  NOT NULL,
    [UserName] nvarchar(max)  NOT NULL,
    [Age] bigint  NOT NULL,
    [Gender] bit  NOT NULL,
    [NRIC] nvarchar(max)  NOT NULL,
    [Comments] nvarchar(max)  NOT NULL,
    [Address] nvarchar(max)  NOT NULL,
    [ContactNo] nvarchar(max)  NOT NULL,
    [FullName] nvarchar(max)  NOT NULL,
    [Role_Id] bigint  NOT NULL
);
GO

-- Creating table 'Patients'
CREATE TABLE [dbo].[Patients] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Disease] nvarchar(max)  NOT NULL,
    [Occupation] nvarchar(max)  NOT NULL,
    [User_Id] bigint  NOT NULL
);
GO

-- Creating table 'Caregivers'
CREATE TABLE [dbo].[Caregivers] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [User_Id] bigint  NOT NULL,
    [Patient_Id] bigint  NULL
);
GO

-- Creating table 'Doctors'
CREATE TABLE [dbo].[Doctors] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Designation] nvarchar(max)  NOT NULL,
    [Specialization] nvarchar(max)  NOT NULL,
    [User_Id] bigint  NOT NULL
);
GO

-- Creating table 'Roles'
CREATE TABLE [dbo].[Roles] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [PK_Patients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Caregivers'
ALTER TABLE [dbo].[Caregivers]
ADD CONSTRAINT [PK_Caregivers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Doctors'
ALTER TABLE [dbo].[Doctors]
ADD CONSTRAINT [PK_Doctors]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Roles'
ALTER TABLE [dbo].[Roles]
ADD CONSTRAINT [PK_Roles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [User_Id] in table 'Patients'
ALTER TABLE [dbo].[Patients]
ADD CONSTRAINT [FK_UserPatient]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserPatient'
CREATE INDEX [IX_FK_UserPatient]
ON [dbo].[Patients]
    ([User_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Caregivers'
ALTER TABLE [dbo].[Caregivers]
ADD CONSTRAINT [FK_UserCaregiver]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserCaregiver'
CREATE INDEX [IX_FK_UserCaregiver]
ON [dbo].[Caregivers]
    ([User_Id]);
GO

-- Creating foreign key on [User_Id] in table 'Doctors'
ALTER TABLE [dbo].[Doctors]
ADD CONSTRAINT [FK_UserDoctor]
    FOREIGN KEY ([User_Id])
    REFERENCES [dbo].[Users]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserDoctor'
CREATE INDEX [IX_FK_UserDoctor]
ON [dbo].[Doctors]
    ([User_Id]);
GO

-- Creating foreign key on [Patient_Id] in table 'Caregivers'
ALTER TABLE [dbo].[Caregivers]
ADD CONSTRAINT [FK_PatientCaregiver]
    FOREIGN KEY ([Patient_Id])
    REFERENCES [dbo].[Patients]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PatientCaregiver'
CREATE INDEX [IX_FK_PatientCaregiver]
ON [dbo].[Caregivers]
    ([Patient_Id]);
GO

-- Creating foreign key on [Role_Id] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [FK_UserRole]
    FOREIGN KEY ([Role_Id])
    REFERENCES [dbo].[Roles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserRole'
CREATE INDEX [IX_FK_UserRole]
ON [dbo].[Users]
    ([Role_Id]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------