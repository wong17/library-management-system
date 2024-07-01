-- CREATE TYPES
USE LibraryManagementDB
GO

-- UserRole TYPE
IF TYPE_ID (N'Security.UserRoleType') IS NOT NULL 
	DROP TYPE [Security].UserRoleType;
GO
CREATE TYPE [Security].UserRoleType AS TABLE
(
	UserId INT,
	RoleId INT
)
GO

-- Publisher TYPE
IF TYPE_ID (N'Library.PublisherType') IS NOT NULL 
	DROP TYPE [Library].PublisherType;
GO
CREATE TYPE [Library].PublisherType AS TABLE
(
	PublisherId INT,
	[Name] VARCHAR(100)
)
GO

-- Category TYPE
IF TYPE_ID (N'Library.CategoryType') IS NOT NULL 
	DROP TYPE [Library].CategoryType;
GO
CREATE TYPE [Library].CategoryType AS TABLE
(
	CategoryId INT,
	[Name] VARCHAR(100)
)
GO

-- SubCategory TYPE
IF TYPE_ID (N'Library.SubCategoryType') IS NOT NULL 
	DROP TYPE [Library].SubCategoryType;
GO
CREATE TYPE [Library].SubCategoryType AS TABLE
(
	SubCategoryId INT,
	CategoryId INT,
	[Name] VARCHAR(100)
)
GO

-- Author TYPE
IF TYPE_ID (N'Library.AuthorType') IS NOT NULL 
	DROP TYPE [Library].AuthorType;
GO
CREATE TYPE [Library].AuthorType AS TABLE
(
	AuthorId INT,
	[Name] VARCHAR(100),
	IsFormerGraduated BIT
)
GO

-- Book TYPE
IF TYPE_ID (N'Library.BookType') IS NOT NULL 
	DROP TYPE [Library].BookType;
GO
CREATE TYPE [Library].BookType AS TABLE
(
	BookId INT,
	ISBN10 VARCHAR(13),
	ISBN13 VARCHAR(17),
	[Classification] VARCHAR(25),
	Title NVARCHAR(100),
	[Description] NVARCHAR(500),
	PublicationYear SMALLINT,
	[Image] VARBINARY(MAX),
	IsActive BIT,
	PublisherId INT,
	CategoryId INT,
	NumberOfCopies SMALLINT,
	BorrowedCopies SMALLINT,
	IsAvailable BIT
)
GO

-- BookSubcategory TYPE
IF TYPE_ID (N'Library.BookSubcategoryType') IS NOT NULL 
	DROP TYPE [Library].BookSubcategoryType;
GO
CREATE TYPE [Library].BookSubcategoryType AS TABLE
(
	BookId INT,
	SubCategoryId INT,
	CreatedOn DATETIME,
	ModifiedOn DATETIME
)
GO

-- BookAuthor TYPE
IF TYPE_ID (N'Library.BookAuthorType') IS NOT NULL 
	DROP TYPE [Library].BookAuthorType;
GO
CREATE TYPE [Library].BookAuthorType AS TABLE 
(
	BookId INT,
	AuthorId INT,
	CreatedOn DATETIME,
	ModifiedOn DATETIME
)
GO

-- BookLoan TYPE
IF TYPE_ID (N'Library.BookLoanType') IS NOT NULL 
	DROP TYPE [Library].BookLoanType;
GO
CREATE TYPE [Library].BookLoanType AS TABLE 
(
	BookLoanId INT,
	StudentId INT,
	BookId INT,
	TypeOfLoan VARCHAR(10),
	[State] CHAR(9),
	LoanDate DATETIME,
	DueDate DATETIME,
	ReturnDate DATETIME
)
GO

-- Monograph TYPE
IF TYPE_ID (N'Library.MonographType') IS NOT NULL 
	DROP TYPE [Library].MonographType;
GO
CREATE TYPE [Library].MonographType AS TABLE 
(
	MonographId INT,
	[Classification] VARCHAR(25),
	Title NVARCHAR(250),
	[Description] NVARCHAR(500),
	Tutor NVARCHAR(100),
	PresentationDate DATE,
	CareerId TINYINT,
	IsActive BIT,
	IsAvailable BIT
)
GO

-- MonographLoan TYPE
IF TYPE_ID (N'Library.MonographLoanType') IS NOT NULL 
	DROP TYPE [Library].MonographLoanType;
GO
CREATE TYPE [Library].MonographLoanType AS TABLE 
(
	MonographLoanId INT,
	StudentId INT,
	MonographId INT,
	[State] CHAR(9),
	LoanDate DATETIME,
	DueDate DATETIME,
	ReturnDate DATETIME
)
GO

-- MonographAuthor TYPE
IF TYPE_ID (N'Library.MonographAuthorType') IS NOT NULL 
	DROP TYPE [Library].MonographAuthorType;
GO
CREATE TYPE [Library].MonographAuthorType AS TABLE 
(
	MonographId INT,
	AuthorId INT,
	CreatedOn DATETIME,
	ModifiedOn DATETIME
)
GO

-- Career TYPE
IF TYPE_ID (N'University.CareerType') IS NOT NULL 
	DROP TYPE [University].CareerType;
GO
CREATE TYPE [University].CareerType AS TABLE 
(
	CareerId TINYINT,
	[Name] VARCHAR(50)
)
GO