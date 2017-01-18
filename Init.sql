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
([Patients].Disease, [Patients].Occupation, [Patients].User_Id)
VALUES
('Flu', 'Engineer', 1)
GO
