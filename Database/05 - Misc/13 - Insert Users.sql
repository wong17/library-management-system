-- INSERT Users
USE LibraryManagementDB
GO

EXEC [Security].uspInsertUser 'admin', 'admintemp@gmail.com', 'Abcd1234', 1
GO
EXEC [Security].uspInsertUser 'bibliotecario', 'bibliotecariotemp@gmail.com', 'Abcd1234', 2
GO
EXEC [Security].uspInsertUser 'estudiante', 'estudiantetemp@gmail.com', 'Abcd1234', 3
GO