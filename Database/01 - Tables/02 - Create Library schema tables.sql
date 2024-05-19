-- CREATE Library SCHEMA TABLES
USE LibraryManagementDB
GO

-- CREATE Publisher TABLE
IF OBJECT_ID(N'Library.Publisher', 'U') IS NOT NULL
  DROP TABLE [Library].Publisher
GO
--
CREATE TABLE [Library].Publisher (
	PublisherId INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	CONSTRAINT PK_Publisher_PublisherId PRIMARY KEY(PublisherId),
	CONSTRAINT CK_Publisher_Name CHECK([NAME] NOT LIKE '%[^a-zA-Z-. ]%'),
	CONSTRAINT AK_Publisher_Name UNIQUE([Name])
)
GO

-- CREATE Category TABLE
IF OBJECT_ID(N'Library.Category', 'U') IS NOT NULL
  DROP TABLE [Library].Category
GO
--
CREATE TABLE [Library].Category (
	CategoryId INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	CONSTRAINT PK_Category_CategoryId PRIMARY KEY(CategoryId),
	CONSTRAINT CK_Category_Name CHECK([Name] NOT LIKE '%[^a-zA-Z0-9- ]%'),
	CONSTRAINT AK_Category_Name UNIQUE([Name])
)
GO

-- CREATE SubCategory TABLE
IF OBJECT_ID(N'Library.SubCategory', 'U') IS NOT NULL
  DROP TABLE [Library].SubCategory
GO
--
CREATE TABLE [Library].SubCategory (
	SubCategoryId INT IDENTITY(1,1) NOT NULL,
	CategoryId INT NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	CONSTRAINT PK_Subcategory_SubCategoryId PRIMARY KEY(SubCategoryId),
	CONSTRAINT FK_Subcategory_Category_CategoryId FOREIGN KEY(CategoryId) REFERENCES [Library].Category(CategoryId),
	CONSTRAINT CK_SubCategory_Name CHECK([Name] NOT LIKE '%[^a-zA-Z0-9- ]%'),
	CONSTRAINT AK_SubCategory_Name UNIQUE([Name])
)
GO

-- CREATE Author TABLE
IF OBJECT_ID(N'Library.Author', 'U') IS NOT NULL
  DROP TABLE [Library].Author
GO
--
CREATE TABLE [Library].Author (
	AuthorId INT IDENTITY(1,1) NOT NULL,
	[Name] VARCHAR(100) NOT NULL,
	IsFormerGraduated BIT NOT NULL CONSTRAINT DF_Author_IsFormerGraduated DEFAULT 0,
	CONSTRAINT PK_Author_AuthorId PRIMARY KEY(AuthorId),
	CONSTRAINT CK_Author_Name CHECK([Name] NOT LIKE '%[^a-zA-Z. ]%'),
	CONSTRAINT AK_Author_Name UNIQUE([Name])
)
GO

-- CREATE Book TABLE
BEGIN
    --If table is system-versioned, SYSTEM_VERSIONING must be set to OFF first 
    IF ((SELECT temporal_type FROM sys.tables WHERE object_id = OBJECT_ID('Library.Book', 'U')) = 2)
    BEGIN
        ALTER TABLE [Library].[Book] SET (SYSTEM_VERSIONING = OFF)
    END
    DROP TABLE IF EXISTS [Library].[Book]
	DROP TABLE IF EXISTS [Library].[BookHistory]
END
GO
--
CREATE TABLE [Library].Book (
	BookId INT IDENTITY(1,1) NOT NULL,
	ISBN10 VARCHAR(13) NOT NULL,
	ISBN13 VARCHAR(17) NOT NULL,
	[Classification] VARCHAR(25) NOT NULL,
	Title NVARCHAR(100) NOT NULL,
	[Description] NVARCHAR(500),
	PublicationYear SMALLINT NOT NULL,
	[Image] VARBINARY(MAX),
	IsActive BIT NOT NULL CONSTRAINT DF_Book_Active DEFAULT 1,
	PublisherId INT NOT NULL,
	CategoryId INT NOT NULL,
	NumberOfCopies SMALLINT NOT NULL,
	BorrowedCopies SMALLINT NOT NULL CONSTRAINT DF_Book_BorrowedCopies DEFAULT 0,
	IsAvailable BIT NOT NULL CONSTRAINT DF_Book_Available DEFAULT 1,
	
    ValidFrom datetime2(7) GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo datetime2(7) GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME(ValidFrom,ValidTo),

	CONSTRAINT PK_Book_BookId PRIMARY KEY(BookId),
	CONSTRAINT CK_Book_ISBN10 CHECK(ISBN10 NOT LIKE '%[^0-9-]%'),
	CONSTRAINT AK_Book_ISBN10 UNIQUE(ISBN10),
	CONSTRAINT CK_Book_ISBN13 CHECK(ISBN13 NOT LIKE '%[^0-9-]%'),
	CONSTRAINT AK_Book_ISBN13 UNIQUE(ISBN13),
	CONSTRAINT CK_Book_Classification CHECK([Classification] NOT LIKE '%[^a-zA-Z0-9-. ]%'),
	CONSTRAINT AK_Book_Classification UNIQUE([Classification]),
	CONSTRAINT CK_Book_Title CHECK(Title NOT LIKE '%[^a-zA-Z0-9-. ]%'),
	CONSTRAINT AK_Book_Title UNIQUE(Title),
	CONSTRAINT CK_Book_PublicationYear CHECK (PublicationYear > 0),
	CONSTRAINT FK_Book_Publisher_PublisherId FOREIGN KEY(PublisherId) REFERENCES [Library].Publisher(PublisherId),
	CONSTRAINT FK_Book_Category_CategoryId FOREIGN KEY(CategoryId) REFERENCES [Library].Category(CategoryId),
	CONSTRAINT CK_Book_NumberOfCopies CHECK(NumberOfCopies >= 0),
	CONSTRAINT CK_Book_BorrowedCopies CHECK(BorrowedCopies >= 0)
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [Library].[BookHistory], DATA_CONSISTENCY_CHECK = ON)
)
GO

-- CREATE BookSubcategory TABLE
IF OBJECT_ID(N'Library.BookSubCategory', 'U') IS NOT NULL
  DROP TABLE [Library].BookSubCategory
GO
CREATE TABLE [Library].BookSubCategory (
	BookId INT NOT NULL,
	SubCategoryId INT NOT NULL,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_BookSubCategory_CreatedOn DEFAULT GETDATE(),
	ModifiedOn DATETIME NOT NULL CONSTRAINT DF_BookSubCategory_ModifiedOn DEFAULT GETDATE(),
	CONSTRAINT PK_BookSubCategory_BookId_SubCategoryId PRIMARY KEY (BookId, SubCategoryId),
	CONSTRAINT FK_BookSubCategory_Book_BookId FOREIGN KEY(BookId) REFERENCES [Library].Book(BookId),
	CONSTRAINT FK_BookSubCategory_SubCategory_SubCategoryId FOREIGN KEY(SubCategoryId) REFERENCES [Library].SubCategory(SubCategoryId)
)
GO

-- CREATE BookAuthor TABLE
IF OBJECT_ID(N'Library.BookAuthor', 'U') IS NOT NULL
  DROP TABLE [Library].BookAuthor
GO
--
CREATE TABLE [Library].BookAuthor (
	BookId INT NOT NULL,
	AuthorId INT NOT NULL,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_BookAuthor_CreatedOn DEFAULT GETDATE(),
	ModifiedOn DATETIME NOT NULL CONSTRAINT DF_BookAuthor_ModifiedOn DEFAULT GETDATE(),
	CONSTRAINT PK_BookAuthor_BookId_AuthorId PRIMARY KEY(BookId, AuthorId),
	CONSTRAINT FK_BookAuthor_Book_BookId FOREIGN KEY(BookId) REFERENCES [Library].Book(BookId),
	CONSTRAINT FK_BookAuthor_Author_AuthorId FOREIGN KEY(AuthorId) REFERENCES [Library].Author(AuthorId)
)
GO

-- CREATE BookLoan TABLE
BEGIN
    --If table is system-versioned, SYSTEM_VERSIONING must be set to OFF first 
    IF ((SELECT temporal_type FROM sys.tables WHERE object_id = OBJECT_ID('Library.BookLoan', 'U')) = 2)
    BEGIN
        ALTER TABLE [Library].[BookLoan] SET (SYSTEM_VERSIONING = OFF)
    END
    DROP TABLE IF EXISTS [Library].[BookLoan]
	DROP TABLE IF EXISTS [Library].[BookLoanHistory]
END
GO
--
CREATE TABLE [Library].BookLoan (
	BookLoanId INT IDENTITY(1,1) NOT NULL,
	StudentId INT NOT NULL,
	BookId INT NOT NULL,
	TypeOfLoan VARCHAR(10) NOT NULL,
	[State] CHAR(9) NOT NULL CONSTRAINT DF_BookLoan_State DEFAULT ('CREADA'),
	LoanDate DATETIME NOT NULL CONSTRAINT DF_BookLoan_LoanDate DEFAULT GETDATE(),
	DueDate DATETIME NULL,
	ReturnDate DATETIME NULL,

	ValidFrom datetime2(7) GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo datetime2(7) GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME(ValidFrom,ValidTo),

	CONSTRAINT PK_BookLoan_BookLoanId PRIMARY KEY(BookLoanId),
	CONSTRAINT FK_BookLoan_Student_StudentId FOREIGN KEY(StudentId) REFERENCES [University].Student(StudentId),
	CONSTRAINT FK_BookLoan_Book_BookId FOREIGN KEY(BookId) REFERENCES [Library].Book(BookId),
	CONSTRAINT CK_BookLoan_TypeOfLoan CHECK(TypeOfLoan IN('SALA','DOMICILIO')),
	CONSTRAINT CK_BookLoan_State CHECK([State] IN('CREADA', 'ELIMINADA', 'PRESTADO', 'DEVUELTO'))
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [Library].[BookLoanHistory], DATA_CONSISTENCY_CHECK = ON)
)
GO

-- CREATE Monograph TABLE
BEGIN
    --If table is system-versioned, SYSTEM_VERSIONING must be set to OFF first 
    IF ((SELECT temporal_type FROM sys.tables WHERE object_id = OBJECT_ID('Library.Monograph', 'U')) = 2)
    BEGIN
        ALTER TABLE [Library].[Monograph] SET (SYSTEM_VERSIONING = OFF)
    END
    DROP TABLE IF EXISTS [Library].[Monograph]
	DROP TABLE IF EXISTS [Library].[MonographHistory]
END
GO
--
CREATE TABLE [Library].Monograph (
	MonographId INT IDENTITY(1,1) NOT NULL,
	[Classification] VARCHAR(25) NOT NULL,
	Title NVARCHAR(250) NOT NULL,
	[Description] NVARCHAR(500) NOT NULL,
	Tutor NVARCHAR(100) NOT NULL,
	PresentationDate DATE NOT NULL,
	[Image] VARBINARY(MAX),
	CareerId TINYINT NOT NULL,
	IsActive BIT NOT NULL CONSTRAINT DF_Monograph_Active DEFAULT 1,
	IsAvailable BIT NOT NULL CONSTRAINT DF_Monograph_Available DEFAULT 1,

	ValidFrom datetime2(7) GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo datetime2(7) GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME(ValidFrom,ValidTo),

	CONSTRAINT PK_Monograph_MonographId PRIMARY KEY(MonographId),
	CONSTRAINT AK_Monograph_NoRegister UNIQUE([Classification]),
	CONSTRAINT CK_Monograph_Title CHECK(Title NOT LIKE '%[^a-zA-Z0-9-. ]%'),
	CONSTRAINT CK_Monograph_Topic CHECK([Description] NOT LIKE '%[^a-zA-Z0-9-. ]%'),
	CONSTRAINT CK_Monograph_Tutor CHECK(Tutor NOT LIKE '%[^a-zA-Z. ]%'),
	CONSTRAINT FK_Monograph_Career_CareerId FOREIGN KEY(CareerId) REFERENCES [University].Career(CareerId)
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [Library].[MonographHistory], DATA_CONSISTENCY_CHECK = ON)
)
GO

-- CREATE MonographLoan TABLE
BEGIN
    --If table is system-versioned, SYSTEM_VERSIONING must be set to OFF first 
    IF ((SELECT temporal_type FROM sys.tables WHERE object_id = OBJECT_ID('Library.MonographLoan', 'U')) = 2)
    BEGIN
        ALTER TABLE [Library].[MonographLoan] SET (SYSTEM_VERSIONING = OFF)
    END
    DROP TABLE IF EXISTS [Library].[MonographLoan]
	DROP TABLE IF EXISTS [Library].[MonographLoanHistory]
END
GO
--
CREATE TABLE [Library].MonographLoan (
	MonographLoanId INT IDENTITY(1,1) NOT NULL,
	StudentId INT NOT NULL,
	MonographId INT NOT NULL,
	[State] CHAR(9) NOT NULL CONSTRAINT DF_MonographLoan_State DEFAULT ('CREADA'),
	LoanDate DATETIME NOT NULL CONSTRAINT DF_MonographLoan_LoanDate DEFAULT GETDATE(),
	DueDate DATETIME NULL,
	ReturnDate DATETIME NULL,

	ValidFrom datetime2(7) GENERATED ALWAYS AS ROW START NOT NULL,
    ValidTo datetime2(7) GENERATED ALWAYS AS ROW END NOT NULL,
    PERIOD FOR SYSTEM_TIME(ValidFrom,ValidTo),

	CONSTRAINT PK_MonographLoan_MonographLoanId PRIMARY KEY(MonographLoanId),
	CONSTRAINT FK_MonographLoan_Student_StudentId FOREIGN KEY(StudentId) REFERENCES [University].Student(StudentId),
	CONSTRAINT FK_MonographLoan_Monograph_MonographId FOREIGN KEY(MonographId) REFERENCES [Library].Monograph(MonographId),
	CONSTRAINT CK_MonographLoan_State CHECK([State] IN('CREADA', 'ELIMINADA', 'PRESTADA', 'DEVUELTA'))
)
WITH
(
    SYSTEM_VERSIONING = ON (HISTORY_TABLE = [Library].[MonographLoanHistory], DATA_CONSISTENCY_CHECK = ON)
)
GO

-- CREATE MonographAuthor TABLE
IF OBJECT_ID(N'Library.MonographAuthor', 'U') IS NOT NULL
  DROP TABLE [Library].MonographAuthor
GO
--
CREATE TABLE [Library].MonographAuthor (
	MonographId INT NOT NULL,
	AuthorId INT NOT NULL,
	CreatedOn DATETIME NOT NULL CONSTRAINT DF_MonographAuthor_CreatedOn DEFAULT GETDATE(),
	ModifiedOn DATETIME NOT NULL CONSTRAINT DF_MonographAuthor_ModifiedOn DEFAULT GETDATE(),
	CONSTRAINT PK_MonographAuthor_MonographId_AuthorId PRIMARY KEY(MonographId, AuthorId),
	CONSTRAINT FK_MonographAuthor_Monograph_MonographId FOREIGN KEY(MonographId) REFERENCES [Library].Monograph(MonographId),
	CONSTRAINT FK_MonographAuthor_Author_AuthorId FOREIGN KEY(AuthorId) REFERENCES [Library].Author(AuthorId)
)
GO