USE HospitalManagement
GO

INSERT INTO [Roles]
(Name)
VALUES
('Admin'),
('Patient'),
('Caregiver'),
('Doctor')
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('admin@admin.com', 'admin', 'admin', 35, 'Male', 'Admin NRIC', '', 'Address', '48572947', 'Admin', 1)
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('p1@p1.com', 'patient', 'patient', 25, 'Female', 'Patient NRIC', '', 'Address', '48572947', 'Imran', 2)
GO

INSERT INTO [Patients]
([Patients].Disease, [Patients].Occupation, [Patients].User_Id, [Patients].EntryDate, [Patients].EntryTime)
VALUES
('Flu', 'Engineer', 2, '20170618 10:34:09 AM', '10:34:09 AM')
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('p2@p2.com', 'patient', 'patient', 25, 'Male', 'Patient NRIC', '', 'Address', '48572947', 'P1', 2)
GO

INSERT INTO [Patients]
([Patients].Disease, [Patients].Occupation, [Patients].User_Id, [Patients].EntryDate, [Patients].EntryTime)
VALUES
('Flu', 'Engineer', 3, '20170218 10:34:09 AM', '10:34:09 AM')
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('p3@p3.com', 'patient', 'patient', 25, 'Female', 'Patient NRIC', '', 'Address', '48572947', 'P3', 2)
GO

INSERT INTO [Patients]
([Patients].Disease, [Patients].Occupation, [Patients].User_Id, [Patients].EntryDate, [Patients].EntryTime)
VALUES
('Flu', 'Engineer', 4, '20170618 10:34:09 AM', '10:34:09 AM')
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('p4@p2.com', 'patient', 'patient', 25, 'Male', 'Patient NRIC', '', 'Address', '48572947', 'P4', 2)
GO

INSERT INTO [Patients]
([Patients].Disease, [Patients].Occupation, [Patients].User_Id, [Patients].EntryDate, [Patients].EntryTime)
VALUES
('Flu', 'Engineer', 5, '20170218 10:34:09 AM', '10:34:09 AM')
GO

INSERT INTO [Users] 
([Users].Email, [Users].Password, [Users].UserName, [Users].Age, [Users].Gender, [Users].NRIC, [Users].Comments, [Users].Address, [Users].ContactNo, [Users].FullName, [Users].Role_Id)
VALUES
('p5@p5.com', 'patient', 'patient', 25, 'Female', 'Patient NRIC', '', 'Address', '48572947', 'P5', 2)
GO

INSERT INTO [Patients]
([Patients].Disease, [Patients].Occupation, [Patients].User_Id, [Patients].EntryDate, [Patients].EntryTime)
VALUES
('Flu', 'Engineer', 6, '20170618 10:34:09 AM', '10:34:09 AM')
GO
