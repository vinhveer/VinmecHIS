-- Insert Employees
INSERT INTO EMPLOYEE (FIRST_NAME, LAST_NAME, DATE_OF_BIRTH, GENDER, EMPLOYEE_EMAIL, EMPLOYEE_PHONE, EMPLOYEE_ADDRESS, ROLE_NAME, HIRE_DATE, EMPLOYEE_ROOM, DEPARTMENT) VALUES
('John', 'Smith', '1980-05-15', 'M', 'john.smith@hospital.com', '123-456-7890', '123 Medical Drive, City', 'Doctor', '2015-01-15', 'Room 101', 'Cardiology'),
('Sarah', 'Johnson', '1985-08-22', 'F', 'sarah.j@hospital.com', '123-456-7891', '456 Health Ave, City', 'Doctor', '2016-03-20', 'Room 102', 'Pediatrics'),
('Michael', 'Williams', '1975-11-30', 'M', 'michael.w@hospital.com', '123-456-7892', '789 Care Street, City', 'Doctor', '2014-06-10', 'Room 103', 'Orthopedics'),
('Emily', 'Brown', '1982-04-12', 'F', 'emily.b@hospital.com', '123-456-7893', '321 Wellness Road, City', 'Doctor', '2017-09-05', 'Room 104', 'Dermatology'),
('David', 'Jones', '1978-07-25', 'M', 'david.j@hospital.com', '123-456-7894', '654 Hospital Lane, City', 'Doctor', '2013-12-15', 'Room 105', 'Neurology'),
('Jennifer', 'Davis', '1983-09-18', 'F', 'jennifer.d@hospital.com', '123-456-7895', '987 Doctor Street, City', 'Doctor', '2018-02-28', 'Room 106', 'Internal Medicine'),
('Robert', 'Wilson', '1990-02-14', 'M', 'robert.w@hospital.com', '123-456-7896', '147 Staff Road, City', 'Receptionist', '2019-05-10', 'Front Desk 1', 'Administration'),
('Lisa', 'Anderson', '1988-06-30', 'F', 'lisa.a@hospital.com', '123-456-7897', '258 Admin Ave, City', 'Pharmacist', '2017-08-15', 'Pharmacy 1', 'Pharmacy'),
('James', 'Taylor', '1992-03-25', 'M', 'james.t@hospital.com', '123-456-7898', '369 Reception Lane, City', 'Receptionist', '2020-01-20', 'Front Desk 2', 'Administration'),
('Mary', 'Martinez', '1987-12-08', 'F', 'mary.m@hospital.com', '123-456-7899', '741 Staff Street, City', 'Nurse', '2016-11-30', 'Station 1', 'Nursing'),
('William', 'Thompson', '1979-10-05', 'M', 'william.t@hospital.com', '123-456-7900', '852 Doctor Ave, City', 'Doctor', '2015-07-22', 'Room 107', 'Cardiology'),
('Elizabeth', 'White', '1984-01-15', 'F', 'elizabeth.w@hospital.com', '123-456-7901', '963 Medical Street, City', 'Doctor', '2018-04-15', 'Room 108', 'Pediatrics'),
('Thanh', 'Tri', '2004-05-15', 'M', 'john.smith@hospital.com', '123-456-7890', '123 Medical Drive, City', 'Admin', '2015-01-15', 'Admin Room', 'IT');
GO

-- Insert Patients
INSERT INTO PATIENT (FIRST_NAME, LAST_NAME, DATE_OF_BIRTH, GENDER, PATIENT_EMAIL, PATIENT_PHONE, PATIENT_ADDRESS, EMERGENCY_CONTACT, REGISTRATION_DATE) VALUES
('Thomas', 'Anderson', '1990-03-20', 'M', 'thomas.a@email.com', '12345', '123 Patient St, City', '555-0101', '2023-01-15'),
('Emma', 'Wilson', '1985-07-12', 'F', 'emma.w@email.com', '12346', '456 Client Ave, City', '555-0102', '2023-02-20'),
('Oliver', 'Brown', '1978-11-30', 'M', 'oliver.b@email.com', '12347', '789 Customer Rd, City', '555-0103', '2023-03-10'),
('Sophia', 'Taylor', '1995-04-25', 'F', 'sophia.t@email.com', '12348', '321 Health St, City', '555-0104', '2023-04-05'),
('Lucas', 'Martinez', '1982-09-15', 'M', 'lucas.m@email.com', '12349', '654 Care Ave, City', '555-0105', '2023-05-15'),
('Isabella', 'Garcia', '1993-06-08', 'F', 'isabella.g@email.com', '12350', '987 Wellness Rd, City', '555-0106', '2023-06-20'),
('Mason', 'Lopez', '1975-12-03', 'M', 'mason.l@email.com', '12351', '147 Treatment St, City', '555-0107', '2023-07-25'),
('Ava', 'Lee', '1988-02-28', 'F', 'ava.l@email.com', '12352', '258 Medical Dr, City', '555-0108', '2023-08-30'),
('Ethan', 'Walker', '1992-08-17', 'M', 'ethan.w@email.com', '12353', '369 Healthcare Ln, City', '555-0109', '2023-09-10'),
('Mia', 'Hall', '1987-05-22', 'F', 'mia.h@email.com', '12354', '741 Clinic Way, City', '555-0110', '2023-10-15'),
('Noah', 'Allen', '1983-10-11', 'M', 'noah.a@email.com', '12355', '852 Hospital Rd, City', '555-0111', '2023-11-20'),
('Charlotte', 'Young', '1991-01-14', 'F', 'charlotte.y@email.com', '12356', '963 Doctor Ave, City', '555-0112', '2023-12-25');
go

-- Insert Suppliers
INSERT INTO SUPPLIER (SUPPLIER_NAME, SUPPLIER_ADDRESS, CONTACT_EMAIL, CONTACT_PHONE) VALUES
('MedSupply Co', '123 Supply St, City', 'contact@medsupply.com', '555-1111'),
('PharmaCare Ltd', '456 Pharma Ave, City', 'contact@pharmacare.com', '555-2222'),
('Healthcare Supplies Inc', '789 Health Supplies Rd, City', 'contact@healthsupplies.com', '555-3333');
go

-- Insert Medicines (After Suppliers because of FK constraint)
INSERT INTO MEDICINE (SUPPLIER_ID, MEDICINE_NAME, UNIT, STOCK_QUANTITY, EXPIRATION_DATE, PRICE) VALUES
(1, 'Paracetamol', 'tablets', 1000, '2025-12-31', 5.99),
(1, 'Amoxicillin', 'capsules', 500, '2025-06-30', 12.99),
(2, 'Ibuprofen', 'tablets', 750, '2025-09-30', 7.99),
(2, 'Omeprazole', 'capsules', 300, '2025-08-31', 15.99),
(3, 'Metformin', 'tablets', 600, '2025-07-31', 9.99);
go

-- After Employee and Patient tables are populated with actual IDs, insert Medical Records
INSERT INTO MEDICALRECORD (EMPLOYEE_ID, PATIENT_ID, EXAMINATION_DATE, DIAGNOSIS, TREATMENT, FOLLOW_UP_DATE, ADDITIONAL_NOTES, HOSPITAL_FEES) VALUES
(1, 1, '2024-01-10', 'Hypertension', 'Prescribed blood pressure medication', '2024-02-10', 'Monitor blood pressure daily', 150.00),
(2, 2, '2024-01-11', 'Acute bronchitis', 'Antibiotics prescribed', '2024-01-25', 'Rest and plenty of fluids', 120.00),
(3, 3, '2024-01-12', 'Sprained ankle', 'RICE treatment recommended', '2024-01-26', 'Use crutches for 1 week', 95.00);
go

-- Insert Prescriptions for the Medical Records
INSERT INTO PRESCRIPTION VALUES
(1, '2024-01-10', 'Take with food'),
(2, '2024-01-11', 'Complete full course of antibiotics'),
(3, '2024-01-12', 'Take as needed for pain');
go

-- Insert Prescription Details
INSERT INTO PRESCRIPTIONDETAIL (MEDICAL_RECORD_ID, MEDICINE_ID, PRESCRIBED_QUANTITY, DOSAGE) VALUES
(1, 1, 30, '1 tablet twice daily'),
(2, 2, 20, '1 capsule three times daily'),
(3, 3, 15, '1 tablet as needed for pain');
GO

-- Insert Employee Accounts with auto-incrementing IDs
INSERT INTO EMPLOYEEACCOUNT (EMPLOYEE_USERNAME, EMPLOYEE_PASSWORD) VALUES
('jsmith', '414e7c8ede73a0c2c3d17699134f4080'),
('sjohnson', '414e7c8ede73a0c2c3d17699134f4080'),
('mwilliams', '414e7c8ede73a0c2c3d17699134f4080'),
('ebrown', '414e7c8ede73a0c2c3d17699134f4080'),
('djones', '414e7c8ede73a0c2c3d17699134f4080'),
('jdavis', '414e7c8ede73a0c2c3d17699134f4080'),
('rwilson', '414e7c8ede73a0c2c3d17699134f4080'),
('landerson', '414e7c8ede73a0c2c3d17699134f4080'),
('jtaylor', '414e7c8ede73a0c2c3d17699134f4080'),
('mmartinez', '414e7c8ede73a0c2c3d17699134f4080'),
('wthompson', '414e7c8ede73a0c2c3d17699134f4080'),
('ewhite', '414e7c8ede73a0c2c3d17699134f4080'),
('admin', '414e7c8ede73a0c2c3d17699134f4080');
go 

-- Insert Patient Accounts
INSERT INTO PATIENTACCOUNT (PATIENT_ID, PATIENT_USERNAME, PATIENT_PASSWORD) VALUES
(1, 'tanderson', '414e7c8ede73a0c2c3d17699134f4080'),
(2, 'ewilson', '414e7c8ede73a0c2c3d17699134f4080'),
(3, 'obrown', '414e7c8ede73a0c2c3d17699134f4080'),
(4, 'staylor', '414e7c8ede73a0c2c3d17699134f4080'),
(5, 'lmartinez', '414e7c8ede73a0c2c3d17699134f4080'),
(6, 'igarcia', '414e7c8ede73a0c2c3d17699134f4080'),
(7, 'mlopez', '414e7c8ede73a0c2c3d17699134f4080'),
(8, 'alee', '414e7c8ede73a0c2c3d17699134f4080'),
(9, 'ewalker', '414e7c8ede73a0c2c3d17699134f4080'),
(10, 'mhall', '414e7c8ede73a0c2c3d17699134f4080'),
(11, 'nallen', '414e7c8ede73a0c2c3d17699134f4080'),
(12, 'cyoung', '414e7c8ede73a0c2c3d17699134f4080');
go

-- Insert Sample Medicine Orders
INSERT INTO MEDICINEORDER (EMPLOYEE_ID, SUPPLIER_ID, ORDER_DATE) VALUES
(8, 1, '2024-01-10'),  -- Ordered by pharmacist Lisa Anderson
(8, 2, '2024-01-11');
go

-- Insert Medicine Order Details
INSERT INTO MEDICINEORDERDETAIL (MEDICINE_ORDER_ID, MEDICINE_ID, ORDERED_QUANTITY) VALUES
(1, 1, 500),  -- Ordering Paracetamol
(1, 2, 200),  -- Ordering Amoxicillin
(2, 3, 300);  -- Ordering Ibuprofen
go

INSERT INTO [Vinmec].[dbo].[MEDICINE] 
([SUPPLIER_ID], [MEDICINE_NAME], [UNIT], [STOCK_QUANTITY], [EXPIRATION_DATE], [PRICE]) 
VALUES
(1, 'Aspirin', 'tablets', 750, '2025-10-10 00:00:00', 6750),
(2, 'Cough Syrup', 'bottle', 200, '2024-08-20 00:00:00', 12999),
(3, 'Vitamin C', 'tablets', 1500, '2027-01-01 00:00:00', 3990),
(1, 'Antibiotics', 'capsules', 300, '2025-05-15 00:00:00', 9990),
(2, 'Pain Relief Gel', 'tube', 120, '2024-12-01 00:00:00', 7500),
(1, 'Allergy Relief', 'tablets', 500, '2026-03-12 00:00:00', 6490),
(2, 'Cough Lozenges', 'pack', 400, '2025-11-10 00:00:00', 8750),
(3, 'Antacid', 'tablets', 900, '2026-02-28 00:00:00', 5250),
(1, 'Eye Drops', 'bottle', 250, '2024-07-30 00:00:00', 10999),
(2, 'Fever Reducer', 'tablets', 1000, '2025-08-01 00:00:00', 3750),
(3, 'Muscle Relaxant', 'capsules', 150, '2025-12-31 00:00:00', 11999),
(1, 'Cold Medicine', 'syrup', 180, '2024-11-20 00:00:00', 9500),
(2, 'Skin Ointment', 'tube', 300, '2026-06-15 00:00:00', 7250),
(3, 'Antifungal Cream', 'tube', 220, '2025-04-01 00:00:00', 6990),
(1, 'Ear Drops', 'bottle', 120, '2024-10-10 00:00:00', 8500),
(2, 'Laxative', 'tablets', 350, '2026-01-10 00:00:00', 5500),
(3, 'Antihistamine', 'tablets', 600, '2025-11-30 00:00:00', 4750),
(1, 'Burn Cream', 'tube', 180, '2025-07-25 00:00:00', 9250),
(2, 'Diabetes Medicine', 'tablets', 800, '2027-03-01 00:00:00', 15999),
(3, 'High Blood Pressure Medicine', 'tablets', 500, '2026-08-20 00:00:00', 14999),
(1, 'Cholesterol Lowering Medicine', 'capsules', 400, '2026-05-10 00:00:00', 13999),
(2, 'Sleep Aid', 'tablets', 300, '2024-09-05 00:00:00', 7990),
(3, 'Stomach Pain Relief', 'tablets', 900, '2025-06-10 00:00:00', 5750),
(1, 'Migraine Relief', 'capsules', 150, '2026-12-01 00:00:00', 8990),
(2, 'Digestive Enzymes', 'capsules', 400, '2025-03-20 00:00:00', 10500),
(3, 'Probiotic Supplement', 'capsules', 800, '2027-01-15 00:00:00', 12499),
(1, 'Iron Supplement', 'tablets', 600, '2026-10-10 00:00:00', 9990),
(2, 'Vitamin D', 'tablets', 1000, '2027-02-05 00:00:00', 4500),
(3, 'Calcium Supplement', 'tablets', 700, '2027-04-20 00:00:00', 6990);
GO

UPDATE [Vinmec].[dbo].[MEDICINE]
SET [PRICE] = 5990
WHERE [MEDICINE_ID] = 1;
GO

UPDATE [Vinmec].[dbo].[MEDICINE]
SET [PRICE] = 12990
WHERE [MEDICINE_ID] = 2;
GO

UPDATE [Vinmec].[dbo].[MEDICINE]
SET [PRICE] = 7990
WHERE [MEDICINE_ID] = 3;
GO

UPDATE [Vinmec].[dbo].[MEDICINE]
SET [PRICE] = 15990
WHERE [MEDICINE_ID] = 4;
GO

UPDATE [Vinmec].[dbo].[MEDICINE]
SET [PRICE] = 9990
WHERE [MEDICINE_ID] = 5;
GO

INSERT INTO [Vinmec].[dbo].[EMPLOYEE] 
([FIRST_NAME], [LAST_NAME], [DATE_OF_BIRTH], [GENDER], [EMPLOYEE_EMAIL], [EMPLOYEE_PHONE], [EMPLOYEE_ADDRESS], [ROLE_NAME], [HIRE_DATE], [EMPLOYEE_ROOM], [DEPARTMENT]) 
VALUES
('Andrew', 'Miller', '1981-03-15', 'M', 'andrew.m@hospital.com', '123-456-8001', '101 Medical Plaza, City', 'Doctor', '2016-04-20', 'Room 109', 'Cardiology'),
('Sophia', 'Clark', '1986-09-12', 'F', 'sophia.c@hospital.com', '123-456-8002', '202 Health Drive, City', 'Doctor', '2017-11-05', 'Room 110', 'Pediatrics'),
('Christopher', 'Rodriguez', '1974-11-18', 'M', 'chris.r@hospital.com', '123-456-8003', '303 Care Street, City', 'Doctor', '2014-01-30', 'Room 111', 'Orthopedics'),
('Olivia', 'Walker', '1983-05-25', 'F', 'olivia.w@hospital.com', '123-456-8004', '404 Wellness Blvd, City', 'Doctor', '2018-06-15', 'Room 112', 'Dermatology'),
('Daniel', 'Perez', '1979-08-05', 'M', 'daniel.p@hospital.com', '123-456-8005', '505 Hospital Lane, City', 'Doctor', '2013-10-10', 'Room 113', 'Neurology'),
('Emily', 'Young', '1985-12-11', 'F', 'emily.y@hospital.com', '123-456-8006', '606 Doctor Ave, City', 'Doctor', '2019-02-18', 'Room 114', 'Internal Medicine'),
('Matthew', 'Scott', '1991-07-19', 'M', 'matthew.s@hospital.com', '123-456-8007', '707 Staff Road, City', 'Doctor', '2020-03-22', 'Room 115', 'Cardiology'),
('Grace', 'Harris', '1989-01-09', 'F', 'grace.h@hospital.com', '123-456-8008', '808 Health Ave, City', 'Doctor', '2017-09-12', 'Room 116', 'Pediatrics'),
('Ethan', 'Hall', '1982-10-21', 'M', 'ethan.h@hospital.com', '123-456-8009', '909 Care Blvd, City', 'Doctor', '2015-06-08', 'Room 117', 'Orthopedics'),
('Isabella', 'Allen', '1988-02-28', 'F', 'isabella.a@hospital.com', '123-456-8010', '1010 Wellness Lane, City', 'Doctor', '2016-08-19', 'Room 118', 'Dermatology'),
('Liam', 'King', '1990-04-10', 'M', 'liam.k@hospital.com', '123-456-8011', '123 Pharmacy Lane, City', 'Pharmacist', '2018-07-01', 'Pharmacy 2', 'Pharmacy'),
('Emma', 'Davis', '1987-11-22', 'F', 'emma.d@hospital.com', '123-456-8012', '456 Drug Store, City', 'Pharmacist', '2019-01-10', 'Pharmacy 3', 'Pharmacy'),
('James', 'Miller', '1985-02-14', 'M', 'james.m@hospital.com', '123-456-8013', '789 Medical Plaza, City', 'Pharmacist', '2020-03-25', 'Pharmacy 4', 'Pharmacy'),
('Sophia', 'Martinez', '1992-06-05', 'F', 'sophia.m@hospital.com', '123-456-8014', '101 Care Blvd, City', 'Pharmacist', '2017-11-22', 'Pharmacy 5', 'Pharmacy'),
('Lucas', 'Gonzalez', '1988-10-20', 'M', 'lucas.g@hospital.com', '123-456-8015', '202 Health Road, City', 'Pharmacist', '2016-05-15', 'Pharmacy 6', 'Pharmacy'),
('Zoe', 'Adams', '1995-01-12', 'F', 'zoe.a@hospital.com', '123-456-8021', '123 Reception Blvd, City', 'Receptionist', '2021-02-15', 'Front Desk 3', 'Administration'),
('Mason', 'Baker', '1994-08-17', 'M', 'mason.b@hospital.com', '123-456-8022', '456 Staff Ave, City', 'Receptionist', '2022-04-01', 'Front Desk 4', 'Administration'),
('Charlotte', 'Nelson', '1993-11-25', 'F', 'charlotte.n@hospital.com', '123-456-8023', '789 Admin Road, City', 'Receptionist', '2020-07-20', 'Front Desk 5', 'Administration'),
('Benjamin', 'Carter', '1992-02-18', 'M', 'benjamin.c@hospital.com', '123-456-8024', '101 Admin Lane, City', 'Receptionist', '2019-12-05', 'Front Desk 6', 'Administration'),
('Amelia', 'Mitchell', '1996-03-30', 'F', 'amelia.m@hospital.com', '123-456-8025', '202 Reception Drive, City', 'Receptionist', '2021-09-18', 'Front Desk 7', 'Administration');
GO

INSERT INTO PATIENT (FIRST_NAME, LAST_NAME, DATE_OF_BIRTH, GENDER, PATIENT_EMAIL, PATIENT_PHONE, PATIENT_ADDRESS, EMERGENCY_CONTACT, REGISTRATION_DATE)
VALUES
('Noah', 'Allen', '1983-10-11 00:00:00.000', 'M', 'noah.a@email.com', '12355', '852 Hospital Rd, City', '555-0111', '2023-11-20 00:00:00.000'),
('Charlotte', 'Young', '1991-01-14 00:00:00.000', 'F', 'charlotte.y@email.com', '12356', '963 Doctor Ave, City', '555-0112', '2023-12-25 00:00:00.000'),
('James', 'Davis', '1990-05-20 00:00:00.000', 'M', 'james.d@email.com', '12357', '101 Healthcare Blvd, City', '555-0113', '2024-01-10 00:00:00.000'),
('Lily', 'Martinez', '1996-02-15 00:00:00.000', 'F', 'lily.m@email.com', '12358', '102 Patient Ln, City', '555-0114', '2024-02-18 00:00:00.000'),
('Gabriel', 'Gonzalez', '1980-08-12 00:00:00.000', 'M', 'gabriel.g@email.com', '12359', '103 Medical Park, City', '555-0115', '2024-03-20 00:00:00.000'),
('Sophia', 'Wright', '1988-11-09 00:00:00.000', 'F', 'sophia.w@email.com', '12360', '104 Clinic St, City', '555-0116', '2024-04-15 00:00:00.000'),
('Jacob', 'Lee', '1992-07-18 00:00:00.000', 'M', 'jacob.l@email.com', '12361', '105 Patient Rd, City', '555-0117', '2024-05-10 00:00:00.000'),
('Mia', 'Hernandez', '1994-09-07 00:00:00.000', 'F', 'mia.h@email.com', '12362', '106 Care Ave, City', '555-0118', '2024-06-12 00:00:00.000'),
('Ethan', 'King', '1989-12-25 00:00:00.000', 'M', 'ethan.k@email.com', '12363', '107 Doctor Dr, City', '555-0119', '2024-07-15 00:00:00.000'),
('Harper', 'Scott', '1993-04-28 00:00:00.000', 'F', 'harper.s@email.com', '12364', '108 Clinic Rd, City', '555-0120', '2024-08-18 00:00:00.000'),
('Liam', 'Adams', '1987-10-03 00:00:00.000', 'M', 'liam.a@email.com', '12365', '109 Health Blvd, City', '555-0121', '2024-09-25 00:00:00.000'),
('Chloe', 'Baker', '1986-05-11 00:00:00.000', 'F', 'chloe.b@email.com', '12366', '110 Wellness Rd, City', '555-0122', '2024-10-10 00:00:00.000'),
('Oliver', 'Graham', '1991-03-13 00:00:00.000', 'M', 'oliver.g@email.com', '12367', '111 Care Ave, City', '555-0123', '2024-11-22 00:00:00.000'),
('Zoe', 'Carter', '1992-12-25 00:00:00.000', 'F', 'zoe.c@email.com', '12368', '112 Medical Blvd, City', '555-0124', '2024-12-05 00:00:00.000'),
('Daniel', 'Morris', '1984-06-22 00:00:00.000', 'M', 'daniel.m@email.com', '12369', '113 Treatment Ave, City', '555-0125', '2025-01-10 00:00:00.000'),
('Ella', 'Evans', '1983-03-30 00:00:00.000', 'F', 'ella.e@email.com', '12370', '114 Clinic St, City', '555-0126', '2025-02-25 00:00:00.000'),
('Benjamin', 'Collins', '1990-07-17 00:00:00.000', 'M', 'benjamin.c@email.com', '12371', '115 Patient Blvd, City', '555-0127', '2025-03-20 00:00:00.000'),
('Madison', 'Roberts', '1995-10-04 00:00:00.000', 'F', 'madison.r@email.com', '12372', '116 Medical Park, City', '555-0128', '2025-04-10 00:00:00.000'),
('Jackson', 'Cook', '1991-09-13 00:00:00.000', 'M', 'jackson.c@email.com', '12373', '117 Health St, City', '555-0129', '2025-05-15 00:00:00.000'),
('Amelia', 'Parker', '1994-12-11 00:00:00.000', 'F', 'amelia.p@email.com', '12374', '118 Wellness Rd, City', '555-0130', '2025-06-10 00:00:00.000'),
('Lucas', 'Lopez', '1990-01-17 00:00:00.000', 'M', 'lucas.l@email.com', '12375', '119 Patient Blvd, City', '555-0131', '2025-07-25 00:00:00.000'),
('Avery', 'Mitchell', '1986-03-22 00:00:00.000', 'F', 'avery.m@email.com', '12376', '120 Clinic Lane, City', '555-0132', '2025-08-15 00:00:00.000'),
('William', 'Evans', '1981-02-04 00:00:00.000', 'M', 'william.e@email.com', '12377', '121 Health Rd, City', '555-0133', '2025-09-30 00:00:00.000'),
('Grace', 'Nelson', '1997-07-29 00:00:00.000', 'F', 'grace.n@email.com', '12378', '122 Wellness Blvd, City', '555-0134', '2025-10-10 00:00:00.000'),
('Henry', 'King', '1988-11-17 00:00:00.000', 'M', 'henry.k@email.com', '12379', '123 Treatment Rd, City', '555-0135', '2025-11-12 00:00:00.000');
GO

INSERT INTO PATIENTACCOUNT (PATIENT_ID, PATIENT_USERNAME, PATIENT_PASSWORD)
VALUES
(13, 'james.d', '414e7c8ede73a0c2c3d17699134f4080'),
(14, 'lily.m', '414e7c8ede73a0c2c3d17699134f4080'),
(15, 'gabriel.g', '414e7c8ede73a0c2c3d17699134f4080'),
(16, 'sophia.w', '414e7c8ede73a0c2c3d17699134f4080'),
(17, 'jacob.l', '414e7c8ede73a0c2c3d17699134f4080'),
(18, 'mia.h', '414e7c8ede73a0c2c3d17699134f4080'),
(19, 'ethan.k', '414e7c8ede73a0c2c3d17699134f4080'),
(20, 'harper.s', '414e7c8ede73a0c2c3d17699134f4080'),
(21, 'liam.a', '414e7c8ede73a0c2c3d17699134f4080'),
(22, 'chloe.b', '414e7c8ede73a0c2c3d17699134f4080'),
(23, 'oliver.g', '414e7c8ede73a0c2c3d17699134f4080'),
(24, 'zoe.c', '414e7c8ede73a0c2c3d17699134f4080'),
(25, 'daniel.m', '414e7c8ede73a0c2c3d17699134f4080'),
(26, 'ella.e', '414e7c8ede73a0c2c3d17699134f4080'),
(27, 'benjamin.c', '414e7c8ede73a0c2c3d17699134f4080'),
(28, 'madison.r', '414e7c8ede73a0c2c3d17699134f4080'),
(29, 'jackson.c', '414e7c8ede73a0c2c3d17699134f4080'),
(30, 'amelia.p', '414e7c8ede73a0c2c3d17699134f4080'),
(31, 'lucas.l', '414e7c8ede73a0c2c3d17699134f4080'),
(32, 'avery.m', '414e7c8ede73a0c2c3d17699134f4080'),
(33, 'william.e', '414e7c8ede73a0c2c3d17699134f4080'),
(34, 'grace.n', '414e7c8ede73a0c2c3d17699134f4080'),
(35, 'henry.k', '414e7c8ede73a0c2c3d17699134f4080');
GO

-- Lịch khám ngày 26/11
-- Bác sĩ 1
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(1, 1, '2024-11-26 07:00:00'),
(2, 1, '2024-11-26 08:00:00'),
(3, 1, '2024-11-26 09:00:00'),
(4, 1, '2024-11-26 10:00:00');
GO

-- Bác sĩ 2
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(5, 2, '2024-11-26 07:00:00'),
(6, 2, '2024-11-26 08:00:00'),
(7, 2, '2024-11-26 09:00:00'),
(8, 2, '2024-11-26 10:00:00'),
(9, 2, '2024-11-26 13:00:00');
GO

-- Bác sĩ 3
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(10, 3, '2024-11-26 07:00:00'),
(11, 3, '2024-11-26 08:00:00'),
(12, 3, '2024-11-26 09:00:00'),
(13, 3, '2024-11-26 10:00:00');
GO

-- Bác sĩ 4
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(14, 4, '2024-11-26 07:00:00'),
(15, 4, '2024-11-26 08:00:00'),
(16, 4, '2024-11-26 09:00:00'),
(17, 4, '2024-11-26 10:00:00'),
(18, 4, '2024-11-26 13:00:00');
GO

-- Bác sĩ 5
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(19, 5, '2024-11-26 07:00:00'),
(20, 5, '2024-11-26 08:00:00'),
(21, 5, '2024-11-26 09:00:00'),
(22, 5, '2024-11-26 10:00:00'),
(23, 5, '2024-11-26 13:00:00'),
(24, 5, '2024-11-26 14:00:00');
GO

-- Bác sĩ 6
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(25, 6, '2024-11-26 07:00:00'),
(26, 6, '2024-11-26 08:00:00'),
(27, 6, '2024-11-26 09:00:00'),
(28, 6, '2024-11-26 10:00:00'),
(29, 6, '2024-11-26 13:00:00'),
(30, 6, '2024-11-26 14:00:00');
GO

-- Lịch khám ngày 27/11
-- Bác sĩ 1
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(31, 1, '2024-11-27 07:00:00'),
(32, 1, '2024-11-27 08:00:00'),
(33, 1, '2024-11-27 09:00:00'),
(34, 1, '2024-11-27 10:00:00');
GO

-- Bác sĩ 2
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(35, 2, '2024-11-27 07:00:00'),
(36, 2, '2024-11-27 08:00:00'),
(37, 2, '2024-11-27 09:00:00'),
(38, 2, '2024-11-27 10:00:00'),
(39, 2, '2024-11-27 13:00:00');
GO

-- Bác sĩ 3
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(40, 3, '2024-11-27 07:00:00'),
(41, 3, '2024-11-27 08:00:00'),
(42, 3, '2024-11-27 09:00:00'),
(43, 3, '2024-11-27 10:00:00');
GO

-- Bác sĩ 4
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(44, 4, '2024-11-27 07:00:00'),
(45, 4, '2024-11-27 08:00:00'),
(46, 4, '2024-11-27 09:00:00'),
(47, 4, '2024-11-27 10:00:00'),
(48, 4, '2024-11-27 13:00:00');
GO
-- Bác sĩ 5
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(49, 5, '2024-11-27 07:00:00'),
(50, 5, '2024-11-27 08:00:00'),
(51, 5, '2024-11-27 09:00:00'),
(52, 5, '2024-11-27 10:00:00'),
(53, 5, '2024-11-27 13:00:00'),
(54, 5, '2024-11-27 14:00:00');
GO

-- Bác sĩ 6
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(55, 6, '2024-11-27 07:00:00'),
(56, 6, '2024-11-27 08:00:00'),
(57, 6, '2024-11-27 09:00:00'),
(58, 6, '2024-11-27 10:00:00'),
(59, 6, '2024-11-27 13:00:00'),
(60, 6, '2024-11-27 14:00:00');
GO

-- Lịch khám ngày 26/11
-- Bác sĩ 15
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(1, 15, '2024-11-26 07:00:00'),
(2, 15, '2024-11-26 08:00:00'),
(3, 15, '2024-11-26 09:00:00'),
(4, 15, '2024-11-26 10:00:00'),
(5, 15, '2024-11-26 13:00:00');
GO

-- Bác sĩ 16
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(6, 16, '2024-11-26 07:00:00'),
(7, 16, '2024-11-26 08:00:00'),
(8, 16, '2024-11-26 09:00:00'),
(9, 16, '2024-11-26 10:00:00'),
(10, 16, '2024-11-26 13:00:00'),
(11, 16, '2024-11-26 14:00:00');
GO

-- Bác sĩ 17
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(12, 17, '2024-11-26 07:00:00'),
(13, 17, '2024-11-26 08:00:00'),
(14, 17, '2024-11-26 09:00:00'),
(15, 17, '2024-11-26 10:00:00'),
(16, 17, '2024-11-26 13:00:00'),
(17, 17, '2024-11-26 14:00:00'),
(18, 17, '2024-11-26 15:00:00');
GO

-- Bác sĩ 18
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(19, 18, '2024-11-26 07:00:00'),
(20, 18, '2024-11-26 08:00:00'),
(21, 18, '2024-11-26 09:00:00'),
(22, 18, '2024-11-26 10:00:00'),
(23, 18, '2024-11-26 13:00:00'),
(24, 18, '2024-11-26 14:00:00');
GO

-- Bác sĩ 19
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(25, 19, '2024-11-26 07:00:00'),
(26, 19, '2024-11-26 08:00:00'),
(27, 19, '2024-11-26 09:00:00'),
(28, 19, '2024-11-26 10:00:00'),
(29, 19, '2024-11-26 13:00:00'),
(30, 19, '2024-11-26 14:00:00'),
(31, 19, '2024-11-26 15:00:00');
GO

-- Bác sĩ 20
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(32, 20, '2024-11-26 07:00:00'),
(33, 20, '2024-11-26 08:00:00'),
(34, 20, '2024-11-26 09:00:00'),
(35, 20, '2024-11-26 10:00:00'),
(36, 20, '2024-11-26 13:00:00'),
(37, 20, '2024-11-26 14:00:00');
GO

-- Lịch khám ngày 27/11
-- Bác sĩ 15
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(38, 15, '2024-11-27 07:00:00'),
(39, 15, '2024-11-27 08:00:00'),
(40, 15, '2024-11-27 09:00:00'),
(41, 15, '2024-11-27 10:00:00'),
(42, 15, '2024-11-27 13:00:00');
GO

-- Bác sĩ 16
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(43, 16, '2024-11-27 07:00:00'),
(44, 16, '2024-11-27 08:00:00'),
(45, 16, '2024-11-27 09:00:00'),
(46, 16, '2024-11-27 10:00:00'),
(47, 16, '2024-11-27 13:00:00'),
(48, 16, '2024-11-27 14:00:00');
GO

-- Bác sĩ 17
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(49, 17, '2024-11-27 07:00:00'),
(50, 17, '2024-11-27 08:00:00'),
(51, 17, '2024-11-27 09:00:00'),
(52, 17, '2024-11-27 10:00:00'),
(53, 17, '2024-11-27 13:00:00'),
(54, 17, '2024-11-27 14:00:00'),
(55, 17, '2024-11-27 15:00:00');
GO

-- Bác sĩ 18
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(56, 18, '2024-11-27 07:00:00'),
(57, 18, '2024-11-27 08:00:00'),
(58, 18, '2024-11-27 09:00:00'),
(59, 18, '2024-11-27 10:00:00'),
(60, 18, '2024-11-27 13:00:00');
GO

-- Bác sĩ 19
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(61, 19, '2024-11-27 07:00:00'),
(62, 19, '2024-11-27 08:00:00'),
(63, 19, '2024-11-27 09:00:00'),
(64, 19, '2024-11-27 10:00:00'),
(65, 19, '2024-11-27 13:00:00'),
(66, 19, '2024-11-27 14:00:00'),
(67, 19, '2024-11-27 15:00:00');
GO

-- Bác sĩ 20
INSERT INTO APPOINTMENT (PATIENT_ID, EMPLOYEE_ID, APPOINTMENT_TIME) VALUES
(68, 20, '2024-11-27 07:00:00'),
(69, 20, '2024-11-27 08:00:00'),
(70, 20, '2024-11-27 09:00:00'),
(71, 20, '2024-11-27 10:00:00'),
(72, 20, '2024-11-27 13:00:00');
GO