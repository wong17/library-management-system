USE LibraryManagementDB
GO
--
IF EXISTS (
    SELECT schema_id 
    FROM sys.schemas 
    WHERE name = 'University'
)
DROP SCHEMA [University]
GO
--
CREATE SCHEMA [University]
GO

--
IF EXISTS (
    SELECT schema_id 
    FROM sys.schemas 
    WHERE name = 'Library'
)
DROP SCHEMA [Library]
GO
--
CREATE SCHEMA [Library]
GO

--
IF EXISTS (
    SELECT schema_id 
    FROM sys.schemas 
    WHERE name = 'Security'
)
DROP SCHEMA [Security]
GO
--
CREATE SCHEMA [Security]
GO