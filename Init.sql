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
