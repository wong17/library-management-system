USE LibraryManagementDB
GO
--
IF EXISTS (
    SELECT schema_id 
    FROM sys.schemas 
    WHERE name = 'University'
)
DROP SCHEMA University
GO
--
CREATE SCHEMA University
GO

--
IF EXISTS (
    SELECT schema_id 
    FROM sys.schemas 
    WHERE name = 'Library'
)
DROP SCHEMA Library
GO
--
CREATE SCHEMA Library
GO