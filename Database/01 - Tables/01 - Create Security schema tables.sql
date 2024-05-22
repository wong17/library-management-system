-- CREATE Security SCHEMA TABLES
USE LibraryManagementDB
GO

-- CREATE Role TABLE
IF OBJECT_ID(N'Security.Role', 'U') IS NOT NULL
  DROP TABLE [Security].[Role]
GO
CREATE TABLE [Security].[Role] (
	RoleId INT NOT NULL IDENTITY(1,1),
	[Name] VARCHAR(100) NOT NULL,
	[Description] VARCHAR(500) NULL,
	CONSTRAINT PK_Rol_RoleId PRIMARY KEY(RoleId)
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

-- CREATE UserRole TABLE
IF OBJECT_ID(N'Security.UserRole', 'U') IS NOT NULL
  DROP TABLE [Security].[UserRole]
GO
CREATE TABLE [Security].UserRole (
	UserId INT NOT NULL,
	RoleId INT NOT NULL,
	CONSTRAINT PK_UserId_RoleId PRIMARY KEY(UserId, RoleId),
	CONSTRAINT FK_UserRol_UserId FOREIGN KEY(UserId) REFERENCES [Security].[User](UserId),
	CONSTRAINT FK_UserRol_RoleId FOREIGN KEY(RoleId) REFERENCES [Security].[Role](RoleId)
)