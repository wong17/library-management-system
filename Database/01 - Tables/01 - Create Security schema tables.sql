-- CREATE Security SCHEMA TABLES
USE LibraryManagementDB
GO

-- CREATE Rol TABLE
IF OBJECT_ID(N'Security.Rol', 'U') IS NOT NULL
  DROP TABLE [Security].[Rol]
GO
CREATE TABLE [Security].[Rol] (
	RolId INT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(100) NOT NULL,
	[Description] VARCHAR(500) NULL,
	CONSTRAINT PK_Rol_RolId PRIMARY KEY(RolId)
)
GO

-- CREATE User TABLE
IF OBJECT_ID(N'Security.User', 'U') IS NOT NULL
  DROP TABLE [Security].[User]
GO
CREATE TABLE [Security].[User] (
	UserId INT IDENTITY(1,1) NOT NULL,
	UserName VARCHAR(50) NOT NULL,
	Email VARCHAR(250) NOT NULL,
	[Password] VARBINARY(64) NOT NULL,
	AccessToken NVARCHAR(MAX) NULL,
	RefreshToken NVARCHAR(MAX) NULL,
	RefreshTokenExpiryTime  DATETIME NULL,
	LockoutEnabled BIT NOT NULL CONSTRAINT DF_User_LockoutEnabled DEFAULT 0,
	AccessFailedCount INT NOT NULL CONSTRAINT DF_User_AccessFailedCount DEFAULT 0,
	CONSTRAINT PK_User_UserId PRIMARY KEY(UserId)
)
GO

-- CREATE UserRol TABLE
IF OBJECT_ID(N'Security.UserRol', 'U') IS NOT NULL
  DROP TABLE [Security].[UserRol]
GO
CREATE TABLE [Security].UserRol (
	UserId INT NOT NULL,
	RolId INT NOT NULL,
	CONSTRAINT PK_UserId_RolId PRIMARY KEY(UserId, RolId),
	CONSTRAINT FK_UserRol_UserId FOREIGN KEY(UserId) REFERENCES [Security].[User](UserId),
	CONSTRAINT FK_UserRol_RolId FOREIGN KEY(RolId) REFERENCES [Security].[Rol](RolId)
)