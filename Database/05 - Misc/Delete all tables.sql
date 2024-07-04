-- DELETE ALL TABLES
USE LibraryManagementDB
GO

-- CREATE MonographAuthor TABLE
IF OBJECT_ID(N'Library.MonographAuthor', 'U') IS NOT NULL
  DROP TABLE [Library].MonographAuthor
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

-- CREATE BookAuthor TABLE
IF OBJECT_ID(N'Library.BookAuthor', 'U') IS NOT NULL
  DROP TABLE [Library].BookAuthor
GO


-- CREATE BookSubcategory TABLE
IF OBJECT_ID(N'Library.BookSubCategory', 'U') IS NOT NULL
  DROP TABLE [Library].BookSubCategory
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

-- CREATE Author TABLE
IF OBJECT_ID(N'Library.Author', 'U') IS NOT NULL
  DROP TABLE [Library].Author
GO

-- CREATE SubCategory TABLE
IF OBJECT_ID(N'Library.SubCategory', 'U') IS NOT NULL
  DROP TABLE [Library].SubCategory
GO

-- CREATE Category TABLE
IF OBJECT_ID(N'Library.Category', 'U') IS NOT NULL
  DROP TABLE [Library].Category
GO

-- CREATE Publisher TABLE
IF OBJECT_ID(N'Library.Publisher', 'U') IS NOT NULL
  DROP TABLE [Library].Publisher
GO

-- CREATE Student TABLE
IF OBJECT_ID(N'University.Student', 'U') IS NOT NULL
  DROP TABLE [University].Student
GO

-- CREATE Career TABLE
IF OBJECT_ID(N'University.Career', 'U') IS NOT NULL
  DROP TABLE [University].Career
GO

-- CREATE UserRole TABLE
IF OBJECT_ID(N'Security.UserRole', 'U') IS NOT NULL
  DROP TABLE [Security].[UserRole]
GO

-- CREATE User TABLE
IF OBJECT_ID(N'Security.User', 'U') IS NOT NULL
  DROP TABLE [Security].[User]
GO

-- CREATE Role TABLE
IF OBJECT_ID(N'Security.Role', 'U') IS NOT NULL
  DROP TABLE [Security].[Role]
GO