-- CREATE DATABASE
IF EXISTS (
  SELECT * 
  FROM sys.databases 
  WHERE name = N'LibraryManagementDB'
)
  DROP DATABASE LibraryManagementDB
GO
CREATE DATABASE LibraryManagementDB
GO
-- USE DATABASE
USE LibraryManagementDB
GO