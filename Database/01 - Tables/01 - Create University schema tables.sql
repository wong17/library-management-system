-- CREATE University SCHEMA TABLES
USE LibraryManagementDB
GO

-- CREATE Career TABLE
IF OBJECT_ID(N'University.Career', 'U') IS NOT NULL
  DROP TABLE [University].Career
GO
--
CREATE TABLE [University].Career (
	CareerId TINYINT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(50) NOT NULL,
	CONSTRAINT PK_Career_CareerId PRIMARY KEY(CareerId),
	CONSTRAINT CK_Career_Name CHECK([Name] NOT LIKE '%[^a-zA-Z ]%'),
	CONSTRAINT AK_Career_Name UNIQUE([Name])
)
GO

-- CREATE Student TABLE
IF OBJECT_ID(N'University.Student', 'U') IS NOT NULL
  DROP TABLE [University].Student
GO
--
CREATE TABLE [University].Student (
	StudentId INT IDENTITY(1,1) NOT NULL,
	FirstName VARCHAR(15) NOT NULL,
	SecondName VARCHAR(15) NOT NULL,
	FirstLastname VARCHAR(15) NOT NULL,
	SecondLastname VARCHAR(15) NOT NULL,
	Carnet CHAR(10) NOT NULL,
	PhoneNumber CHAR(8) NOT NULL,
	Sex CHAR NOT NULL,
	Email VARCHAR(255) NOT NULL,
	[Shift] VARCHAR(10) NOT NULL,
	BorrowedBooks SMALLINT NOT NULL CONSTRAINT DF_Student_BorrowedBooks DEFAULT 0,
	HasBorrowedMonograph BIT NOT NULL CONSTRAINT DF_Student_BorrowedMonographs DEFAULT 0,
	Fine DECIMAL(7,2) NOT NULL CONSTRAINT DF_Student_Fine DEFAULT 0,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_Student_CreatedOn DEFAULT GETDATE(),
	ModifiedOn DATETIME NOT NULL CONSTRAINT DF_Student_ModifiedOn DEFAULT GETDATE(), 
	CareerId TINYINT NOT NULL,
	CONSTRAINT PK_Student_StudentId PRIMARY KEY(StudentId),
	CONSTRAINT CK_Student_FirstName CHECK(FirstName NOT LIKE '%[^a-zA-Z]%'),
	CONSTRAINT CK_Student_SecondName CHECK(SecondName NOT LIKE '%[^a-zA-Z]%'),
	CONSTRAINT CK_Student_FirstLastname CHECK(FirstLastname NOT LIKE '%[^a-zA-Z]%'),
	CONSTRAINT CK_Student_SecondLastName CHECK(SecondLastName NOT LIKE '%[^a-zA-Z]%'),
	CONSTRAINT CK_Student_Carnet CHECK(Carnet LIKE '[0-9][0-9][0-9][0-9]-[0-9][0-9][0-9][0-9][A-Z]'),
	CONSTRAINT AK_Student_Carnet UNIQUE(Carnet),
	CONSTRAINT CK_Student_PhoneNumber CHECK(PhoneNumber LIKE '[5|7|8][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
	CONSTRAINT CK_Student_Sex CHECK(Sex LIKE '[M|F]'),
	CONSTRAINT CK_Student_Email CHECK(Email LIKE '%[A-Z0-9][@][A-Z0-9]%[.][A-Z0-9]%'),
	CONSTRAINT CK_Student_Shift CHECK([Shift] IN('Diurno', 'Sabatino')),
	CONSTRAINT CK_Student_BorrowedBooks CHECK(BorrowedBooks BETWEEN 0 AND 3),
	CONSTRAINT CK_Student_Fine CHECK(Fine >= 0),
	CONSTRAINT FK_Student_Career_CareerId FOREIGN KEY(CareerId) REFERENCES [University].Career(CareerId)
)
GO